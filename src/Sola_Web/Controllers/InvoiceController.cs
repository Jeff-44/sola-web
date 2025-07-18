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

        public InvoiceController(IViewRenderService renderer, IEmailAttachmentSender emailSender)
        {
            _renderer = renderer;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new InvoiceViewModel
            {
                Items = new List<InvoiceItemViewModel> { new InvoiceItemViewModel() }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var html = await _renderer.RenderToStringAsync("Invoice/Invoice", model);
            var htmlBytes = System.Text.Encoding.UTF8.GetBytes(html);

            await _emailSender.SendEmailWithAttachmentAsync(model.Email, "Invoice", "Please find attached your invoice.", htmlBytes, "invoice.html");

            return File(htmlBytes, "text/html", "invoice.html");
        }
    }
}
