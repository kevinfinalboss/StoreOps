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
            if (!IsValidEmail(customer.Email))
                throw new FormatException("Email inválido");

            customer.CPF = FormatCPF(customer.CPF);
            customer.PhoneNumber = FormatPhoneNumber(customer.PhoneNumber);

            _customers.InsertOne(customer);
            return customer;
        }

        public List<Customer> GetCustomers()
        {
            return _customers.Find(customer => true).ToList();
        }

        public List<Customer> SearchCustomers(string search)
        {
            return _customers
                .Find(
                    customer =>
                        customer.Name.Contains(search)
                        || customer.Age.ToString().Contains(search)
                        || customer.CPF.Contains(search)
                        || customer.Email.Contains(search)
                        || customer.PhoneNumber.Contains(search)
                )
                .ToList();
        }

        public void DeleteCustomer(ObjectId id)
        {
            _customers.DeleteOne(customer => customer.Id == id);
        }

        public Customer GetCustomerById(string customerId)
        {
            var objectId = new ObjectId(customerId);
            return _customers.Find(customer => customer.Id == objectId).FirstOrDefault();
        }

        private bool IsValidEmail(string email)
        {
            var regexPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            return Regex.IsMatch(email, regexPattern);
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
