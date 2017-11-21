using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DashBoardDetailsDTO
    {
        public DataTable  OffersRaised { get; set; }
        public DataTable OffersAccepted { get; set; }
        public DataTable OffersRemoved { get; set; }
        public DataTable VisitedCustomers { get; set; }
        public DataTable TotalNumberOfCustomers { get; set; }
        public DataTable NumberOfTruflRestaurants { get; set; }
        public DataTable RestaurantDetails { get; set; }
    }
}
