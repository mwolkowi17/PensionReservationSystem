using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Member_since { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string EmailAdress { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
