using StoreOps.Models;
using StoreOps.Services;

namespace StoreOps.UI
{
    class CustomerMenu
    {
        private readonly CustomerService _customerService;

        public CustomerMenu(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Menu de Clientes:");
                Console.WriteLine("1 - Adicionar Cliente");
                Console.WriteLine("2 - Ver Clientes");
                Console.WriteLine("3 - Deletar Clientes");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        ViewCustomers();
                        break;
                    case "3":
                        DeleteCustomer();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }

        private void AddCustomer()
        {
            try
            {
                Console.WriteLine("Informe o nome do cliente:");
                string name = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Informe a idade do cliente:");
                int age = int.Parse(Console.ReadLine() ?? string.Empty);

                Console.WriteLine("Informe o CPF do cliente:");
                string cpf = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Informe o e-mail do cliente:");
                string email = Console.ReadLine() ?? string.Empty;

                Console.WriteLine("Informe o número de telefone do cliente:");
                string phoneNumber = Console.ReadLine() ?? string.Empty;

                Customer customer = new()
                {
                    Name = name,
                    Age = age,
                    CPF = cpf,
                    Email = email,
                    PhoneNumber = phoneNumber
                };

                _customerService.AddCustomer(customer);

                Console.WriteLine("Cliente adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar cliente: {ex.Message}");
            }
        }

        private void ViewCustomers()
        {
            
        }

        private void DeleteCustomer()
        {
            
        }
    }
}
