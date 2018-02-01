using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class BookingTableDTO
    {
        public int BookingID { get; set; }
        public int TruflUserID { get; set; }
        public int RestaurantID { get; set; }
        public int PartySize { get; set; }
        public int OfferType { get; set; }
        public int OfferAmount { get; set; }
        public int Quoted { get; set; }
        public int BookingStatus { get; set; }
        public int LoggedInUser { get; set; }
        public int GetTableNowType { get; set; }
    }
}
