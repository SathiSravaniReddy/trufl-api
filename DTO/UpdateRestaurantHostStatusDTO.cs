using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UpdateRestaurantHostStatusDTO
    {
        public string TruflUserType { get; set; }
        public int RestaurantID { get; set; }
        public int UserID { get; set; }
        public bool ActiveStatus { get; set; }
    }
}
