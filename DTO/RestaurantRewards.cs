using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class RestaurantRewards
    {
        public int TruflUserID { get; set; }
        public int RestaurantID { get; set; }
        public int BillAmount { get; set; }
        //public int RewardPoints { get; set; }
       // public bool IsWinBid { get; set; }
        public string RewardType { get; set; }
    }
}
