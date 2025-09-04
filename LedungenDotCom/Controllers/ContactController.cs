using LedungenDotCom.Models;
using LedungenDotCom.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LedungenDotCom.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ContactForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid form data");

            string subject = "New Contact Form Submission";
            string body = $"Name: {form.Name}\nEmail: {form.Email}\nMessage:\n{form.Message}";

            await _emailService.SendEmailAsync("yourname@gmail.com", subject, body);

            // Return a simple success page
            return Content("Email sent successfully!");
        }
    }
}