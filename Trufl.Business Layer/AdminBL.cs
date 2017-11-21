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
    public class AdminBL
    {
        AdminDL _adminDL = new AdminDL();
        #region Trufl_Admin
        public DashBoardDetailsDTO GetDashBoardDetails(DashBoardDTO dashboardInput)
        {
            return _adminDL.GetDashBoardDetails(dashboardInput);
        }

        public DataTable GetNotifications(int RestaurantID)
        {
            return _adminDL.GetNotifications(RestaurantID);
        }

        public bool SaveNotifications(NotificationsDTO notifications)
        {
            return _adminDL.SaveNotifications(notifications);
        }

        public bool SaveRestaurant(SaveRestaurantDTO restaurant)
        {
            return _adminDL.SaveRestaurant(restaurant);
        }

        public SettingsDTO GetRestaurantUserDetails(int? RestaurantID, int TruflUserID, string UserType)
        {
            return _adminDL.GetRestaurantUserDetails(RestaurantID,TruflUserID,UserType);
        }
        public DataTable GetAllRestaurants()
        {
            return _adminDL.GetAllRestaurants();
        }

        public bool SaveProfilePassword(RestPasswordDTO ProfilePassword)
        {
            return _adminDL.SaveProfilePassword(ProfilePassword);
        }
        #endregion

        public bool SaveRestaurantSettings(RestaurantSettingsDTO RestaurantSettings)
        {
            return _adminDL.SaveRestaurantSettings(RestaurantSettings);
        }

        public DataTable GetRestaurantSettings(int RestaurantID)
        {
            return _adminDL.GetRestaurantSettings(RestaurantID);
        }
    }
}