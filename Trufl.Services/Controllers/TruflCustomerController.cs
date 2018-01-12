using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using Trufl.Business_Layer;
using System.Data;
using System.Web.Http.Results;

namespace Trufl.Services.Controllers
{
    [RoutePrefix("api/Admin")]
    public class TruflCustomerController : ApiController
    {
        CustomerBL _customerBL = new CustomerBL();
        JsonResponseResult JsonResponseResult = new JsonResponseResult();


        [Route("GetCustomerRewards")]
        [HttpPost]
        public object GetCustomerRewards(CustomerRewards customerRewards)
        {
            DataSet res = new DataSet();
            try
            {
                res = _customerBL.GetCustomerRewards(customerRewards);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }



    }
}
