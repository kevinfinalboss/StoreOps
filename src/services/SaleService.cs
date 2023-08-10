using StoreOps.Models;
using StoreOps.Database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace StoreOps.Services
{
    public class SaleService
    {
        private readonly IMongoCollection<Sale> _sales;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly EmailService _emailService;

        public SaleService(DatabaseConnection databaseConnection, ProductService productService, CustomerService customerService, EmailService emailService)
        {
            _sales = databaseConnection.Database.GetCollection<Sale>("sales");
            _productService = productService;
            _customerService = customerService;
            _emailService = emailService;
        }

        public void CreateSale(string productId, string customerId, PaymentMethod paymentMethod)
        {
            var product = _productService.GetProductById(productId);
            if (product.Stock <= 0)
            {
                Console.WriteLine("Produto sem estoque!");
                return;
            }

            var customer = _customerService.GetCustomerById(customerId);
            if (customer == null)
            {
                Console.WriteLine("Cliente não encontrado!");
                return;
            }

            var sale = new Sale
            {
                ProductId = productId,
                CustomerId = customerId,
                ProductName = product.Name,
                CustomerName = customer.Name,
                PaymentMethod = paymentMethod,
                SaleDate = DateTime.Now,
                Product = product,
                Customer = customer
            };

            _sales.InsertOne(sale);

            product.Stock--;
            _productService.UpdateProduct(product);

            string saleConfirmationHtmlTemplate = File.ReadAllText("./templates/vendasnotifier.html");
            saleConfirmationHtmlTemplate = saleConfirmationHtmlTemplate.Replace("{{NAME}}", customer.Name).Replace("{{PRODUCT_NAME}}", product.Name);
            _emailService.SendEmailAsync(customer.Email, "Confirmação de Compra", saleConfirmationHtmlTemplate, CancellationToken.None).GetAwaiter().GetResult();

            Console.WriteLine("Venda realizada com sucesso!");
        }

        public List<Sale> GetSales()
        {
            return _sales.Find(sale => true).ToList();
        }
    }
}
