using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
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
        //old index method to remove
        /* public IActionResult Index()
         {
             var guestToDislplay = from n in _context.Guests
                                   select n;
             var guestVM = new PensionViewModel()
             {
                 GuestList = guestToDislplay.ToList()

             };
             return View(guestVM);

       }*/
        // now it's get ready for sorting and filtering
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var guestToDislplay = from n in _context.Guests
                                  select n;
            int pageSize = 8;
            return View(await PaginatedList<Guest>.CreateAsync(guestToDislplay.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

            [HttpPost]
        public IActionResult AddGuest(string nameuser, string surnameuser, string cityuser, string adressuser, string emailuser, string telephonenumberuser)
        {
            if (nameuser != null && surnameuser != null)
            {
                Guest nextguest = new Guest()
                {
                    Name=nameuser,
                    Surname=surnameuser,
                    Member_since=DateTime.Now,
                    City=cityuser,
                    Adress=adressuser,
                    EmailAdress=emailuser,
                    TelephoneNumber=telephonenumberuser
                };

                _context.Guests.Add(nextguest);
                _context.SaveChanges();

            }
            if (nameuser == null || surnameuser == null)
            {
                ViewBag.ValidationText = "Please enter correct name and surname!";
            }
            //return RedirectToAction(nameof(Index));
            var guestToDislplay = from n in _context.Guests
                                  select n;
            var guestVM = new PensionViewModel()
            {
                GuestList = guestToDislplay.ToList()

            };
            //return View(guestVM);
            return RedirectToAction(nameof(Index));

        }

        public IActionResult DeleteGuest(int id)
        {
            var usertodelete = (from Guest item in _context.Guests
                                where item.GuestID == id
                                select item).FirstOrDefault();
           

            var reservationHistoryItemsToDelete = _context.ReservationHistoryItems
                                                  .Where(n => n.Guest == usertodelete)
                                                  .ToList();
            foreach(var item in reservationHistoryItemsToDelete)
            {
                _context.ReservationHistoryItems.Remove(item);
            }

            var reservationsIncludeUserToDelete = _context.Reserevations
                                                 .Where(n => n.Guest == usertodelete)
                                                 .Include(c => c.Guest)
                                                 .ToList();
            if (reservationsIncludeUserToDelete.Count == 0) 
            { 
            _context.Guests.Remove(usertodelete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.CanNotDelInform = "You can't delete client, because she/he has active reservation";
                return View();
            }
            
        }

        public IActionResult GetDetails (int id)
        {
            var guestDetails = _context.Guests
                             .Where(c => c.GuestID == id)
                             .FirstOrDefault();
            return View(guestDetails);
        }

        public IActionResult Edit (int id) 
        {
            var guestDetails = _context.Guests
                            .Where(c => c.GuestID == id)
                            .FirstOrDefault();
            return View(guestDetails);
        }
        [HttpPost]
        public IActionResult EditAdd(int id, string Name, string Surname, string City, string Adress, string EmailAdress, string TelephoneNumber)
        {
            var guestToEdit = _context.Guests
                           .Where(c => c.GuestID == id)
                           .FirstOrDefault();
            guestToEdit.Name = Name;
            guestToEdit.Surname = Surname;
            guestToEdit.City = City;
            guestToEdit.Adress = Adress;
            guestToEdit.EmailAdress = EmailAdress;
            guestToEdit.TelephoneNumber = TelephoneNumber;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetUserReservationHistory (int id)
        {
            var reservationHistoryOfGuest = _context.ReservationHistoryItems
                                            .Include(c => c.Guest)
                                            .Include(c => c.Room)
                                            .Where(n => n.Guest.GuestID == id)
                                            .ToList();
            var ReservationtoDisplay = new PensionViewModel()
            {
                ReservationHistoryList = reservationHistoryOfGuest
            };
            return View(ReservationtoDisplay);
        }
    }
}
