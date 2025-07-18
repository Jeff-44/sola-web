using ApplicationCore.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Sola_Web.Services;
using Sola_Web.ViewModels;

namespace Sola_Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IViewRenderService _renderer;
        private readonly IEmailAttachmentSender _emailSender;
        private readonly IProductService _productService;
        private readonly IPdfService _pdfService;

        public InvoiceController(IViewRenderService renderer, IEmailAttachmentSender emailSender, IProductService productService, IPdfService pdfService)
        {
            _renderer = renderer;
            _emailSender = emailSender;
            _productService = productService;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new InvoiceViewModel
            {
                Items = new List<InvoiceItemViewModel> { new InvoiceItemViewModel() }
            };
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = await _productService.GetAllProductsAsync();
                return View(model);
            }

            var html = await _renderer.RenderToStringAsync("Invoice/Invoice", model);
            var pdfBytes = _pdfService.ConvertHtmlToPdf(html);

            await _emailSender.SendEmailWithAttachmentAsync(model.Email, "Invoice", "Please find attached your invoice.", pdfBytes, "invoice.pdf", "application/pdf");

            return View("Invoice", model);
        }
    }
}
