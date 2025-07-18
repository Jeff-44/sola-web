using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using ApplicationCore.Settings;
namespace Sola_Web.Controllers
{
    public class ContactController : Controller
    {
        //private readonly IEmailService _emailService;
        private readonly IEmailSender _emailSender;
        public ContactController(IEmailSender emailService)
        {
            _emailSender = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(model.Email, model.Subject, model.Message);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
    
}
