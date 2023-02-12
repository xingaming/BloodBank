using Blood_Bank.Data;
using Blood_Bank.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood_Bank.Controllers
{
    public class MailController : Controller
    {
        ProjectDbContext _context;
        private readonly IEmailSender _emailSender;
        public MailController(IEmailSender emailSender, ProjectDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> SendMail(string email, string subject, string htmlMessage)
        {
            await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SendMailContact(string email, string subject, string htmlMessage)
        {
            email = "vinhxin1105@gmail.com";
            await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SendMailHtml(string email, string subject)
        {
            //Fetching Email Body Text from EmailTemplate File.  
            string FilePath = "wwwroot/mail/test.html";
            StreamReader str = new StreamReader(FilePath);
            string htmlMessage = str.ReadToEnd();
            str.Close();

            //Repalce [newusername] = signup user name   
            //MailText = MailText.Replace("[newusername]", txtUserName.Text.Trim());

            await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> SendMailHtmls(string mail, string phone, string message)
        {
            List<string> list = (from ld in _context.User
                                 join ldRole in _context.UserRoles! on ld.Id equals ldRole.UserId
                                 where ldRole.RoleId == 1
                                 select ld.Email).Distinct().ToList();
            //var list = IQueryabl.Select(s => new { Email = s.Email }).AsEnumerable().Cast<dynamic>().ToList<dynamic>(); ;
            string[] arr = list.ToArray();
            string email = string.Join(",", arr);
            string subject = "Blood Bank";
            //Fetching Email Body Text from EmailTemplate File.  
            string FilePath = "wwwroot/mail/SendMail.html";
            StreamReader str = new StreamReader(FilePath);
            string htmlMessage = str.ReadToEnd();
            str.Close();
             
            //Repalce [mail] = name email  
            htmlMessage = htmlMessage.Replace("[mail]", mail.ToString().Trim());
            htmlMessage = htmlMessage.Replace("[phone]", phone.ToString().Trim());
            htmlMessage = htmlMessage.Replace("[message]", message.ToString().Trim());

            await _emailSender.SendEmailAsync(email, subject, htmlMessage);
            TempData["message"] = "Send email successful!";

            return RedirectToAction("Mobilink", "Seekers");

        }
    }
}
