using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RestaurantManageServer
    {
        public int RestaurantID { get; set; }
        public int HostessID { get; set; }
        public int StartTableNumber { get; set; }
        public int EndTableNumber { get; set; }
    }
}
