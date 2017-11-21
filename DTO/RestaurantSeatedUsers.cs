using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class RestaurantSeatedUsers
    {
        public int TruflUserID { get; set; }
        public string TUserName { get; set; }
        public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public int PartySize { get; set; }
        public int OfferAmount { get; set; }
        public bool Status { get; set; }
        public int TableNumber { get; set; }
        public bool AppServed { get; set; }
        public bool MenuServed { get; set; }
        public bool DesertServed { get; set; }
        public bool Boozing { get; set; }
        public bool CheckReceived { get; set; }
        public bool Empty { get; set; }
        public bool Seated { get; set; }
    }
}
