﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PensionMenagmentService.Data;
using PensionMenagmentService.Models;

namespace PensionMenagmentService.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var roomsToDisplay = _context.Rooms
                       .Where(n => n.Is_ocuppied == true)
                       .Include(c => c.Guest)
                       .ToList();
                
               

            var checkoutdata = new PensionViewModel()
            {
                RoomList = roomsToDisplay
            };
            return View(checkoutdata);
        }

        public IActionResult CheckOutGuest(int id)
        {
            var room = _context.Rooms
                      .Where(n => n.Is_ocuppied == true)
                      .Include(c => c.Guest)
                      .ToList();


            var checkoutdata = new PensionViewModel()
            {
                RoomList = room

            };
            ViewBag.FirstNameGuest = room.FirstOrDefault().Guest.Name;
            ViewBag.NameGuest = room.FirstOrDefault().Guest.Surname;

          
           
            var roomtocheckout = _context.Rooms
                                 .Where(n => n.RoomID == id)
                                 .Include(c => c.Guest)
                                 .ToList();
                                
            roomtocheckout.FirstOrDefault().Is_ocuppied = false;
            roomtocheckout.FirstOrDefault().Guest = null;
            _context.SaveChanges();
            //new segment - to tests 
            var reservationToCheckout = _context.Reserevations
                                        .Where(n => n.Room == roomtocheckout.FirstOrDefault())
                                        .FirstOrDefault();
            var numberOfDaysInHotel = reservationToCheckout.Check_out.DayOfYear - reservationToCheckout.Check_in.DayOfYear;
            // end of ne segment
            var chargeForStayInHotel = numberOfDaysInHotel * roomtocheckout.FirstOrDefault().ReguralPrice;

            ViewBag.DaysInHotelTotal = $"Days in hotel: {numberOfDaysInHotel}.";
            ViewBag.TotalAmount = $"Total amount for stay: {chargeForStayInHotel} PLN.";
            //return RedirectToAction(nameof(Index));
            return View(checkoutdata);
        }
    }
}
