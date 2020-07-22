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
    public class ReservationHistoryListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationHistoryListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var reservationHistoryToDisplay = _context.ReservationHistoryItems
                                             .Include(c => c.Guest)
                                             .Include(c => c.Room)
                                             .ToList();
            var reservationHistoryVM = new PensionViewModel
            {
                ReservationHistoryList = reservationHistoryToDisplay
            };
            return View(reservationHistoryVM);
        }
    }
}
