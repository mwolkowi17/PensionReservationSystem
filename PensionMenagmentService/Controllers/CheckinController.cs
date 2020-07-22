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
    public class CheckinController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CheckinController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var roomreservedfortoday = _context.Reserevations
                                        .Where(n => n.Check_in.Date == DateTime.Today.Date)
                                        .Include(c => c.Room)
                                        .Include(c => c.Guest)
                                        .ToList();
            

            var roomnotoccupiedtoday = _context.Rooms
                                       .Where(m => m.Is_ocuppied == false)
                                       .Include(c => c.Guest)
                                       .ToList();
                                       
            var checkindata = new PensionViewModel
            {
                ReservationList = roomreservedfortoday,
                RoomList = roomnotoccupiedtoday

            };
            return View(checkindata);
        }

        public IActionResult SelectRoom(int id)
        {
            var roomselected = _context.Rooms
                               .Where(n => n.RoomID == id)
                               .Include(c=>c.Guest)
                               .ToList();
                               
            var checkindata = new PensionViewModel
            {
                RoomList = roomselected
            };
            ViewBag.RoomNr = id;
            return View(checkindata);
        }

        public IActionResult SelectReservation(int id)
        {
            var reservationselected = _context.Reserevations
                                      .Where(n => n.ReservationID == id)
                                      .Include(c => c.Guest)
                                      .Include(c => c.Room)
                                      .ToList();
            var checkindata = new PensionViewModel
            {
                ReservationList = reservationselected
            };

            return View(checkindata);
        }

        public IActionResult CheckinAdd(int id)
        {
            var roomnumbertocheckin = from n in _context.Reserevations
                                      where n.ReservationID == id
                                      select n.Room.RoomID;

            var roomtocheckin = _context.Rooms
                                .Where(m => m.RoomID == roomnumbertocheckin.First())
                                .Include(c => c.Guest)
                                .FirstOrDefault();

            var usertocheckin = from n in _context.Reserevations
                                where n.ReservationID == id
                                select n.Guest;
            roomtocheckin.Is_ocuppied = true;
            roomtocheckin.Guest = usertocheckin.First();
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            ViewBag.CheckinInformation = "Check-in complete.";
            ViewBag.CheckinData = $"Room nr {roomtocheckin.RoomID} has been rented to {usertocheckin.First().Name}.";

            var roomreservedfortoday = _context.Reserevations
                                       .Where(n => n.Check_in.Date == DateTime.Today.Date)
                                       .Include(c => c.Guest)
                                       .Include(c => c.Room)
                                       .ToList();

            var roomnotoccupiedtoday = _context.Rooms
                                       .Where(n => n.Is_ocuppied == false)
                                       .Include(c => c.Guest)
                                       .ToList();
                                      
            var checkindata = new PensionViewModel
            {
                ReservedForToday = roomreservedfortoday,
                RoomList = roomnotoccupiedtoday.ToList()

            };
            return View(checkindata);
        }

        public IActionResult CheckinRoomAdd(int id, string name, string surname)
        {
            var roomtocheckin = _context.Rooms
                                .Where(n => n.RoomID == id)
                                .Include(c => c.Guest)
                                .ToList();
                               
            roomtocheckin.First().Is_ocuppied = true;

            var newguest = new Guest();
            newguest.Name = name;
            newguest.Surname = surname;
            newguest.Member_since = DateTime.Today;

            roomtocheckin.First().Guest = newguest;
            _context.SaveChanges();



            //if(!_context.Users.Contains(newguest))

            var guestcontain = _context.Guests
                               .Where(n => n.Name == name)
                               .Where(m => m.Surname == surname)
                               .ToList();
                             


            ViewBag.CheckinInformation = "Check-in Complete.";
            ViewBag.CheckinData = $"Room nr {id} has been rented to {name} {surname}";
            var roomreservedfortoday = _context.Reserevations
                                       .Where(n => n.Check_in.Date == DateTime.Today.Date)
                                       .Include(c => c.Guest)
                                       .Include(c => c.Room)
                                       .ToList();

            var roomnotoccupiedtoday = _context.Rooms
                                       .Where(n => n.Is_ocuppied == false)
                                       .Include(c => c.Guest)
                                       .ToList();
                                       
            var simpleguest = from n in _context.Guests
                              select n;
            var checkindata = new PensionViewModel
            {
                ReservedForToday = roomreservedfortoday,
                RoomList = roomnotoccupiedtoday,
                GuestList = simpleguest.ToList()

            };
            //do zrobienia bo nie działa dobrze
            //if (!checkindata.GuestList.Contains(newguest))
            if (guestcontain.ToList().Count == 0)
            {
                _context.Guests.Add(newguest);
                _context.SaveChanges();
            }
            return View(checkindata);
        }
    }
}
