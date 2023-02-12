using Blood_Bank.Data;
using Blood_Bank.HashPass;
using Blood_Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace Blood_Bank.Controllers
{
    public class RegistrationController : Controller
    {
        ProjectDbContext _context;
        public RegistrationController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User data, UserRoles role)
        {
            var email = await _context.User!.SingleOrDefaultAsync(u => u.Email.Equals(data.Email));
            if(email != null)
            {
                TempData["message"] = "Email already exists!";
                return RedirectToAction("Index");
            }
            var tel = await _context.User!.SingleOrDefaultAsync(u => u.ContactNumber.Equals(data.ContactNumber));
            if (tel != null)
            {
                TempData["message"] = "Contact number already exists!";
                return RedirectToAction("Index");
            }
            var username = await _context.User!.SingleOrDefaultAsync(u => u.Username.Equals(data.Username));
            if (username != null)
            {
                TempData["message"] = "Username already exists!";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                data.Password = MD5.md5(data.Password);
                _context.User!.Add(data);
                await _context.SaveChangesAsync();

                role.UserId = data.Id;
                role.RoleId = 1;
                _context.UserRoles!.Add(role);
                await _context.SaveChangesAsync();

                TempData["message2"] = "Registration successful!";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
