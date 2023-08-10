using StoreOps.Models;
using StoreOps.Services;
using Figgle;

namespace StoreOps.UI
{
    class ConfigurationMenu
    {
        private readonly CategoryService _categoryService;

        public ConfigurationMenu(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine(FiggleFonts.Standard.Render("Configuration"));
                Console.WriteLine("========================================");
                Console.WriteLine("         Menu de Configurações          ");
                Console.WriteLine("========================================");
                Console.WriteLine("1 - Adicionar Categoria");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                Console.WriteLine("----------------------------------------");
                Console.Write("Escolha a opção: ");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddCategory();
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

                Console.WriteLine("Informe a descrição da categoria:");
                string description = Console.ReadLine() ?? string.Empty;

                Category category = new() { Name = name, Description = description };

                _categoryService.AddCategory(category);

                Console.WriteLine("Categoria adicionada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar categoria: {ex.Message}");
            }
        }
    }
}
