using Spectre.Console;
using StoreOps.Models;
using StoreOps.Services;

namespace StoreOps.UI
{
    class ConfigurationMenu
    {
        private readonly CategoryService _categoryService;

        public ConfigurationMenu(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Obsolete]
        public void ShowMenu()
        {
            while (true)
            {
                AnsiConsole.Render(new FigletText("Configuration"));
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                AnsiConsole.MarkupLine("[bold blue]         Menu de Configurações          [/]");
                AnsiConsole.MarkupLine("[bold blue]========================================[/]");
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Escolha uma opção:")
                        .AddChoices(new[] { "Adicionar Categoria", "Voltar ao Menu Principal" }));

                switch (option)
                {
                    case "Adicionar Categoria":
                        AddCategory();
                        break;
                    case "Voltar ao Menu Principal":
                        return;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Opção inválida![/]");
                        break;
                }
            }
        }

        private void AddCategory()
        {
            try
            {
                string name = AnsiConsole.Ask<string>("Informe o nome da categoria:");
                string description = AnsiConsole.Ask<string>("Informe a descrição da categoria:");

                Category category = new() { Name = name, Description = description };

                _categoryService.AddCategory(category);

                AnsiConsole.MarkupLine("[bold green]Categoria adicionada com sucesso![/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]Erro ao adicionar categoria: {ex.Message}[/]");
            }
        }
    }
}
