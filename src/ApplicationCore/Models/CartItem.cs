using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public string CartId { get; set; }  // Session ID or User ID
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        
        [Required]
        public DateTime DateCreated { get; set; }

        [NotMapped]
        public decimal TotalPrice => Quantity * UnitPrice;

        // Navigation properties
        public virtual Product Product { get; set; }
    }
}
