using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sola_Web.ViewModels
{
    public class InvoiceItemViewModel
    {
        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0", "9999999")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }

    public class InvoiceViewModel
    {
        public List<InvoiceItemViewModel> Items { get; set; } = new();

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public decimal Total => Items.Sum(i => i.TotalPrice);
    }
}
