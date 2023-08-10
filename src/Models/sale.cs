using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace StoreOps.Models
{
    public enum PaymentMethod
    {
        Dinheiro,
        Cartao
    }

    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("CustomerId")]
        public string CustomerId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("CustomerName")]
        public string CustomerName { get; set; } 

        [BsonElement("PaymentMethod")]
        [BsonRepresentation(BsonType.String)]
        public PaymentMethod PaymentMethod { get; set; }

        [BsonElement("SaleDate")]
        public DateTime SaleDate { get; set; }

        [BsonIgnore]
        public Product Product { get; set; }

        [BsonIgnore]
        public Customer Customer { get; set; }
    }
}
