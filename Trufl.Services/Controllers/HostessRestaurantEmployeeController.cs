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
    public class HostessRestaurantEmployeeController : ApiController
    {
        AdminBL _adminBL = new AdminBL();
        HostessBL _hostessBL = new HostessBL();
        JsonResponseResult JsonResponseResult = new JsonResponseResult();

        [Route("GetRestaurantUserDetails/{RestaurantID}/{TruflUserID}/{UserType}")]
        [HttpGet]
        public object GetRestaurantUserDetails(int? RestaurantID, int TruflUserID, string UserType)
        {
            SettingsDTO res = new SettingsDTO();
            try
            {
                res = _adminBL.GetRestaurantUserDetails(RestaurantID, TruflUserID, UserType);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }


        [Route("UpdateRestaurantHostStatus")]
        [HttpPost]
        public object UpdateRestaurantHostStatus(UpdateRestaurantHostStatusDTO UpdateRestaurantHost)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantHostStatus(UpdateRestaurantHost);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantTables/{RestaurantID}/{UserID}")]
        [HttpGet]
        public object GetRestaurantTables(int RestaurantID, int UserID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetRestaurantTables(RestaurantID, UserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateRestaurantEmployee")]
        [HttpPost]
        public object spUpdateRestaurantEmployee(EmployeeConfigDTO employeeConfigDTO)
        {
            try
            {
                bool res = _hostessBL.spUpdateRestaurantEmployee(employeeConfigDTO);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("SaveRestaurantGuest")]
        [HttpPost]
        public object SaveRestaurantGuest(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            try
            {
                bool res = _hostessBL.SaveRestaurantGuest(SaveRestaurantGuest);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("UpdateRestaurantGuest")]
        [HttpPost]
        public object UpdateRestaurantGuest(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantGuest(UpdateRestaurantGuest);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("GetRestaurantGuest/{RestaurantID}/{UserID}/{UserType}")]
        [HttpGet]
        public object GetRestaurantGuest(int RestaurantID, int UserId, string UserType)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantGuest(RestaurantID, UserId, UserType);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("SaveRestaurantGuestImmediately")]
        [HttpPost]
        public object SaveRestaurantGuestImmediately(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            try
            {
                bool res = _hostessBL.SaveRestaurantGuestImmediately(SaveRestaurantGuest);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("UpdateRestaurantGuestImmediately")]
        [HttpPost]
        public object UpdateRestaurantGuestImmediately(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantGuestImmediately(UpdateRestaurantGuest);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }
    }
}