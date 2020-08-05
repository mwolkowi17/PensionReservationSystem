using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Models
{
    public class ReservationHistory
    {
        public int ReservationHistoryID { get; set; }
        [Display(Name = "Checkin Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset check_in_History { get; set; }
        [Display(Name = "Checkout Date")]
        [DataType(DataType.Date)]
        public DateTimeOffset check_out_History { get; set; }
        //public int GuestID_History { get; set; }
        public Guest Guest { get; set; }
        public string GuestName_History { get; set; }
        //public int RoomID_History { get; set; }
        public Room Room { get; set; }
        public int TotalAmount_History { get; set; }
    }
}
