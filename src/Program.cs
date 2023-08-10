using StoreOps.Database;
using StoreOps.Services;
using StoreOps.UI;

namespace StoreOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao StoreOps");
            var databaseConnection = new DatabaseConnection();
            var categoryService = new CategoryService(databaseConnection);
            var productService = new ProductService(databaseConnection);
            var customerService = new CustomerService(databaseConnection);
            var emailService = new EmailService(); 
            var saleService = new SaleService(databaseConnection, productService, customerService, emailService); // Corrigido aqui

            var menu = new Menu(categoryService, productService, customerService, saleService, emailService);
            menu.ShowMenu();
        }
    }
}
