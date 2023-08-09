using MongoDB.Driver;
using StoreOps.Database;
using MongoDB.Bson;

using StoreOps.Models;
using System.Collections.Generic;

namespace StoreOps.Services
{
    public class CustomerService
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerService(DatabaseConnection databaseConnection)
        {
            _customers = databaseConnection.Database.GetCollection<Customer>("customers");
        }

        public Customer AddCustomer(Customer customer)
        {
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
    }
}
