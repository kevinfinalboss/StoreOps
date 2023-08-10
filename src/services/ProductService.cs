using MongoDB.Driver;
using MongoDB.Bson;
using StoreOps.Database;
using StoreOps.Models;
using System.Collections.Generic;
using System.Linq;

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

        public List<Product> SearchProducts(string search, ObjectId? categoryId = null)
        {
            return _products
                .AsQueryable()
                .Where(
                    product =>
                        (
                            string.IsNullOrEmpty(search)
                            || product.Name.Contains(search)
                            || product.Description.Contains(search)
                        )
                        && (
                            !categoryId.HasValue
                            || product.CategoryId == categoryId.Value.ToString()
                        )
                )
                .ToList();
        }

        public Product GetProductById(string productId)
        {
            return _products.Find(product => product.Id == productId).FirstOrDefault();
        }
        public void UpdateProduct(Product product)
        {
            _products.ReplaceOne(p => p.Id == product.Id, product);
        }
    }
}
