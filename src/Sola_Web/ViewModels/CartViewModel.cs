using ApplicationCore.Models;

namespace Sola_Web.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total { get; set; }
    }
}
