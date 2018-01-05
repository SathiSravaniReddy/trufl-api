using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SaveRestaurantGuestDTO
    {
        public int RestaurantID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string UserType { get; set; }
        public int PartySize { get; set; }
        public int QuotedTime { get; set; }
        public string Relationship { get; set; }
        public string ThisVisit { get; set; }
        public string FoodAndDrink { get; set; }
        public string SeatingPreferences { get; set; }
        public string Description { get; set; }
        public DateTime WaitListTime { get; set; }
        public int BookingStatus { get; set; }
        public string TableNumbers { get; set; }
        public string SeatedTableType { get; set; }
    }
}
