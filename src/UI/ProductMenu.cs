using Spectre.Console;
using StoreOps.Models;
using StoreOps.Services;
using System;

namespace StoreOps.UI
{
    class ProductMenu
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly CustomerService _customerService;
        private readonly SaleService _saleService;
        private readonly EmailService _emailService;

        public ProductMenu(
            ProductService productService,
            CategoryService categoryService,
            CustomerService customerService,
            SaleService saleService,
            EmailService emailService
        )
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
            _saleService = saleService;
            _emailService = emailService;
        }

        [Obsolete]
        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Render(new FigletText("Products"));
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                AnsiConsole.MarkupLine("[bold blue]             Menu de Produtos           [/]");
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Escolha uma opção:")
                        .AddChoices(new[] { "Registrar Produto", "Ver Produtos", "Vender Produto", "Voltar ao Menu Principal" }));

                switch (option)
                {
                    case "Registrar Produto":
                        AddProduct();
                        break;
                    case "Ver Produtos":
                        ViewProducts();
                        break;
                    case "Vender Produto":
                        SellProduct();
                        break;
                    case "Voltar ao Menu Principal":
                        return;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Opção inválida![/]");
                        break;
                }
            }
        }

        private void ViewProducts()
        {
            Console.WriteLine("1 - Ver Todos os Produtos");
            Console.WriteLine("2 - Pesquisar Produtos");
            string option = Console.ReadLine() ?? string.Empty;

            if (option == "1")
            {
                var products = _productService.GetProducts();
                foreach (var product in products)
                {
                    Console.WriteLine(
                        $"{product.Name} - {product.Description} - {product.Price:C} - {product.Stock} unidades"
                    );
                }
            }
            else if (option == "2")
            {
                Console.WriteLine("Informe a busca:");
                string search = Console.ReadLine() ?? string.Empty;
                var products = _productService.SearchProducts(search);
                foreach (var product in products)
                {
                    Console.WriteLine(
                        $"{product.Name} - {product.Description} - {product.Price:C} - {product.Stock} unidades"
                    );
                }
            }
            else
            {
                Console.WriteLine("Opção inválida!");
            }
        }

        private void AddProduct()
        {
            try
            {
                Console.WriteLine("Informe o nome do produto:");
                string name = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Informe a descrição do produto:");
                string description = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Informe o preço do produto:");
                if (!decimal.TryParse(Console.ReadLine() ?? string.Empty, out decimal price))
                {
                    Console.WriteLine("Preço inválido!, apenas números.ok tks");
                    return;
                }

                Console.WriteLine("Informe o estoque do produto:");
                if (!int.TryParse(Console.ReadLine() ?? string.Empty, out int stock))
                {
                    Console.WriteLine("Estoque inválido!, apenas números");
                    return;
                }

                Console.WriteLine("Selecione a categoria do produto:");
                var categories = _categoryService.GetCategories();
                for (int i = 0; i < categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {categories[i].Name}");
                }

                string selectedCategoryIndexInput = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(selectedCategoryIndexInput, out int selectedCategoryIndex))
                {
                    selectedCategoryIndex -= 1;
                    if (selectedCategoryIndex >= 0 && selectedCategoryIndex < categories.Count)
                    {
                        var selectedCategory = categories[selectedCategoryIndex];

                        Product product =
                            new()
                            {
                                Id = null,
                                Name = name,
                                Description = description,
                                Price = price,
                                Stock = stock,
                                CategoryId = selectedCategory.Id,
                                Category = selectedCategory
                            };

                        _productService.AddProduct(product);

                        Console.WriteLine("Produto adicionado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Índice fora dos limites!");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar produto: {ex.Message}");
            }
        }

        private void SellProduct()
        {
            try
            {
                Console.WriteLine("Selecione um produto para vender:");
                var products = _productService.GetProducts();
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1} - {products[i].Name} - {products[i].Price:C} - {products[i].Stock} unidades"
                    );
                }

                string selectedProductIndexInput = Console.ReadLine() ?? string.Empty;
                if (
                    int.TryParse(selectedProductIndexInput, out int selectedProductIndex)
                    && selectedProductIndex > 0
                    && selectedProductIndex <= products.Count
                )
                {
                    selectedProductIndex -= 1;
                    var selectedProduct = products[selectedProductIndex];

                    Console.WriteLine("Selecione um cliente:");
                    var customers = _customerService.GetCustomers();
                    for (int i = 0; i < customers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - {customers[i].Name}");
                    }

                    string selectedCustomerIndexInput = Console.ReadLine() ?? string.Empty;
                    if (
                        int.TryParse(selectedCustomerIndexInput, out int selectedCustomerIndex)
                        && selectedCustomerIndex > 0
                        && selectedCustomerIndex <= customers.Count
                    )
                    {
                        selectedCustomerIndex -= 1;
                        var selectedCustomer = customers[selectedCustomerIndex];

                        Console.WriteLine(
                            "Selecione um método de pagamento (1 - Dinheiro, 2 - Cartão):"
                        );
                        string paymentMethodInput = Console.ReadLine() ?? string.Empty;
                        PaymentMethod paymentMethod =
                            paymentMethodInput == "1"
                                ? PaymentMethod.Dinheiro
                                : PaymentMethod.Cartao;

                        string productId = selectedProduct.Id.ToString();
                        string customerId = selectedCustomer.Id.ToString();

                        _saleService.CreateSale(productId, customerId, paymentMethod);
                    }
                    else
                    {
                        Console.WriteLine("Índice de cliente fora dos limites!");
                    }
                }
                else
                {
                    Console.WriteLine("Índice de produto fora dos limites!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao vender produto: {ex.Message}");
            }
        }
    }
}
