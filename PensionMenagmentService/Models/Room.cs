using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PensionMenagmentService.Models
{
    public enum RoomType
    {
        singleperson, doubleperson
    }
    public class Room
    {
        
        public int RoomID { get; set; }
        public int Number { get; set; }
        public bool Is_ocuppied { get; set; }
        public bool Smoke { get; set; }
        public bool Is_cleaned { get; set; }
        public RoomType nubmerbeds { get; set; }
        public Guest Guest { get; set; }
    }
}
