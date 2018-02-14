using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TruflUserDTO
    {
        public int TruflUserID { get; set; }
        //public int RestaurantID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public byte[] pic { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        //public DateTime DOB { get; set; }
        //public char ActiveInd { get; set; }
        //public int RestaurantEmpInd { get; set; }
        public string TruflMemberType { get; set; }
        //public int TruflRelationship {get;set;}
        public string ReferedShareCode { get; set; }
        public string DeviceID { get; set; }
        public int ModifiedBy { get; set; }
    }
}
