using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Trufl.Data_Access_Layer;
using DTO;

namespace Trufl.Business_Layer
{
   public class CustomerBL
    {
        CustomerDL _customerDL = new CustomerDL();

        public DataSet GetCustomerRewards(CustomerRewards customerRewards)
        {
            return _customerDL.GetCustomerRewards(customerRewards);
        }
    }
}
