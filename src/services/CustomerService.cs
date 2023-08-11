using MongoDB.Driver;
using OfficeOpenXml;
using StoreOps.Database;
using MongoDB.Bson;
using StoreOps.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using OfficeOpenXml.Drawing.Chart;

namespace StoreOps.Services
{
    public partial class CustomerService
    {
        private readonly IMongoCollection<Customer> _customers;
         private readonly EmailService _emailService;

        public CustomerService(DatabaseConnection databaseConnection, EmailService emailService)
        {
            _customers = databaseConnection.Database.GetCollection<Customer>("customers");
            _emailService = emailService;
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


                public void SendWelcomeEmail(string customerId)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine("Cliente não encontrado!");
                return;
            }

            string welcomeHtmlTemplate = File.ReadAllText("./templates/usercreate.html");
            welcomeHtmlTemplate = welcomeHtmlTemplate.Replace("{{NAME}}", customer.Name);
            _emailService.SendEmailAsync(customer.Email, "Bem-vindo!", welcomeHtmlTemplate, CancellationToken.None).GetAwaiter().GetResult();

            Console.WriteLine("E-mail de boas-vindas enviado com sucesso!");
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
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Clientes");
            worksheet.Cells[1, 1].Value = "Nome";
            worksheet.Cells[1, 2].Value = "Idade";
            worksheet.Cells[1, 3].Value = "CPF";
            worksheet.Cells[1, 4].Value = "Email";
            worksheet.Cells[1, 5].Value = "Telefone";
            worksheet.Cells[1, 1, 1, 5].Style.Font.Bold = true;

            int totalAge = 0;
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                worksheet.Cells[i + 2, 1].Value = customer.Name;
                worksheet.Cells[i + 2, 2].Value = customer.Age;
                worksheet.Cells[i + 2, 3].Value = customer.CPF;
                worksheet.Cells[i + 2, 4].Value = customer.Email;
                worksheet.Cells[i + 2, 5].Value = customer.PhoneNumber;
                totalAge += customer.Age;
            }

            worksheet.Cells.AutoFitColumns();

            var pieChart = worksheet.Drawings.AddChart("MediaIdade", eChartType.Pie) as ExcelPieChart;
            pieChart.Title.Text = "Média de Idade dos Clientes";
            pieChart.Series.Add(
                ExcelRange.GetAddress(2, 2, customers.Count + 1, 2),
                ExcelCellBase.GetAddress(2, 1, customers.Count + 1, 1)
            );
            pieChart.SetPosition(5, 0, 6, 0);
            pieChart.SetSize(400, 400);

            string folderPath = Path.Combine("relatorios", "usuarios");
            Directory.CreateDirectory(folderPath);
            string fileName = $"relatorio de usuarios {DateTime.Now:dd-MM-yyyy}.xlsx";
            string fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllBytes(fullPath, package.GetAsByteArray());
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
