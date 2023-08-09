using Figgle;
using StoreOps.Models;
using StoreOps.Services;
using System;

namespace StoreOps
{
    class Menu
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public Menu(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public void ShowMenu()
        {
            string asciiArt = FiggleFonts.Standard.Render("StoreOps");

            Console.WriteLine(asciiArt);
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Menu Principal:");
                Console.WriteLine("1 - Produtos");
                Console.WriteLine("5 - Configuração");
                Console.WriteLine("0 - Sair");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        ShowProductMenu();
                        break;
                    case "5":
                        ShowConfigurationMenu();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void ShowProductMenu()
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

        private void ShowConfigurationMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu de Configuração:");
                Console.WriteLine("1 - Adicionar Categoria");
                Console.WriteLine("2 - Editar Categoria");
                Console.WriteLine("3 - Deletar Categoria");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddCategory();
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
        private void AddCategory()
        {
            try
            {
                Console.WriteLine("Informe o nome da categoria:");
                string name = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                    throw new Exception("Nome da categoria não pode estar vazio.");

                Console.WriteLine("Informe a descrição da categoria:");
                string description = Console.ReadLine() ?? string.Empty;

                Category category = new() { Id = null, Name = name, Description = description };

                _categoryService.AddCategory(category);

                Console.WriteLine("Categoria adicionada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar categoria: {ex.Message}");
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
                int selectedCategoryIndex = int.Parse(selectedCategoryIndexInput) - 1;
                var selectedCategory = categories[selectedCategoryIndex]!;

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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar produto: {ex.Message}");
            }
        }
    }
}
