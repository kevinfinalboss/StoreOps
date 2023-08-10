using MongoDB.Driver;
using OfficeOpenXml;
using StoreOps.Database;
using MongoDB.Bson;
using StoreOps.Models;
using System.Collections.Generic;
using System.IO;
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

        public void GenerateCustomerReport()
        {
            var customers = GetCustomers();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Clientes");

                worksheet.Cells[1, 1].Value = "Nome";
                worksheet.Cells[1, 2].Value = "Idade";
                worksheet.Cells[1, 3].Value = "CPF";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Telefone";
                worksheet.Cells[1, 1, 1, 5].Style.Font.Bold = true;

                for (int i = 0; i < customers.Count; i++)
                {
                    var customer = customers[i];
                    worksheet.Cells[i + 2, 1].Value = customer.Name;
                    worksheet.Cells[i + 2, 2].Value = customer.Age;
                    worksheet.Cells[i + 2, 3].Value = customer.CPF;
                    worksheet.Cells[i + 2, 4].Value = customer.Email;
                    worksheet.Cells[i + 2, 5].Value = customer.PhoneNumber;
                }

                worksheet.Cells.AutoFitColumns();

                string folderPath = Path.Combine("relatorio", "usuarios");
                Directory.CreateDirectory(folderPath);

                string fileName = $"relatorio-usuario-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string fullPath = Path.Combine(folderPath, fileName);

                File.WriteAllBytes(fullPath, package.GetAsByteArray());
            }
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
