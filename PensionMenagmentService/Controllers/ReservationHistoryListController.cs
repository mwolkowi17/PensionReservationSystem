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
        /*public IActionResult Index()
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
            /* var reservationHistoryToDisplay = from n in _context.ReservationHistoryItems
                                              select n;*/
            var reservationHistoryToDisplay = _context.ReservationHistoryItems
                                              .Include(c => c.Guest)
                                              .Include(c => c.Room);
                                            
            int pageSize = 8;
            return View(await PaginatedList<ReservationHistory>.CreateAsync(reservationHistoryToDisplay.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
