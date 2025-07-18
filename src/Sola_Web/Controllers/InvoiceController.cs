using ApplicationCore.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;
using Sola_Web.Services;
using Sola_Web.ViewModels;

namespace Sola_Web.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IViewRenderService _renderer;
        private readonly IPdfService _pdfService;
        private readonly IEmailAttachmentSender _emailSender;

        public InvoiceController(IViewRenderService renderer, IPdfService pdfService, IEmailAttachmentSender emailSender)
        {
            _renderer = renderer;
            _pdfService = pdfService;
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
            var pdfBytes = _pdfService.GeneratePdfFromHtml(html);

            await _emailSender.SendEmailWithAttachmentAsync(model.Email, "Invoice", "Please find attached your invoice.", pdfBytes, "invoice.pdf");

            return File(pdfBytes, "application/pdf", "invoice.pdf");
        }
    }
}
