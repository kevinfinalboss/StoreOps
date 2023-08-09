using System;

namespace StoreOps
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao StoreOps");

            Menu menu = new Menu();

            menu.ShowMenu();
        }
    }
}
