using MongoDB.Driver;
using StoreOps.Database;
using StoreOps.Models;

namespace StoreOps.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(DatabaseConnection databaseConnection)
        {
            _categories = databaseConnection.Database.GetCollection<Category>("categories");
        }

        public Category AddCategory(Category category)
        {
            _categories.InsertOne(category);
            return category;
        }
    }
}
