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
            _customerMenu = new CustomerMenu(customerService);
            _configurationMenu = new ConfigurationMenu(categoryService);
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu Principal:");
                Console.WriteLine("1 - Produtos");
                Console.WriteLine("2 - Clientes");
                Console.WriteLine("3 - Configurações");
                Console.WriteLine("0 - Sair");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        _productMenu.ShowMenu();
                        break;
                    case "2":
                        _customerMenu.ShowMenu().GetAwaiter().GetResult();
                        break;
                    case "3":
                        _configurationMenu.ShowMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }
    }
}
