using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace StoreOps.Models
{
    public class Customer
    {
        public ObjectId Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome do cliente deve ter no máximo 100 caracteres.")]
        public required string Name { get; set; }

        [Required]
        [Range(18, 150, ErrorMessage = "A idade do cliente deve estar entre 18 e 150 anos.")]
        public int Age { get; set; }

        [Required]
        [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "CPF inválido.")]
        public required string CPF { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public required string PhoneNumber { get; set; }
    }
}
