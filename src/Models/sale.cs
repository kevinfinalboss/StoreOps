using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StoreOps.Models
{
    public enum PaymentMethod { Dinheiro, Cartao }

    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [BsonElement("ProductId")]
        public required string ProductId { get; set; }

        [Required]
        [BsonElement("CustomerId")]
        public required string CustomerId { get; set; }

        [Required]
        [BsonElement("ProductName")]
        public required string ProductName { get; set; }

        [Required]
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
