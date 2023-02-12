using Blood_Bank.Data;
using Blood_Bank.HashPass;
using Blood_Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Blood_Bank.Controllers
{
    public class LoginCheckController : Controller
    {
        ProjectDbContext _context;
        public LoginCheckController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") != null)
            {
                return RedirectToAction("Donor", "home");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username , string password)
        {

            if (ModelState.IsValid)
            {
                var user = await _context.User!.SingleOrDefaultAsync(m => m.Username == username && m.Password == MD5.md5(password));
                var accounts = await _context.User!.Where(m => m.Username == username).ToListAsync();

                if (accounts.Count() == 0)
                {
                    TempData["message"] = "Account no exist!";
                    return RedirectToAction("Index");
                }
                if (accounts.Count() != 0)
                {
                    if (accounts.First().Password == MD5.md5(password))
                    {
                        var session = HttpContext.Session;
                        session.SetInt32("id", user.Id);
                        session.SetString("name", user.Name);
                        session.SetString("city", user.City);
                        session.SetString("email", user.Email);
                        session.SetInt32("weight", (int)user.Weight);
                        session.SetString("address", user.Address);
                        session.SetString("phone", user.ContactNumber);
                        session.SetInt32("bloodgroup", (int)user.BloodGroup);
                        session.SetInt32("gender", (int)user.Gender);
                        session.SetString("name", user.Password);

                        ViewBag.Name = session.GetString("name");

                        if (session.Keys.Contains("username"))
                        {
                            return RedirectToAction("Login");
                        }
                        TempData["message2"] = "Login successful!";
                        return RedirectToAction("Donor", "home");
                    }
                }

                //if (user != null)
                //{
                //    HttpContext.Session.SetString("name", user.Name);
                //    HttpContext.Session.SetString("city", user.City);
                //    HttpContext.Session.SetString("email", user.Email);
                //    HttpContext.Session.SetInt32("weight", (int)user.Weight);
                //    HttpContext.Session.SetString("address", user.Address);
                //    HttpContext.Session.SetString("phone", user.ContactNumber);
                //    HttpContext.Session.SetInt32("bloodgroup", (int)user.BloodGroup);
                //    HttpContext.Session.SetInt32("gender", (int)user.Gender);
                //    HttpContext.Session.SetString("name", user.Password);
                //    return RedirectToAction("Index", "Home");
                //}
               
            }
            TempData["message"] = "Password Fail!";
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Donor", "home");
        }

    }
}
