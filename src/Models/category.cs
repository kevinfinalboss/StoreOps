using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StoreOps.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
        [BsonElement("Name")]
        public required string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "A descrição da categoria deve ter no máximo 500 caracteres.")]
        [BsonElement("Description")]
        public required string Description { get; set; }
    }
}