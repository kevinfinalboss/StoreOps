using Spectre.Console;
using StoreOps.Models;
using StoreOps.Services;

namespace StoreOps.UI
{
    class CustomerMenu
    {
        private readonly CustomerService _customerService;

        public CustomerMenu(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [Obsolete]
        public async Task ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Render(new FigletText("Customers"));
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                AnsiConsole.MarkupLine("[bold blue]           Menu de Clientes             [/]");
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Escolha uma opção:")
                        .AddChoices(
                            new[]
                            {
                                "Adicionar Cliente",
                                "Ver Clientes",
                                "Deletar Clientes",
                                "Voltar ao Menu Principal"
                            }
                        )
                );

                switch (option)
                {
                    case "Adicionar Cliente":
                        await AddCustomer();
                        break;
                    case "Ver Clientes":
                        ViewCustomers();
                        break;
                    case "Deletar Clientes":
                        DeleteCustomer();
                        break;
                    case "Voltar ao Menu Principal":
                        return;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Opção inválida![/]");
                        break;
                }
            }
        }

        private async Task AddCustomer()
        {
            string name = AnsiConsole.Ask<string>("Informe o nome do cliente:");

            int age;
            while (true)
            {
                string ageString = AnsiConsole.Ask<string>("Informe a idade do cliente:");
                if (int.TryParse(ageString, out age))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Idade inválida, apenas números![/]");
                }
            }

            string cpf = AnsiConsole.Ask<string>("Informe o CPF do cliente:");
            string email = AnsiConsole.Ask<string>("Informe o e-mail do cliente:");
            string phoneNumber = AnsiConsole.Ask<string>(
                "Informe o número de telefone do cliente:"
            );

            Customer customer =
                new()
                {
                    Name = name,
                    Age = age,
                    CPF = cpf,
                    Email = email,
                    PhoneNumber = phoneNumber
                };

            _customerService.AddCustomer(customer);

            AnsiConsole.MarkupLine("[green]Cliente adicionado com sucesso![/]");

            var emailService = new EmailService();
            string subject = "Bem-vindo ao StoreOps!";
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            for (int attempt = 0; attempt < 2; attempt++)
            {
                try
                {
                    await emailService.SendEmailAsync(
                        email,
                        subject,
                        name,
                        cancellationTokenSource.Token
                    );
                    AnsiConsole.MarkupLine("[green]E-mail enviado com sucesso![/]");
                    break;
                }
                catch (OperationCanceledException)
                {
                    AnsiConsole.MarkupLine(
                        "[red]Erro ao enviar e-mail: a operação atingiu o tempo limite.[/]"
                    );
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[red]Erro ao enviar e-mail: {ex.Message}[/]");
                }
            }
        }

        private void ViewCustomers()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Escolha uma opção:")
                    .AddChoices(
                        new[]
                        {
                            "Ver todos os clientes",
                            "Pesquisar cliente",
                            "Gerar relatório de clientes"
                        }
                    )
            );

            switch (option)
            {
                case "Ver todos os clientes":
                    var allCustomers = _customerService.GetCustomers();
                    foreach (var customer in allCustomers)
                    {
                        AnsiConsole.MarkupLine(
                            $"Nome: {customer.Name}, Idade: {customer.Age}, CPF: {customer.CPF}, Email: {customer.Email}, Telefone: {customer.PhoneNumber}"
                        );
                    }
                    break;
                case "Pesquisar cliente":
                    string search = AnsiConsole.Ask<string>(
                        "Digite a pesquisa (nome, idade, CPF, email, telefone):"
                    );
                    var customers = _customerService.SearchCustomers(search);
                    foreach (var customer in customers)
                    {
                        AnsiConsole.MarkupLine(
                            $"Nome: {customer.Name}, Idade: {customer.Age}, CPF: {customer.CPF}, Email: {customer.Email}, Telefone: {customer.PhoneNumber}"
                        );
                    }
                    break;
                case "Gerar relatório de clientes":
                    _customerService.GenerateCustomerReport();
                    string folderPath = Path.Combine("relatorio", "usuarios");
                    AnsiConsole.MarkupLine(
                        $"[green]Relatório gerado com sucesso em {folderPath}[/]"
                    );
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Opção inválida![/]");
                    break;
            }
        }

        private void DeleteCustomer() { }
    }
}
