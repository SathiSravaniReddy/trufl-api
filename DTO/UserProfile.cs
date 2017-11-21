using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class UserProfile
    {
        public string RestaurantID { get; set; }
        public string UserName { get; set; }
        public string PartySize { get; set; }
        public string Quoted { get; set; }
        public string Waited { get; set; }
        public string OfferAmount { get; set; }
        public string Email { get; set; }
        public string pic { get; set; }
        public string Contact1 { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string ActiveInd { get; set; }
        public int ResauranEmpInd { get; set; }
        public int TruffMemberType { get; set; }
        public int TruflRelationship { get; set; }
        public string TruflshareCode { get; set; }
        public int ReferTruflUserID { get; set; }
        public string ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }

    }
}
