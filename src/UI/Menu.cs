using System;
using Figgle;

namespace StoreOps
{
    class Menu
    {
        public void ShowMenu()
        {
            string asciiArt = FiggleFonts.Standard.Render("StoreOps");

            Console.WriteLine(asciiArt);
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Menu Principal:");
                Console.WriteLine("1 - Produtos");
                Console.WriteLine("0 - Sair" );
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowProductMenu();
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
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        
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
    }
}
