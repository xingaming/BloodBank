using Blood_Bank.Data;
using Blood_Bank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blood_Bank.Controllers
{
    public class SeekersController : Controller
    {
        public ProjectDbContext _context;
        
        public SeekersController(ProjectDbContext context) {
            _context = context;
        }

        public ActionResult LifeSaving()
        {
            //LinQ
            var listLifeSaving = from ls in _context.User
                                 join lsRole in _context.UserRoles! on ls.Id equals lsRole.UserId
                                 where lsRole.RoleId == 3
                                 select new
                                 {
                                     Name = ls.Name,
                                     City = ls.City,
                                     ContactNumber = ls.ContactNumber,
                                     Email = ls.Email,
                                 };
            ViewBag.ListLifeSaving = listLifeSaving;
            return View();
        }

        public ActionResult SearchDonor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchDonor(string city, int bloodGroup)
        {
            var listDonor = from ld in _context.User
                            where ld.City == city
                            where ld.BloodGroup == bloodGroup
                            join ldRole in _context.UserRoles! on ld.Id equals ldRole.UserId
                            where ldRole.RoleId == 1
                            select new
                            {
                                Id = ld.Id,
                                Name = ld.Name,
                                City = ld.City,
                                BloodGroup = ld.BloodGroup,
                            };
            ViewBag.list = listDonor;
            var count = listDonor.Count();
            if(count == 0)
            {
                TempData["message"] = "SORRY DONORS ARE NOT AVAILABE WITH THE FOLLOWING BLOOD GROUP AND AREA";
                ViewBag.mess = TempData["message"];
            }
            return View(TempData["message"]);
        }

        public async Task<IActionResult> DonorDetail(int? id)
        {

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

        public ActionResult Mobilink()
        {
            return View();
        }
    }
}
