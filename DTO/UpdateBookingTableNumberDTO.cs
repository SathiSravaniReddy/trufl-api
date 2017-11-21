using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateBookingTableNumberDTO
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public int BStatus { get; set; }
        public string TableNumbers { get; set; }
        
    }
}
