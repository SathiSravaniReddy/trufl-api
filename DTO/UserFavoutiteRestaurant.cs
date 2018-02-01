using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserFavoutiteRestaurant
    {
        public int TruflUserID { get; set; }
        public int RestaurantID { get; set; }
        public bool IsFav { get; set; }
    }
}
