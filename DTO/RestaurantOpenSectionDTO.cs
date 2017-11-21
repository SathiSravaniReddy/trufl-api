using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RestaurantOpenSectionDTO
    {
        public string time;
        public RestaurantSectionStaffDTO restaurantSectionStaffDTO { get; set; }
        public RestaurantActiveSectionsDTO restaurantActiveSectionsDTO { get; set; }
    }
}
