using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Models
{
    public enum ResStatus
    {
        active, canceled, deleted
    }
    public class Reservation
    {
        public int ReservationID { get; set; }

        [Display(Name = "Checkin Date")]
        [DataType(DataType.Date)]

        public DateTimeOffset Check_in { get; set; }

        [Display(Name = "Checkout Date")]
        [DataType(DataType.Date)]

        public DateTimeOffset Check_out { get; set; }
        public ResStatus Status { get; set; }
        
        //public int GuestID { get; set; }
        public Guest Guest { get; set; }

        
        //public int RoomID { get; set; }
        public Room Room { get; set; }

    }
}
