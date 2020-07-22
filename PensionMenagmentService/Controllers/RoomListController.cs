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
    public class RoomListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RoomListController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            var roomsToDisplay = _context.Rooms
                                 .Include(a => a.Guest)
                                 .ToList();
                               
            var roomVM = new PensionViewModel()
            {
                RoomList = roomsToDisplay
            };

            return View(roomVM);
        }
    }
}
