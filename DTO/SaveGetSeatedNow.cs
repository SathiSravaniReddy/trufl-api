using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class SaveGetSeatedNow
    {
        public int RestaurantID { get; set; }
        public int TableType { get; set; }
        public int NoOfTables { get; set; }
        public float Amount { get; set; }
        public bool IsEnabled { get; set; }
    }
}
