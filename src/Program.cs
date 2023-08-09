using StoreOps.Database;
using StoreOps.Services;

namespace StoreOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao StoreOps");
            var databaseConnection = new DatabaseConnection();
            var categoryService = new CategoryService(databaseConnection);

            var menu = new Menu(categoryService);

            menu.ShowMenu();
        }
    }
}
