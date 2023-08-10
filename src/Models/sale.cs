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
        public string? Id { get; set; }

        [BsonElement("ProductId")]
        public required string ProductId { get; set; }

        [BsonElement("CustomerId")]
        public required string CustomerId { get; set; }

        [BsonElement("ProductName")]
        public required string ProductName { get; set; }

        [BsonElement("CustomerName")]
        public required string CustomerName { get; set; } 

        [BsonElement("PaymentMethod")]
        [BsonRepresentation(BsonType.String)]
        public PaymentMethod PaymentMethod { get; set; }

        [BsonElement("SaleDate")]
        public DateTime SaleDate { get; set; }

        [BsonIgnore]
        public required Product Product { get; set; }

        [BsonIgnore]
        public required Customer Customer { get; set; }
    }
}
