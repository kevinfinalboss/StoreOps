using System;
using StoreOps.Database;

namespace StoreOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao StoreOps");
            _ = new DatabaseConnection();

            Menu menu = new();

            menu.ShowMenu();
        }
    }
}
