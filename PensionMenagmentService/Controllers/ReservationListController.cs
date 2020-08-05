using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensionMenagmentService.Data;
using PensionMenagmentService.Models;

namespace PensionMenagmentService.Controllers
{
    public class ReservationListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           /* var reservationToDisplay = from n in _context.Reserevations
                                       select n;*/
            var reservationsToDisplay = _context.Reserevations
                                       .Include(c => c.Room)
                                       .Include(c => c.Guest)
                                       .ToList();
            var reservationVM = new PensionViewModel
            {
                ReservationList = reservationsToDisplay
            };
            return View(reservationVM);
        }
        public IActionResult AddReservationC(RoomType type, int idguest, DateTime checkin, DateTime checkout)
        {
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


                if (RoomToRent != null)
                {
                    //RoomToRent.Is_ocuppied = true;

                    Reservation NewReservation = new Reservation();
                    NewReservation.Status = 0;
                    NewReservation.Guest = GuestToRent;
                   
                    NewReservation.Room = RoomToRent;
                    NewReservation.Check_in = checkin;
                    NewReservation.Check_out = checkout;
                    NewReservation.TotalAmount = (checkout.DayOfYear - checkin.DayOfYear) * RoomToRent.ReguralPrice;

                    _context.Reserevations.Add(NewReservation);
                    _context.SaveChanges();

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
                                       .Include(c => c.Guest)
                                       .ToList();
                                       
            var reservationVM = new PensionViewModel
            {
                ReservationList = reservationsToDisplay
            };
            return View(reservationVM);
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
    }
}
