using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RestaurantActiveSectionsDTO
    {
        public int RestaurantID { get; set; }
        public int FloorNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
