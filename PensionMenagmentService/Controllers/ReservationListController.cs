using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PensionMenagmentService.Data;
using PensionMenagmentService.Models;

namespace PensionMenagmentService.Controllers
{
    public class ReservationListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public ReservationListController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }
        //old index
        /*public IActionResult Index()
        {
           //var reservationToDisplay = from n in _context.Reserevations
            //                           select n;
            var reservationsToDisplay = _context.Reserevations
                                       .Include(c => c.Room)
                                       .Include(c => c.Guest)
                                       .ToList();
            var reservationVM = new PensionViewModel
            {
                ReservationList = reservationsToDisplay
            };
            return View(reservationVM);
        }*/

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
            var reservationsToDisplay = _context.Reserevations
                                      .Include(c => c.Room)
                                      .Include(c => c.Guest);
            int pageSize = 8;

            return View(await PaginatedList<Reservation>.CreateAsync(reservationsToDisplay.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        //public IActionResult AddReservationC(RoomType type, int idguest, DateTime checkin, DateTime checkout, bool breakfestincluded)
        public async Task<IActionResult> AddReservationC(string sortOrder, string currentFilter, string searchString, int? pageNumber, RoomType type, int idguest, DateTime checkin, DateTime checkout, bool breakfestincluded)
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

            if (ModelState.IsValid)
            {
                var RoomToRentTypeList = (from Room item in _context.Rooms
                                          where item.nubmerbeds == type
                                          select item).ToList();

                var GuestToRent =    (from Guest item in _context.Guests
                                     where item.GuestID == idguest
                                     select item).SingleOrDefault();
                var RoomTypeNumbers = (from Room n in RoomToRentTypeList
                                       select n.RoomID).ToList();
                var ReservationTypes = (from Reservation n in _context.Reserevations
                                        where RoomTypeNumbers.Contains(n.Room.RoomID)
                                        select n);

                // need db for checkin&checkoutvalues
                // filtering method propsal
                DateTime checkinvalue = new DateTime();
                DateTime checkoutvalue = new DateTime();
                checkinvalue = checkin;
                checkoutvalue = checkout;
                int numberOfRoomProposal = (from Reservation m in ReservationTypes
                                            where (checkinvalue < m.Check_in
                                            && checkoutvalue < m.Check_in) ||
                                            (checkinvalue > m.Check_out
                                            && checkoutvalue > m.Check_out)
                                            select m.Room.RoomID).FirstOrDefault();
                // lista numerów zajętych w konkretnej dacie
                var numbersOfRoomOccupied = (from Reservation m in ReservationTypes
                                             where (checkinvalue >= m.Check_in
                                             && checkinvalue <= m.Check_out) ||
                                             (checkoutvalue >= m.Check_in
                                             && checkoutvalue <= m.Check_out)
                                             select m.Room.RoomID).ToList();
                // lista z wyrzuconymi zajętymi numerami
                var RoomsToRent = (from Room n in RoomToRentTypeList
                                   where !numbersOfRoomOccupied.Contains(n.RoomID)
                                   select n).ToList();
                /*var RoomToRent = (from Room n in RoomToRentTypeList
                                  where n.RoomID == numberOfRoomProposal
                                  select n).FirstOrDefault();*/

                var RoomToRent = (from Room n in RoomsToRent
                                  select n).FirstOrDefault();

                try { 
                if (RoomToRent != null)
                {
                    //RoomToRent.Is_ocuppied = true;

                    Reservation NewReservation = new Reservation();
                    NewReservation.Status = 0;
                    NewReservation.Guest = GuestToRent;
                   
                    NewReservation.Room = RoomToRent;
                    NewReservation.Check_in = checkin;
                    NewReservation.Check_out = checkout;
                   
                    NewReservation.BreakfestIncluded = breakfestincluded;
                    // add breakfest fee with "if" conditional
                    if (NewReservation.BreakfestIncluded == true)
                     {
                         NewReservation.TotalAmount = ((checkout.DayOfYear - checkin.DayOfYear) * RoomToRent.ReguralPrice)+((checkout.DayOfYear - checkin.DayOfYear)*80);
                     }
                     else
                     {
                         NewReservation.TotalAmount = (checkout.DayOfYear - checkin.DayOfYear) * RoomToRent.ReguralPrice;
                     }
                    //Old way of Total Amount Count method
                    //NewReservation.TotalAmount = (checkout.DayOfYear - checkin.DayOfYear) * RoomToRent.ReguralPrice;
                    _context.Reserevations.Add(NewReservation);
                    _context.SaveChanges();

                }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Hello, this is the bug!", ex);

                    throw;
                }
                
                if (RoomToRent == null)
                {
                    ViewBag.NoFreeRooms = "No rooms available.";
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ValidationText = "Please enter correct check-in and check-out value!";
            }
            /*var singlereservation = from n in _context.Reserevations
                                    select n;*/
            var reservationsToDisplay = _context.Reserevations
                                       .Include(c => c.Room)
                                       .Include(c => c.Guest);

            int pageSize = 8;

            return View(await PaginatedList<Reservation>.CreateAsync(reservationsToDisplay.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult DeleteReservation(int id)
        {
            var reservationIncludeForegin = _context.Reserevations
                                            .Include(c => c.Room)
                                            .Include(c => c.Guest)
                                            .ToList();
            var reservationtodelete = (from Reservation item in reservationIncludeForegin
                                       where item.ReservationID == id
                                       select item).FirstOrDefault();
           
           var roomreserved = (from Room n in _context.Rooms
                                where n.RoomID == reservationtodelete.Room.RoomID
                                select n).FirstOrDefault();

            if (roomreserved.Is_ocuppied == false)
            { 
            Reservation ReservationToArchive = (from Reservation item in _context.Reserevations
                                                where item.ReservationID == id
                                                select item).FirstOrDefault();
            ReservationHistory NewReservationHistoryItem = new ReservationHistory();
            NewReservationHistoryItem.check_in_History = ReservationToArchive.Check_in.Date;
            NewReservationHistoryItem.check_out_History = ReservationToArchive.Check_out.Date;
            NewReservationHistoryItem.Guest = ReservationToArchive.Guest;
            NewReservationHistoryItem.GuestName_History = ReservationToArchive.Guest.Name;
            NewReservationHistoryItem.Room = ReservationToArchive.Room;
            NewReservationHistoryItem.TotalAmount_History = ReservationToArchive.TotalAmount;

            _context.ReservationHistoryItems.Add(NewReservationHistoryItem);
            _context.Reserevations.Remove(reservationtodelete);
            roomreserved.Is_ocuppied = false;
            _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ReservationDetails(int id)
        {
            var reservationDetails = _context.Reserevations
                                   .Where(n => n.ReservationID == id)
                                   .Include(c=>c.Guest)
                                   .Include(c=>c.Room)
                                   .FirstOrDefault();
          
            return View(reservationDetails);
        }
         
        public IActionResult ChangeBreakfest (int id)
        {
            var reservationToChange = _context.Reserevations
                                      .Where(n=>n.ReservationID ==id)
                                      .Include(c => c.Guest)
                                      .Include(c => c.Room)
                                      .FirstOrDefault();
            if (reservationToChange.BreakfestIncluded == true)
            {
                reservationToChange.BreakfestIncluded = false;               
                _context.SaveChanges();
            }
            else 
            {
                reservationToChange.BreakfestIncluded = true;
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // old FindGuest Method
        /*public IActionResult FindGuestId(string guestname)

        {
            
            var searchedGuests = _context.Guests
                               .Where(n => n.Surname == guestname)
                               .ToList();

            var reservationsToDisplay = _context.Reserevations
                                      .Include(c => c.Room)
                                      .Include(c => c.Guest)
                                      .ToList();
            if (searchedGuests.Count == 0)
            {
                ViewBag.Info = "No guest found.";
            }
            var GuestIdDisplay = new PensionViewModel()
            {
                GuestList = searchedGuests,
                ReservationList=reservationsToDisplay
            };        
            return View(GuestIdDisplay);
        }*/

        public async Task<IActionResult> FindGuestIdB(string sortOrder, string currentFilter, string searchString, int? pageNumber, string guestname)
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

            var searchedGuests = _context.Guests
                               .Where(n => n.Surname == guestname)
                               .ToList();

            var reservationsToDisplay = _context.Reserevations
                                      .Include(c => c.Room)
                                      .Include(c => c.Guest);

            ViewData["searched_guests"] = searchedGuests;

            int pageSize = 8;

            return View(await PaginatedList<Reservation>.CreateAsync(reservationsToDisplay.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

    }
}
