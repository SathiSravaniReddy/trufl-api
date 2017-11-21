using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class RestaurantSeatedUsersDTO
    {
       public int RestaurantID { get; set; }
       public int TruflUserID { get; set; }
       public string AmenitiName { get; set; }
        public bool AmenitiChecked { get; set; }
        
    }
}
