using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Spatial;

namespace DTO
{
   public class SaveRestaurantDTO
    {
      public int RestaurantID { get; set; }
        public string RestaurantName { get; set; }
        public string Description { get; set; }
        public string PrimaryContact { get; set; }
        public string SecondaryContact { get; set; }
        public int HoursofOperation { get; set; }
        public bool Parking { get; set; }
        public string Geo { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string OwnerName { get; set; }
        public string OwnerContact1 { get; set; }
        public string OwnerContact2 { get; set; }
        public string OwnerEmail { get; set; }
        public bool GetSeatedOffer { get; set; }
        public string QuotedTime { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public int SeatingSize { get; set; }
        public int NumberOfTables { get; set; }
        public int MenuPath { get; set; }
        public int LoggedInUser { get; set; }
    }
}
