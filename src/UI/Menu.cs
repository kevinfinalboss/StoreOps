using Spectre.Console;
using StoreOps.Services;

namespace StoreOps.UI
{
    class Menu
    {
        private readonly ProductMenu _productMenu;
        private readonly CustomerMenu _customerMenu;
        private readonly ConfigurationMenu _configurationMenu;

        public Menu(CategoryService categoryService, ProductService productService, CustomerService customerService, SaleService saleService, EmailService emailService)
        {
            _productMenu = new ProductMenu(productService, categoryService, customerService, saleService, emailService);
            _customerMenu = new CustomerMenu(customerService, emailService);
            _configurationMenu = new ConfigurationMenu(categoryService);
        }

        [Obsolete]
        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Render(new FigletText("StoreOps"));
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                AnsiConsole.MarkupLine("[bold blue]               Menu Principal           [/]");
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Escolha uma opção:")
                        .AddChoices(new[] { "Produtos", "Clientes", "Configurações", "Sair" }));

                switch (option)
                {
                    case "Produtos":
                        _productMenu.ShowMenu();
                        break;
                    case "Clientes":
                        _customerMenu.ShowMenu().GetAwaiter().GetResult();
                        break;
                    case "Configurações":
                        _configurationMenu.ShowMenu();
                        break;
                    case "Sair":
                        return;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Opção inválida![/]");
                        break;
                }
            }
        }
    }
}
