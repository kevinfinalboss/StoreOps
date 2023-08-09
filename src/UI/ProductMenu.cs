using StoreOps.Models;
using StoreOps.Services;

namespace StoreOps.UI
{
    class ProductMenu
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductMenu(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu de Produtos:");
                Console.WriteLine("1 - Registrar Produto");
                Console.WriteLine("2 - Vender Produto");
                Console.WriteLine("3 - Deletar Produto");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
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
    }
}

