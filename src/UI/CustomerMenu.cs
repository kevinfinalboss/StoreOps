using Spectre.Console;
using StoreOps.Models;
using StoreOps.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoreOps.UI
{
    class CustomerMenu
    {
        private readonly CustomerService _customerService;

        public CustomerMenu(CustomerService customerService)
        {
            _customerService = customerService;
        }

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
            Console.WriteLine("Informe o nome do cliente:");
            string name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Informe a idade do cliente:");
            if (!int.TryParse(Console.ReadLine() ?? string.Empty, out int age))
            {
                Console.WriteLine("Idade inválida, apenas números!");
                return;
            }

            Console.WriteLine("Informe o CPF do cliente:");
            string cpf = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Informe o e-mail do cliente:");
            string email = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Informe o número de telefone do cliente:");
            string phoneNumber = Console.ReadLine() ?? string.Empty;

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

            Console.WriteLine("Cliente adicionado com sucesso!");

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
                    Console.WriteLine("E-mail enviado com sucesso!");
                    break;
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Erro ao enviar e-mail: a operação atingiu o tempo limite.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                }
            }
        }

        private void ViewCustomers()
        {
            Console.WriteLine("1 - Ver todos os clientes");
            Console.WriteLine("2 - Pesquisar cliente");
            Console.WriteLine("3 - Gerar relatório de clientes");
            string option = Console.ReadLine() ?? string.Empty;

            switch (option)
            {
                case "1":
                    var allCustomers = _customerService.GetCustomers();
                    foreach (var customer in allCustomers)
                    {
                        Console.WriteLine(
                            $"Nome: {customer.Name}, Idade: {customer.Age}, CPF: {customer.CPF}, Email: {customer.Email}, Telefone: {customer.PhoneNumber}"
                        );
                    }
                    break;
                case "2":
                    Console.WriteLine("Digite a pesquisa (nome, idade, CPF, email, telefone):");
                    string search = Console.ReadLine() ?? string.Empty;
                    var customers = _customerService.SearchCustomers(search);
                    foreach (var customer in customers)
                    {
                        Console.WriteLine(
                            $"Nome: {customer.Name}, Idade: {customer.Age}, CPF: {customer.CPF}, Email: {customer.Email}, Telefone: {customer.PhoneNumber}"
                        );
                    }
                    break;
                case "3":
                    _customerService.GenerateCustomerReport();
                    string folderPath = Path.Combine("relatorio", "usuarios");
                    Console.WriteLine($"Relatório gerado com sucesso em {folderPath}");
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
        }

        private void DeleteCustomer() { }
    }
}
