﻿using System;
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

        #region AdminDashBoardController
        
        public DashBoardDetailsDTO GetDashBoardDetails(DashBoardDTO dashboardInput)
        {
            return _adminDL.GetDashBoardDetails(dashboardInput);
        }

        #endregion

        #region AdminNotificationsController

        public DataTable GetNotifications(int RestaurantID)
        {
            return _adminDL.GetNotifications(RestaurantID);
        }

        public bool SaveNotifications(NotificationsDTO notifications)
        {
            return _adminDL.SaveNotifications(notifications);
        }

        #endregion

        #region AdminRestaurantController

        public bool SaveRestaurant(SaveRestaurantDTO restaurant)
        {
            return _adminDL.SaveRestaurant(restaurant);
        }

        public DataSet GetAllRestaurants(int ID, string QType)
        {
            return _adminDL.GetAllRestaurants(ID, QType);
        }
        
        public bool SaveRestaurantSettings(RestaurantSettingsDTO RestaurantSettings)
        {
            return _adminDL.SaveRestaurantSettings(RestaurantSettings);
        }

        public DataTable GetRestaurantSettings(int RestaurantID)
        {
            return _adminDL.GetRestaurantSettings(RestaurantID);
        }

        #endregion

        #region TruflAdminLoginController

        public bool SaveProfilePassword(RestPasswordDTO ProfilePassword)
        {
            return _adminDL.SaveProfilePassword(ProfilePassword);
        }

        #endregion
        public DataTable GetRestaurantImageUrls(int RestaurantID)
        {
            return _adminDL.GetRestaurantImageUrls(RestaurantID);
        }
    }
}