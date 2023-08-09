using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreOps.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public required string Name { get; set; }

        [BsonElement("Description")]
        public required string Description { get; set; }
    }
}
