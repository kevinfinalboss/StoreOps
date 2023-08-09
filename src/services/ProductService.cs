using MongoDB.Driver;
using StoreOps.Database;
using StoreOps.Models;
using System.Collections.Generic;

namespace StoreOps.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(DatabaseConnection databaseConnection)
        {
            _products = databaseConnection.Database.GetCollection<Product>("products");
        }

        public Product AddProduct(Product product)
        {
            _products.InsertOne(product);
            return product;
        }

        public List<Product> GetProducts()
        {
            return _products.Find(product => true).ToList();
        }
    }
}
