using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public class RestPasswordDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string LoginPassword { get; set; }
        public string NewLoginPassword { get; set; }
    }
}
