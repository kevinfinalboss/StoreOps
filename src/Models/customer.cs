using MongoDB.Bson;

namespace StoreOps.Models
{
    public class Customer
    {
        public ObjectId Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required string CPF { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
