using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;

using Trufl.Business_Layer;
using System.Data;
using System.Web.Http.Results;
using System.Text;
//using System.Web.Mvc;

using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace Trufl.Services.Controllers
{
    [RoutePrefix("api/Hostess")]
    public class HostessSettingsController : ApiController
    {
        AdminBL _adminBL = new AdminBL();
        HostessBL _hostessBL = new HostessBL();
        JsonResponseResult JsonResponseResult = new JsonResponseResult();

        #region Settings
        [Route("SaveUserBioEvents")]
        [HttpPost]
        public object SaveUserBioEvents(SaveUserBioEventsDTO saveUserBioEvents)
        {
            try
            {
            bool res = _hostessBL.SaveUserBioEvents(saveUserBioEvents);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveTruflUserCardData")]
        [HttpPost]
        public object SaveTruflUserCardData(SaveUserCardDataDTO saveUserCardData)
        {
            try
            {
                bool res = _hostessBL.SaveTruflUserCardData(saveUserCardData);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetEmployeConfiguration/{TruflUserType}/{RestaurantID}")]
        [HttpGet]
        public object GetEmployeConfigration(string TruflUserType, int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetEmployeConfiguration(TruflUserType, RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetTruflUserCardData/{TruflUserID}")]
        [HttpGet]
        public object GetTruflUserCardData(int TruflUserID)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.GetTruflUserCardData(TruflUserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetCustomerRewards")]
        [HttpPost]
        public object GetCustomerRewards(CustomerRewards customerRewards)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetCustomerRewards(customerRewards);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }
        #endregion
    }
}
