using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SaveUserBioEventsDTO
    {
        public int TruflUserID { get; set; }
        public string Relationship { get; set; }
        public string ThisVisit { get; set; }
        public string FoodAndDrink { get; set; }
        public string SeatingPreferences { get; set; }
        public string Description { get; set; }

    }
}
