using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trufl.Data_Access_Layer;
using DTO;

namespace Trufl.Business_Layer
{
    public class HostessBL
    {

        HostessDL _hostessDL = new HostessDL();

        #region SeatedUserController

        public DataTable GetRestaurantSeatedUsers(int RestaurantID)
        {
            return _hostessDL.GetRestaurantSeatedUsers(RestaurantID);
        }

        public bool SaveSeatBooking(List<RestaurantSeatedUsersDTO> restaurantSeatedUsersInputDTO)
        {
            return _hostessDL.SaveSeatBooking(restaurantSeatedUsersInputDTO);
        }

        public bool UpdateExtraTime(int BookingID, int AddTime)
        {
            return _hostessDL.UpdateExtraTime(BookingID, AddTime);
        }

        public bool UpdateCheckReceived(int BookingID)
        {
            return _hostessDL.UpdateCheckReceived(BookingID);
        }

        public bool UpdateEmptyBookingStatus(int BookingID)
        {
            return _hostessDL.UpdateEmptyBookingStatus(BookingID);
        }

        public bool VerifySeatedUsers(int BookingID, int TruflUserID, int RestaurantID)
        {
            return _hostessDL.VerifySeatedUsers(BookingID, TruflUserID, RestaurantID);
        }

        #endregion

        #region LoginController

        public DataTable GetUserTypes(string UserType, int RestaurantID)
        {
            return _hostessDL.GetUserTypes(UserType, RestaurantID);
        }

        public bool SaveSignUpUserInfo(TruflUserDTO registerUserInfo)
        {
            return _hostessDL.SaveSignUpUserInfo(registerUserInfo);
        }

        public DataTable LoginAuthentication(LoginDTO loginInput)
        {
            return _hostessDL.LoginAuthentication(loginInput);
        }

        public DataTable ForgetPassword(string LoginEmail)
        {
            return _hostessDL.ForgetPassword(LoginEmail);
        }

        public DataTable SaveRestPassword(RestPasswordDTO restPasswordInput)
        {
            return _hostessDL.SaveRestPassword(restPasswordInput);
        }

        public DataTable GetTruflUserDetails(int TruflUserID)
        {
            return _hostessDL.GetTruflUserDetails(TruflUserID);
        }

        public DataTable GetRestaurantDetails(int RestaurantID)
        {
            return _hostessDL.GetRestaurantDetails(RestaurantID);
        }

        #endregion

        #region HostessSettingsController

        public bool SaveUserBioEvents(SaveUserBioEventsDTO saveUserBioEvents)
        {
            return _hostessDL.SaveUserBioEvents(saveUserBioEvents);
        }

        public DataTable GetBioCategories()
        {
            return _hostessDL.GetBioCategories();
        }

        public DataTable GetBioEvents(int BioID)
        {
            return _hostessDL.GetBioEvents(BioID);
        }

        public DataTable GetEmployeConfiguration(string TruflUserType, int RestaurantID)
        {
            return _hostessDL.GetEmployeConfiguration(TruflUserType, RestaurantID);
        }

        #endregion

        #region HostessRestaurantEmployeeController

        public SettingsDTO GetRestaurantUserDetails(int? RestaurantID, int TruflUserID, string UserType)
        {
            return _hostessDL.GetRestaurantUserDetails(RestaurantID, TruflUserID, UserType);
        }

        public bool UpdateRestaurantHostStatus(UpdateRestaurantHostStatusDTO UpdateRestaurantHost)
        {
            return _hostessDL.UpdateRestaurantHostStatus(UpdateRestaurantHost);
        }

        public bool UpdateRestaurantEmployee(EmployeeConfigDTO employeeConfigDTO)
        {
            return _hostessDL.UpdateRestaurantEmployee(employeeConfigDTO);
        }

        public DataTable GetRestaurantTables(int RestaurantID, int UserID)
        {
            return _hostessDL.GetRestaurantTables(RestaurantID, UserID);
        }

        public DataTable GetVerifyEmailID()
        {
            return _hostessDL.GetVerifyEmailID();
        }

        public DataSet GetRestaurantGuest(int RestaurantID, int UserId, string UserType)
        {
            return _hostessDL.GetRestaurantGuest(RestaurantID, UserId, UserType);
        }

        public bool SaveRestaurantGuest(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            return _hostessDL.SaveRestaurantGuest(SaveRestaurantGuest);
        }

        public bool UpdateRestaurantGuest(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            return _hostessDL.UpdateRestaurantGuest(UpdateRestaurantGuest);
        }

        public bool SaveRestaurantGuestImmediately(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            return _hostessDL.SaveRestaurantGuestImmediately(SaveRestaurantGuest);
        }

        public bool UpdateRestaurantGuestImmediately(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            return _hostessDL.UpdateRestaurantGuestImmediately(UpdateRestaurantGuest);
        }

        #endregion

        #region HostessWaitListUserController

        #region WaitList

        public List<UserProfile> RetrieveUser()
        {
            return _hostessDL.RetrieveUser();
        }

        public DataTable GetWaitListUsers(int RestaurantID)
        {
            return _hostessDL.GetWaitListUsers(RestaurantID);
        }

        public bool ResetWaitList(int RestaurantID)
        {
            return _hostessDL.ResetWaitList(RestaurantID);
        }

        public bool UpdateAcceptOffer(int BookingID, int BookingStatus)
        {
            return _hostessDL.UpdateAcceptOffer(BookingID, BookingStatus);
        }

        public DataTable UpdateWaitListAcceptNotify(int RestaurantID, int BookingID, string UpdateType)
        {
            return _hostessDL.UpdateWaitListAcceptNotify(RestaurantID, BookingID, UpdateType);
        }
        public bool SaveWaitedlistBooking(BookingTableDTO bookingTableInput)
        {
            return _hostessDL.SaveWaitedlistBooking(bookingTableInput);
        }

        public DataTable GetRestaurantTableAmount(int RestaurantID, int TableNumber)
        {
            return _hostessDL.GetRestaurantTableAmount(RestaurantID, TableNumber);
        }

        public bool UpdateBooking(UpdateBookingTableNumberDTO updateBookingTableNumber)
        {
            return _hostessDL.UpdateBooking(updateBookingTableNumber);
        }

        public DataTable GetSeatAGuest(int RestaurantID)
        {
            return _hostessDL.GetSeatAGuest(RestaurantID);
        }

        public DataSet GetRestaurantGetSeatedNow(int RestaurantID)
        {
            return _hostessDL.GetRestaurantGetSeatedNow(RestaurantID);
        }

        public bool SaveRestaurantGetSeatedNow(SaveGetSeatedNow saveGetSeatedNow)
        {
            return _hostessDL.SaveRestaurantGetSeatedNow(saveGetSeatedNow);
        }

        public bool UpdateWaitListSeated(int BookingID, string TableNumbers)
        {
            return _hostessDL.UpdateWaitListSeated(BookingID, TableNumbers);
        }

        public DataTable SendPushNotification(int TruflUserID)
        {
            return _hostessDL.SendPushNotification(TruflUserID);
        }

        public string SendPushNotification(PushNotification pushNotification)
        {
            return _hostessDL.SendPushNotification(pushNotification);
        }

        #endregion

        #region StartService

        public DataTable GetRestaurantOpenSections(int RestaurantID)
        {
            return _hostessDL.GetRestaurantOpenSections(RestaurantID);
        }

        public bool UpdateRestaurantActiveSections(List<RestaurantActiveSectionsDTO> restaurantActiveSections)
        {
            return _hostessDL.UpdateRestaurantActiveSections(restaurantActiveSections);
        }

        public bool SaveRestaurantOpenSectionStaff(List<RestaurantSectionStaffDTO> restaurantSectionStaff)
        {
            return _hostessDL.SaveRestaurantOpenSectionStaff(restaurantSectionStaff);
        }

        public DataSet GetRestaurantWaitTimeOpenSectionStaff(int RestaurantID)
        {
            return _hostessDL.GetRestaurantWaitTimeOpenSectionStaff(RestaurantID);
        }

        public bool SaveRestaurantOpenTime(int RestaurantID, string Time)
        {
            return _hostessDL.SaveRestaurantOpenTime(RestaurantID, Time);
        }

        public DataTable GetRestaurantHostessOpenSectionDetails(int RestaurantID, string UserType)
        {
            return _hostessDL.GetRestaurantHostessOpenSectionDetails(RestaurantID, UserType);
        }

        public DataSet GetRestaurantSectionTables(int RestaurantID)
        {
            return _hostessDL.GetRestaurantSectionTables(RestaurantID);
        }

        public DataSet GetRestaurantSelectStaff(int RestaurantID)
        {
            return _hostessDL.GetRestaurantSelectStaff(RestaurantID);
        }

        public bool UpdateRestaurantOpenDate(int RestaurantID)
        {
            return _hostessDL.UpdateRestaurantOpenDate(RestaurantID);
        }

        public bool ResetRestaurantOpenDate(int RestaurantID)
        {
            return _hostessDL.ResetRestaurantOpenDate(RestaurantID);
        }

        public int GetRestaurantOpenDate(int RestaurantID)
        {
            return _hostessDL.GetRestaurantOpenDate(RestaurantID);
        }

        public DataTable AssignColorsToServer(string strColors, int RestaurantID)
        {
            return _hostessDL.AssignColorsToServer(strColors, RestaurantID);
        }

        #endregion

        #region Snapshot Setting

        public DataSet GetRestaurantStaffTables(int RestaurantID)
        {
            return _hostessDL.GetRestaurantStaffTables(RestaurantID);
        }

        public DataTable GetServerwiseSnapshot(int RestaurantID)
        {
            return _hostessDL.GetServerwiseSnapshot(RestaurantID);
        }

        public DataTable GetCapacitywiseSnapshot(int RestaurantID)
        {
            return _hostessDL.GetCapacitywiseSnapshot(RestaurantID);
        }

        public DataTable GetTablewiseSnapshot(int RestaurantID)
        {
            return _hostessDL.GetTablewiseSnapshot(RestaurantID);
        }

        public bool UpdateServerClockOut(int RestaurantID, int CurrentUserID, int NewUserID)
        {
            return _hostessDL.UpdateServerClockOut(RestaurantID, CurrentUserID, NewUserID);
        }

        public bool UpdateSwitchServer(int RestaurantID, int TableNumber, int NewUserID)
        {
            return _hostessDL.UpdateSwitchServer(RestaurantID, TableNumber, NewUserID);
        }

        public bool SaveDefineSections(List<RestaurantDefineSections> restaurantDefineSections)
        {
            return _hostessDL.SaveDefineSections(restaurantDefineSections);
        }

        public bool SaveManageServer(List<RestaurantManageServer> restaurantManageServer)
        {
            return _hostessDL.SaveManageServer(restaurantManageServer);
        }

        public bool UpdateSnapshotTableEmptyAndCheck(int RestaurantID, int TableNumber, string UpdateType)
        {
            return _hostessDL.UpdateSnapshotTableEmptyAndCheck(RestaurantID, TableNumber, UpdateType);
        }

        public bool UpdateRestaurantSectionOpenClose(int RestaurantID, int FloorNumber, int ActiveStatus)
        {
            return _hostessDL.UpdateRestaurantSectionOpenClose(RestaurantID, FloorNumber, ActiveStatus);
        }

        public bool GetVerifySnapShot(int RestaurantID)
        {
            return _hostessDL.GetVerifySnapShot(RestaurantID);
        }

        #endregion

        public DataSet GetRestaurantRewards(int TruflUserID, int RestaurantID)
        {
            return _hostessDL.GetRestaurantRewards(TruflUserID, RestaurantID);
        }

        public bool SaveRestaurantRewards(RestaurantRewards restaurantRewards)
        {
            return _hostessDL.SaveRestaurantRewards(restaurantRewards);
        }
        #endregion

    }
}
