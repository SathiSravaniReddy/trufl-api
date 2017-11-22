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
    [RoutePrefix("api/WaitListUser")]
    public class HostessWaitListUserController : ApiController
    {

        AdminBL _adminBL = new AdminBL();
        HostessBL _hostessBL = new HostessBL();
        JsonResponseResult JsonResponseResult = new JsonResponseResult();

        #region WaitList

        [Route("GetWaitListUsers/{RestaurantID}")]
        [HttpGet]
        public object GetWaitListUsers(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetWaitListUsers(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveWaitedlistBooking")]
        [HttpPost]
        public object SaveWaitedlistBooking(BookingTableDTO bookingTableInput)
        {
            bool res = _hostessBL.SaveWaitedlistBooking(bookingTableInput);
            try
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantTableAmount/{RestaurantID}/{TableNumber}")]
        [HttpGet]
        public object GetRestaurantTableAmount(int RestaurantID, int TableNumber)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetRestaurantTableAmount(RestaurantID, TableNumber);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("AcceptedandRemovedWaitedUser/{BookingID}/{BookinStatus}")]
        [HttpPost]
        public object AcceptedWaitedUser(int BookingID, int BookinStatus)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.AcceptedWaitedUser(BookingID, BookinStatus);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateBooking")]
        [HttpPost]
        public object UpdateBooking(UpdateBookingTableNumberDTO updateBookingTableNumber)
        {
            bool res = _hostessBL.UpdateBooking(updateBookingTableNumber);
            try
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetSeatAGuest/{RestaurantID}")]
        [HttpGet]
        public object GetSeatAGuest(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetSeatAGuest(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantGetSeatedNow/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantGetSeatedNow(int RestaurantID)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantGetSeatedNow(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveRestaurantGetSeatedNow")]
        [HttpPost]
        public object SaveRestaurantGetSeatedNow(SaveGetSeatedNow saveGetSeatedNow)
        {
            try
            {
                bool res = _hostessBL.SaveRestaurantGetSeatedNow(saveGetSeatedNow);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateWaitListAccept/{BookingID}/{TableNumbers}")]
        [HttpPost]
        public object UpdateWaitListAccept(int BookingID, string TableNumbers)
        {
            try
            {
                bool res = _hostessBL.UpdateWaitListAccept(BookingID, TableNumbers);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SendPushNotification/{TruflUserID}")]
        [HttpGet]
        public object SendPushNotification(int TruflUserID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.SendPushNotification(TruflUserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SendPushNotification")]
        [HttpPost]
        public object SendPushNotification(PushNotification pushNotification)
        {
            try
            {
                string res = _hostessBL.SendPushNotification(pushNotification);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        #endregion

        #region StartService

        [Route("GetRestaurantOpenSections/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantOpenSections(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetRestaurantOpenSections(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateRestaurantActiveSections")]
        [HttpPost]
        public object UpdateRestaurantActiveSections([FromBody]List<RestaurantActiveSectionsDTO> restaurantActiveSections)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantActiveSections(restaurantActiveSections);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveRestaurantOpenSectionStaff")]
        [HttpPost]
        public object SaveRestaurantOpenSectionStaff([FromBody]List<RestaurantSectionStaffDTO> restaurantSectionStaff)
        {
            try
            {
                bool res = _hostessBL.SaveRestaurantOpenSectionStaff(restaurantSectionStaff);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantWaitTimeOpenSectionStaff/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantWaitTimeOpenSectionStaff(int RestaurantID)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantWaitTimeOpenSectionStaff(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveRestaurantOpenTime")]
        [HttpPost]
        public object SaveRestaurantOpenTime(int RestaurantID, string Time)
        {
            try
            {
                bool res = _hostessBL.SaveRestaurantOpenTime(RestaurantID, Time);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantHostessOpenSectionDetails/{RestaurantID}/{UserType}")]
        [HttpGet]
        public object GetRestaurantHostessOpenSectionDetails(int RestaurantID, string UserType)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetRestaurantHostessOpenSectionDetails(RestaurantID, UserType);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantSectionTables/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantSectionTables(int RestaurantID)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantSectionTables(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantSelectStaff/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantSelectStaff(int RestaurantID)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantSelectStaff(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("AssignColorsToServer/{strColors}/{RestaurantID}")]
        [HttpGet]
        public object AssignColorsToServer(string strColors, int RestaurantID)
        {
            try
            {
                DataTable dtResult = _hostessBL.AssignColorsToServer(strColors, RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = dtResult, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateRestaurantOpenDate/{RestaurantID}")]
        [HttpPost]
        public object UpdateRestaurantOpenDate(int RestaurantID)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantOpenDate(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetRestaurantOpenDate/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantOpenDate(int RestaurantID)
        {
            try
            {
                int iResult = _hostessBL.GetRestaurantOpenDate(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = iResult, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        #endregion

        #region Snapshot Settings

        [Route("GetRestaurantStaffTables/{RestaurantID}")]
        [HttpGet]
        public object GetRestaurantStaffTables(int RestaurantID)
        {
            DataSet res = new DataSet();
            try
            {
                res = _hostessBL.GetRestaurantStaffTables(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetServerwiseSnapshot/{RestaurantID}")]
        [HttpGet]
        public object GetServerwiseSnapshot(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetServerwiseSnapshot(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetCapacitywiseSnapshot/{RestaurantID}")]
        [HttpGet]
        public object GetCapacitywiseSnapshot(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetCapacitywiseSnapshot(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("GetTablewiseSnapshot/{RestaurantID}")]
        [HttpGet]
        public object GetTablewiseSnapshot(int RestaurantID)
        {
            DataTable res = new DataTable();
            try
            {
                res = _hostessBL.GetTablewiseSnapshot(RestaurantID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateServerClockOut/{RestaurantID}/{CurrentUserID}/{NewUserID}")]
        [HttpPost]
        public object UpdateServerClockOut(int RestaurantID, int CurrentUserID, int NewUserID)
        {
            try
            {
                bool res = _hostessBL.UpdateServerClockOut(RestaurantID, CurrentUserID, NewUserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateSwitchServer/{RestaurantID}/{TableNumber}/{NewUserID}")]
        [HttpPost]
        public object UpdateSwitchServer(int RestaurantID, int TableNumber, int NewUserID)
        {
            try
            {
                bool res = _hostessBL.UpdateSwitchServer(RestaurantID, TableNumber, NewUserID);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveDefineSections")]
        [HttpPost]
        public object SaveDefineSections(List<RestaurantDefineSections> restaurantDefineSections)
        {
            try
            {
                bool res = _hostessBL.SaveDefineSections(restaurantDefineSections);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("SaveManageServer")]
        [HttpPost]
        public object SaveManageServer(List<RestaurantManageServer> restaurantManageServer)
        {
            try
            {
                bool res = _hostessBL.SaveManageServer(restaurantManageServer);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateSnapshotTableEmptyAndCheck/{RestaurantID}/{TableNumber}/{UpdateType}")]
        [HttpPost]
        public object UpdateSnapshotTableEmptyAndCheck(int RestaurantID, int TableNumber, string UpdateType)
        {
            try
            {
                bool res = _hostessBL.UpdateSnapshotTableEmptyAndCheck(RestaurantID, TableNumber, UpdateType);
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeSuccess, _Data = res, _StatusCode = TruflConstants._StatusCodeOK, _StatusMessage = TruflConstants._StatusMessageSuccess });
            }
            catch (Exception ex)
            {
                return Json(new JsonResponseResult { _ErrorCode = TruflConstants._ErrorCodeFailed, _Data = ex.ToString(), _StatusCode = TruflConstants._StatusCodeFailed, _StatusMessage = TruflConstants._StatusMessageFailed });
            }
        }

        [Route("UpdateRestaurantSectionOpenClose/{RestaurantID}/{FloorNumber}/{ActiveStatus}")]
        [HttpPost]
        public object UpdateRestaurantSectionOpenClose(int RestaurantID, int FloorNumber, int ActiveStatus)
        {
            try
            {
                bool res = _hostessBL.UpdateRestaurantSectionOpenClose(RestaurantID, FloorNumber, ActiveStatus);
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