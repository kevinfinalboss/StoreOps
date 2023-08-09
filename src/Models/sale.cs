using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace StoreOps.Models
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonElement("CustomerId")]
        public string CustomerId { get; set; }

        [BsonElement("PaymentMethod")]
        public string PaymentMethod { get; set; }

        [BsonElement("SaleDate")]
        public DateTime SaleDate { get; set; }

        [BsonIgnore]
        public Product Product { get; set; }

        [BsonIgnore]
        public Customer Customer { get; set; }
    }
}
