using Blood_Bank.Data;
using Blood_Bank.Models;
using Blood_Bank.HashPass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Blood_Bank.Controllers
{
    public class DonorController: Controller
    {
        ProjectDbContext _context;
        public DonorController(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") != 1)
            {
                return RedirectToAction("Donor", "home");
            }

            User? user = await _context.User!.FirstOrDefaultAsync(x => x.Id == id!);
            ViewBag.Id = id;
            if (user == null)
            {
                return NotFound();
            }
            User dto = new User
            {
                Id = user.Id,
                Name = user.Name,
                Address = user.Address,
                Gender = user.Gender,
                City = user.City,
                Weight = user.Weight,
                BloodGroup = user.BloodGroup,
                Dob = user.Dob,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
            };

            return View(dto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") != 1)
            {
                return RedirectToAction("Donor", "home");
            }

            User? user = await _context.User!.FirstOrDefaultAsync(x => x.Id == id!);
            ViewBag.Id = id;
            if (user == null)
            {
                return NotFound();
            }
            User dto = new User
            {
                Id = user.Id,
                Name = user.Name,
                Address = user.Address,
                Gender = user.Gender,
                City = user.City,
                Weight = user.Weight,
                BloodGroup = user.BloodGroup,
                Dob = user.Dob,
                ContactNumber = user.ContactNumber,
                Email = user.Email,
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, User? user)
        {
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") == 1)
            {
                return RedirectToAction("Donor", "home");
            }

            //Validate

            User? old = await _context.User!.FirstOrDefaultAsync(x => x.Id == id!);

            if (old!.Name.Equals(user.Name) && old!.Address.Equals(user.Address) && old!.Gender == user.Gender && old!.City.Equals(user.City) && old!.Weight == user.Weight && old!.BloodGroup.Equals(user.BloodGroup) && old!.ContactNumber.Equals(user.ContactNumber) && old!.Dob == user.Dob && old!.Email.Equals(user.Email))
            {
                TempData["message"] = "Do not have any change!";
                return RedirectToAction("Edit", new { id = id });
            }

            var email = await _context.User!.SingleOrDefaultAsync(u => u.Email.Equals(user.Email) && u.Id != id);
            if (email != null)
            {
                TempData["message"] = "Email already exists!";
                return RedirectToAction("Edit", new { id = id });
            }
            var tel = await _context.User!.SingleOrDefaultAsync(u => u.ContactNumber.Equals(user.ContactNumber) && u.Id != id);
            if (tel != null)
            {
                TempData["message"] = "Contact number already exists!";
                return RedirectToAction("Edit", new { id = id });
            }
            var username = await _context.User!.SingleOrDefaultAsync(u => u.Username.Equals(user.Username) && u.Id != id);
            if (username != null)
            {
                TempData["message"] = "Username already exists!";
                return RedirectToAction("Edit", new { id = id });
            }

                //Update info
            
                old!.Name = user.Name;
                old!.Address = user.Address;
                old!.Gender = user.Gender;
                old!.City = user.City;
                old!.Weight = user.Weight;
                old!.BloodGroup = user.BloodGroup;
                old!.Dob = user.Dob;
                old!.ContactNumber = user.ContactNumber;
                old!.Email = user.Email;

                _context.User!.Update(old);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = id });
            
        }
        
        public async Task<IActionResult> ChangePassword()
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") != 1)
            {
                return RedirectToAction("Donor", "home");
            }

            ViewBag.Id = HttpContext.Session.GetInt32("id");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string? old, string confirm, string newP)
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") != 1)
            {
                return RedirectToAction("Donor", "home");
            }

            //Lấy id
            var id = HttpContext.Session.GetInt32("id");

            //Validate
            var user = await _context.User!.SingleOrDefaultAsync(m => m.Id == id);

            if(user.Password != MD5.md5(old))
            {
                TempData["message"] = "Old password not true!";
                ViewBag.Id = HttpContext.Session.GetInt32("id");
                return View();
            }

            if (newP == old)
            {
                TempData["message"] = "The new password is the same as the old password!";
                ViewBag.Id = HttpContext.Session.GetInt32("id");
                return View();
            }

            if (newP != confirm)
            {
                TempData["message"] = "The new password is not the same as the confirmation password!";
                ViewBag.Id = HttpContext.Session.GetInt32("id");
                return View();
            }

            //Update password
            User? oldPass = await _context.User!.FirstOrDefaultAsync(x => x.Id == id!);

            oldPass!.Password = MD5.md5(newP);

            _context.User!.Update(oldPass);
            await _context.SaveChangesAsync();

            TempData["message2"] = "Change password successful!";
            ViewBag.Id = HttpContext.Session.GetInt32("id");
            return View();
        }

        public async Task<IActionResult> DeleteAccount(int? id)
        {
            //chặn bằng ss
            if (HttpContext.Session.GetInt32("role") == null || HttpContext.Session.GetInt32("role") == 1)
            {
                return RedirectToAction("Donor", "home");
            }
            User? user = await _context.User!.FirstOrDefaultAsync(x => x.Id == id);
            _context.User!.Remove(user);
            await _context.SaveChangesAsync();
            
            UserRoles? role = await _context.UserRoles!.FirstOrDefaultAsync(x => x.UserId == id);
            _context.UserRoles!.Remove(role);
            await _context.SaveChangesAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("Donor", "Home");
        }

    }
}
