using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
