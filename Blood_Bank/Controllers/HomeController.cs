using Blood_Bank.Data;
using Blood_Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Blood_Bank.Controllers
{
    public class HomeController : Controller
    {
        ProjectDbContext _context;
        public HomeController(ProjectDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult faq()
        {
            return View();
        }

        public IActionResult generalInfomation()
        {
            return View();
        }

        public IActionResult TermOfService()
        {
            return View();
        }

        public async Task<IActionResult> Donor()
        {
            var id = HttpContext.Session.GetInt32("id");
            var data = await _context.UserRoles!.SingleOrDefaultAsync(m => m.UserId == id);
            if(data != null)
            {
                HttpContext.Session.SetInt32("role", data.RoleId);
                ViewBag.Role = HttpContext.Session.GetInt32("role");
                ViewBag.Id = id;
            }
            if(data == null)
            {
                ViewBag.Role = 0;
            }
            return View();
        }

        public async Task<IActionResult> Seeker()
        {
            
            return View();
        }

    }
}