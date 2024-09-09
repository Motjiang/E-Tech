using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_Tech.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = "";
        [Required]
        [Precision(16, 2)]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string ImageFile { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
