using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartId { get; set; }  // Session ID or User ID
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        private DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => _dateCreated = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        // Navigation properties
        public Product Product { get; set; }
    }
}
