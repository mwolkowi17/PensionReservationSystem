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
    public class BreakfastController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BreakfastController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var ReservationsWithBreakfests = _context.Reserevations
                                            .Where(n => n.Check_in <= DateTime.Now)
                                            .Where(n => n.Check_out >= DateTime.Now)
                                            .Where(n => n.BreakfestIncluded == true)
                                            .Include(c =>c.Room)
                                            .Include(c =>c.Guest)
                                            .Where(n=>n.Room.Is_ocuppied == true)
                                            .ToList();

            var ReservationsWithBreakfestsToDisplay = new PensionViewModel
            {
                ReservationList = ReservationsWithBreakfests
            };
            return View(ReservationsWithBreakfestsToDisplay);
        }
    }
}
