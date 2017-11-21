using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RestaurantSettingsDTO
    {
        public int RestaurantID { get; set; }
        public int DiningTime { get; set; }
        public int Geofence { get; set; }
        public int TableNowCapacity { get; set; }
        public int DefaultTableNowPrice { get; set; }
        public int MinimumTableNowPrice { get; set; }
    }
}
