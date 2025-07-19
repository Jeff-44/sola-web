using ApplicationCore.Interfaces.IServices;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Sola_Web.ViewModels;

namespace Sola_Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IEmailAttachmentSender _emailSender;
        private readonly IPdfService _pdfService;

        public CartController(
            ICartService cartService,
            IProductService productService,
            IEmailAttachmentSender emailSender,
            IPdfService pdfService)
        {
            _cartService = cartService;
            _productService = productService;
            _emailSender = emailSender;
            _pdfService = pdfService;
        }

        public async Task<IActionResult> Index()
        {
            var cartId = GetOrCreateCartId();
            var items = await _cartService.GetCartItemsAsync(cartId);
            var total = await _cartService.GetCartTotalAsync(cartId);

            var viewModel = new CartViewModel
            {
                Items = items,
                Total = total
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var cartId = GetOrCreateCartId();
            try
            {
                await _cartService.AddToCartAsync(cartId, productId, quantity);
                TempData["SuccessMessage"] = "Product added to cart!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var cartId = GetOrCreateCartId();
            try
            {
                if (quantity == 0)
                {
                    await _cartService.RemoveFromCartAsync(cartId, productId);
                    TempData["SuccessMessage"] = "Product removed from cart!";
                }
                else
                {
                    await _cartService.UpdateCartItemAsync(cartId, productId, quantity);
                    TempData["SuccessMessage"] = "Cart updated successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cartId = GetOrCreateCartId();
            await _cartService.RemoveFromCartAsync(cartId, productId);
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var cartId = GetOrCreateCartId();
            try
            {
                await _cartService.ClearCartAsync(cartId);
                TempData["SuccessMessage"] = "Cart cleared successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Please provide a valid email address.";
                return RedirectToAction(nameof(Index));
            }

            var cartId = GetOrCreateCartId();
            var items = await _cartService.GetCartItemsAsync(cartId);
            
            if (!items.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Generate order summary
                var orderSummary = await this.RenderViewToStringAsync("_OrderSummary", items);
                if (string.IsNullOrEmpty(orderSummary))
                {
                    throw new InvalidOperationException("Failed to generate order summary.");
                }

                // Convert to PDF
                var pdfBytes = _pdfService.ConvertHtmlToPdf(orderSummary);

                // Send email with PDF attachment
                await _emailSender.SendEmailWithAttachmentAsync(
                    email,
                    "Your Order Summary",
                    "Thank you for your order. Please find the order summary attached.",
                    pdfBytes,
                    "OrderSummary.pdf"
                );

                // Clear the cart after successful checkout
                await _cartService.ClearCartAsync(cartId);

                TempData["SuccessMessage"] = "Order completed successfully! Check your email for the order summary.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error here
                TempData["ErrorMessage"] = "An error occurred while processing your order. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        private string GetOrCreateCartId()
        {
            if (!HttpContext.Session.TryGetValue("CartId", out byte[] cartIdBytes))
            {
                var cartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartId", cartId);
                return cartId;
            }

            return System.Text.Encoding.UTF8.GetString(cartIdBytes);
        }
    }

    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewName, TModel model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                if (viewResult.Success == false)
                    throw new ArgumentNullException($"{viewName} does not match any available view");

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
