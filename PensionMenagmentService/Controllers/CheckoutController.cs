﻿using System;
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
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public CheckoutController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
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
            //logowanie błędów
            try { 
            roomtocheckout.FirstOrDefault().Is_ocuppied = false;
            roomtocheckout.FirstOrDefault().Guest = null;
            _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Hello, this is the bug!", ex);

                throw;
            }

            //new segment - to tests - to refine
            var reservationToCheckout = _context.Reserevations
                                        .Where(n => n.Room == roomtocheckout.FirstOrDefault())
                                        .FirstOrDefault();
                                        
            var numberOfDaysInHotel = DateTime.Now.DayOfYear - reservationToCheckout.Check_in.DayOfYear;
          
            var chargeForStayInHotel = numberOfDaysInHotel * roomtocheckout.FirstOrDefault().ReguralPrice;
            if (reservationToCheckout.BreakfestIncluded == true)
            {
                chargeForStayInHotel = (numberOfDaysInHotel * roomtocheckout.FirstOrDefault().ReguralPrice) + (numberOfDaysInHotel * 80);
            }
            // end of ne segment
            ViewBag.DaysInHotelTotal = $"Days in hotel: {numberOfDaysInHotel}.";
            ViewBag.TotalAmount = $"Total amount for stay: {chargeForStayInHotel} PLN.";
            //return RedirectToAction(nameof(Index));
            return View(checkoutdata);
        }
    }
}
