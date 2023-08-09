using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreOps.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public required string Name { get; set; }

        [BsonElement("Description")]
        public required string Description { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("CategoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string CategoryId { get; set; }

        [BsonElement("Stock")]
        public int Stock { get; set; }

        [BsonIgnore]
        public required Category Category { get; set; }
    }
}
