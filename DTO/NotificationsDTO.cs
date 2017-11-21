using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NotificationsDTO
    {
        public int RestaurantID { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
