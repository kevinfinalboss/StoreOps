using MongoDB.Driver;
using StoreOps.Database;
using MongoDB.Bson;
using StoreOps.Models;
using System.Text.RegularExpressions;

namespace StoreOps.Services
{
    public partial class CustomerService
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerService(DatabaseConnection databaseConnection)
        {
            _customers = databaseConnection.Database.GetCollection<Customer>("customers");
        }

        public Customer AddCustomer(Customer customer)
        {
            if (!IsValidEmail(customer.Email)) throw new FormatException("Email inválido");

            customer.CPF = FormatCPF(customer.CPF);
            customer.PhoneNumber = FormatPhoneNumber(customer.PhoneNumber);

            _customers.InsertOne(customer);
            return customer;
        }

        public List<Customer> GetCustomers()
        {
            return _customers.Find(customer => true).ToList();
        }

        public void DeleteCustomer(ObjectId id)
        {
            _customers.DeleteOne(customer => customer.Id == id);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var regexPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
                return Regex.IsMatch(email, regexPattern);
            }
            catch
            {
                return false;
            }
        }

        private string FormatCPF(string cpf)
        {
            return MyRegex().Replace(cpf, "$1.$2.$3-$4");
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            return MyRegex1().Replace(phoneNumber, "($1) $2-$3");
        }

        [GeneratedRegex("(\\d{3})(\\d{3})(\\d{3})(\\d{2})")]
        private static partial Regex MyRegex();
        [GeneratedRegex("(\\d{2})(\\d{5})(\\d{4})")]
        private static partial Regex MyRegex1();
    }
}
