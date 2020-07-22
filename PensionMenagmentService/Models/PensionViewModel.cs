using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Models
{
    public class PensionViewModel
    {
        public List<Room> RoomList { get; set; }
        public List<Guest> GuestList { get; set; }
        public List<Reservation> ReservationList { get; set; }
        public List<ReservationHistory> ReservationHistoryList { get; set; }
        public List<Reservation> ReservedForToday { get; set; }
    }
}
