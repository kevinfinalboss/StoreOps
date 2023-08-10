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

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu de Produtos:");
                Console.WriteLine("1 - Registrar Produto");
                Console.WriteLine("2 - Ver Produtos");
                Console.WriteLine("3 - Vender Produto");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        ViewProducts();
                        break;
                    case "3":
                        SellProduct();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
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
                string priceInput = Console.ReadLine() ?? string.Empty;
                decimal price = decimal.Parse(priceInput);

                Console.WriteLine("Informe o estoque do produto:");
                string stockInput = Console.ReadLine() ?? string.Empty;
                int stock = int.Parse(stockInput);

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
                    Console.WriteLine($"{i + 1} - {products[i].Name} - {products[i].Price:C} - {products[i].Stock} unidades");
                }

                string selectedProductIndexInput = Console.ReadLine() ?? string.Empty;
                if (int.TryParse(selectedProductIndexInput, out int selectedProductIndex) && selectedProductIndex > 0 && selectedProductIndex <= products.Count)
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
                    if (int.TryParse(selectedCustomerIndexInput, out int selectedCustomerIndex) && selectedCustomerIndex > 0 && selectedCustomerIndex <= customers.Count)
                    {
                        selectedCustomerIndex -= 1;
                        var selectedCustomer = customers[selectedCustomerIndex];

                        Console.WriteLine("Selecione um método de pagamento (1 - Dinheiro, 2 - Cartão):");
                        string paymentMethodInput = Console.ReadLine() ?? string.Empty;
                        PaymentMethod paymentMethod = paymentMethodInput == "1" ? PaymentMethod.Dinheiro : PaymentMethod.Cartao;

                        string productId = selectedProduct.Id.ToString();
                        string customerId = selectedCustomer.Id.ToString();

                        _saleService.CreateSale(productId, customerId, paymentMethod);

                        Console.WriteLine("Venda realizada com sucesso!");
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
