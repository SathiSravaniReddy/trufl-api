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
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        AdminBL _adminBL = new AdminBL();
        HostessBL _hostessBL = new HostessBL();
        JsonResponseResult JsonResponseResult = new JsonResponseResult();
      
        #region LoginUser

        [Route("GetUserTypes/{UserType}/{RestaurantID}")]
        [HttpGet]
        public object GetUserTypes(string UserType, int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetUserTypes(UserType, RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SignUp")]
        [HttpPost]
        public object SaveSignUpUserInfo(TruflUserDTO registerUserInfo)
        {
            try
            {
            bool res = _hostessBL.SaveSignUpUserInfo(registerUserInfo);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("LoginAuthentication")]
        [HttpPost]
        public object LoginAuthentication(LoginDTO loginInput)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.LoginAuthentication(loginInput);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                //return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
                return Json(new JsonResponseResult { _ErrorCode = ((System.Data.SqlClient.SqlException)ex).Number.ToString(), _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = ex.Message });
            }
        }

        [Route("ForgetPassword")]
        [HttpGet]
        public object ForgetPassword(string LoginEmail)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.ForgetPassword(LoginEmail);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("RestPassword")]
        [HttpPost]
        public object SaveRestPassword(RestPasswordDTO restPasswordInput)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.SaveRestPassword(restPasswordInput);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetTruflUserDetails/{TruflUserID}")]
        [HttpGet]
        public object GetTruflUserDetails(int TruflUserID)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.GetTruflUserDetails(TruflUserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantDetails/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantDetails(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
            res = _hostessBL.GetRestaurantDetails(RestaurantID);
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
