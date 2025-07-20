using ApplicationCore.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sola_Web.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartSummaryViewComponent(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartId = GetOrCreateCartId();
            var count = await _cartService.GetCartItemCountAsync(cartId);
            return View(count);
        }

        private string GetOrCreateCartId()
        {
            var httpContext = _httpContextAccessor.HttpContext!;
            if (!httpContext.Session.TryGetValue("CartId", out var cartIdBytes))
            {
                var cartId = Guid.NewGuid().ToString();
                httpContext.Session.SetString("CartId", cartId);
                return cartId;
            }
            return System.Text.Encoding.UTF8.GetString(cartIdBytes);
        }
    }
}
