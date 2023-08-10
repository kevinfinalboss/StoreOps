using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StoreOps.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        [BsonElement("Name")]
        public required string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "A descrição do produto deve ter no máximo 500 caracteres.")]
        [BsonElement("Description")]
        public required string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O preço do produto deve ser positivo.")]
        [BsonElement("Price")]
        public decimal Price { get; set; }

        [Required]
        [BsonElement("CategoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string CategoryId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque do produto deve ser positivo.")]
        [BsonElement("Stock")]
        public int Stock { get; set; }

        [BsonIgnore]
        public required Category Category { get; set; }
    }
}
