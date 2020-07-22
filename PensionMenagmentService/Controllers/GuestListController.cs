using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using PensionMenagmentService.Data;
using PensionMenagmentService.Models;

namespace PensionMenagmentService.Controllers
{
    public class GuestListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GuestListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var guestToDislplay = from n in _context.Guests
                                  select n;
            var guestVM = new PensionViewModel()
            {
                GuestList = guestToDislplay.ToList()

            };
            return View(guestVM);
        }

        public IActionResult AddGuest(string nameuser, string surnameuser)
        {
            if (nameuser != null && surnameuser != null)
            {
                Guest nextguest = new Guest()
                {
                    Name=nameuser,
                    Surname=surnameuser,
                    Member_since=DateTime.Now
                };

                _context.Guests.Add(nextguest);
                _context.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteGuest(int id)
        {
            var usertodelete = (from Guest item in _context.Guests
                                where item.GuestID == id
                                select item).FirstOrDefault();
            _context.Guests.Remove(usertodelete);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
