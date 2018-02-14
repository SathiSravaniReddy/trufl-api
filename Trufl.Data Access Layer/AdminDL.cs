using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DTO;
using Trufl.Logging;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.File;

namespace Trufl.Data_Access_Layer
{
    public class AdminDL
    {
        #region Db Connection 
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["TraflConnection"]);
        string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
        #endregion

        #region AdminDashBoardController

        public DashBoardDetailsDTO GetDashBoardDetails(DashBoardDTO dashboardInput)
        {
            DashBoardDetailsDTO response = new DashBoardDetailsDTO();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetDashBoardDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (dashboardInput != null)
                        {
                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@FromDate", dashboardInput.FromDate);
                            tvpParam.SqlDbType = SqlDbType.DateTime;
                            SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@ToDate", dashboardInput.ToDate);
                            tvparam1.SqlDbType = SqlDbType.DateTime;
                        }
                        else
                        {
                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);
                            tvpParam.SqlDbType = SqlDbType.DateTime;
                            SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);
                            tvparam1.SqlDbType = SqlDbType.DateTime;
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            //sendResponse = ds.Tables[0];  // Create
                            //sendResponse.Merge(ds.Tables[1]);
                            response.OffersRaised = ds.Tables[0];
                            response.OffersAccepted = ds.Tables[1];
                            response.OffersRemoved = ds.Tables[2];
                            response.VisitedCustomers = ds.Tables[3];
                            response.TotalNumberOfCustomers = ds.Tables[4];
                            response.NumberOfTruflRestaurants = ds.Tables[5];
                            response.RestaurantDetails = ds.Tables[6];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            //return sendResponse;
            return response;
        }

        #endregion

        #region AdminNotificationsController

        /// <summary>
        /// This method 'GetNotifications ' returns Notifications details
        /// </summary>
        /// <returns>Notifications List</returns>
        public DataTable GetNotifications(int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetNotifications", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
                // }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }
        
        /// <summary>
        /// This method 'SaveNotifications' will save Notifcation data
        /// </summary>
        /// <param name="SaveNotifications"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveNotifications(NotificationsDTO notifications)
        {
            try
            {
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveNotifications", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", notifications.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Description", notifications.Description);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@ExpiryDate ", notifications.ExpiryDate);
                        tvpParam2.SqlDbType = SqlDbType.DateTime;


                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();

                        if (status == -1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //string s = ex.ToString();
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        #endregion

        #region AdminRestaurantController

        /// This method 'spSaveRestaurant' will save Restaurant data
        /// </summary>
        /// <param name="SaveRestaurant"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveRestaurant(SaveRestaurantDTO restaurant)
        {
            try
            {
                var dtClient = new DataTable();

                dtClient.Columns.Add("RestaurantID", typeof(Int32));
                dtClient.Columns.Add("RestaurantName", typeof(string));
                dtClient.Columns.Add("Description", typeof(string));
                dtClient.Columns.Add("PrimaryContact", typeof(string));
                dtClient.Columns.Add("SecondaryContact", typeof(string));
                dtClient.Columns.Add("HoursofOperation", typeof(Int32));
                dtClient.Columns.Add("Parking", typeof(bool));
                dtClient.Columns.Add("Geo", typeof(string));
                dtClient.Columns.Add("Email", typeof(string));
                dtClient.Columns.Add("Address1", typeof(string));
                dtClient.Columns.Add("Address2", typeof(string));
                dtClient.Columns.Add("State", typeof(string));
                dtClient.Columns.Add("Zipcode", typeof(string));
                dtClient.Columns.Add("OwnerName", typeof(string));
                dtClient.Columns.Add("OwnerContact1", typeof(string));
                dtClient.Columns.Add("OwnerContact2", typeof(string));
                dtClient.Columns.Add("OwnerEmail", typeof(string));
                dtClient.Columns.Add("GetSeatedOffer", typeof(bool));
                dtClient.Columns.Add("QuotedTime", typeof(string));
                dtClient.Columns.Add("ModifiedDate", typeof(DateTime));
                dtClient.Columns.Add("ModifiedBy", typeof(Int32));
                dtClient.Columns.Add("SeatingSize", typeof(Int32));
                dtClient.Columns.Add("NumberOfTables", typeof(Int32));
                dtClient.Columns.Add("MenuPath", typeof(Int32));

                dtClient.Rows.Add(restaurant.RestaurantID,
                                  restaurant.RestaurantName,
                                  restaurant.Description,
                                  restaurant.PrimaryContact,
                                  restaurant.SecondaryContact,
                                  restaurant.HoursofOperation,
                                  restaurant.Parking,
                                  restaurant.Geo,
                                  restaurant.Email,
                                  restaurant.Address1,
                                  restaurant.Address2,
                                  restaurant.State,
                                  restaurant.Zipcode,
                                  restaurant.OwnerName,
                                  restaurant.OwnerContact1,
                                  restaurant.OwnerContact2,
                                  restaurant.OwnerEmail,
                                  restaurant.GetSeatedOffer,
                                  restaurant.QuotedTime,
                                  restaurant.ModifiedDate,
                                  restaurant.ModifiedBy,
                                  restaurant.SeatingSize,
                                  restaurant.NumberOfTables,
                                  restaurant.MenuPath
                                   );

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurant", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantTY", dtClient);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@LoggedInUser", restaurant.LoggedInUser);
                        tvpParam1.SqlDbType = SqlDbType.Int;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();

                        if (status == -1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
        }
        
        /// <summary>
        /// This method 'GetAllRestaurants ' returns AllRestaurants details
        /// </summary>
        /// <returns>Notifications List</returns>
        public DataSet GetAllRestaurants(int ID, string QType)
        {
            DataSet sendResponse = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetAllRestaurants", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@ID", ID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Type", QType);
                        tvpParam1.SqlDbType = SqlDbType.Text;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }

                DataTable dt_MealTiming = new DataTable();
                DataSet ds_ImageUrl = new DataSet();
                if (QType.ToUpper() == "ALL")
                    sendResponse.Tables[0].TableName = "AllRestaurants";

                else if (QType.ToUpper() == "USER")
                    sendResponse.Tables[0].TableName = "UserRestaurants";

                else if (QType.ToUpper() == "REST")
                    {

                    dt_MealTiming = CalcMealTime(ID);
                    //if (dsReturnRestDetails.Tables["OpenUntil"].Rows.Count > 0)
                    //{
                    //    sendResponse.Tables[0].Rows[0]["OpenUntil"] = dsReturnRestDetails.Tables[0].Rows[0]["OpenUntil"];
                    //}
                    sendResponse.Tables.Add(dt_MealTiming);

                    ds_ImageUrl = GetRestaurantImageUrls(ID);

                    if (ds_ImageUrl.Tables["MainLogo"].Rows.Count > 0)
                    {
                        sendResponse.Tables[0].Rows[0]["Image_Url"] = ds_ImageUrl.Tables["MainLogo"].Rows[0]["MainLogo"];
                    }
                    sendResponse.Tables.Add(ds_ImageUrl.Tables["RestImageUri"].Copy());
                    sendResponse.Tables[0].TableName = "RestaurantDetails";
                    sendResponse.Tables[1].TableName = "RestaurantMealTimings";
                    sendResponse.Tables[2].TableName = "RestaurantImageUrls";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        public DataSet GetRestaurantMealTimings(int RestaurantID)
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantMealTimings", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                        dsResponse.Tables[0].TableName = "MealTiming";
                        dsResponse.Tables[1].TableName = "MealType";

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResponse;
        }

        public DataTable CalcMealTime(int RestaurantID)
        {
            DataSet dsMealTiming;
            DateTime MealStartTime, MealEndTime;
            int sDay, eDay, MealID;
            string strDay, MealType, From, To;
            bool blMealType;
            DataRow drnew;

            DataTable dtMeal = new DataTable();
            dtMeal.Columns.Add("MealID", typeof(string));
            dtMeal.Columns.Add("MealType", typeof(string));
            dtMeal.Columns.Add("MealTime", typeof(string));

            //ToDay = Convert.ToInt16(DateTime.Now.DayOfWeek);
            try
            {
                dsMealTiming = GetRestaurantMealTimings(RestaurantID);
                foreach (DataRow drMealType in dsMealTiming.Tables["MealType"].Rows)
                {
                    MealID = Convert.ToInt16(drMealType["MealID"]);
                    MealType = Convert.ToString(drMealType["MealType"]);
                    blMealType = false;

                    DataView dv = new DataView(dsMealTiming.Tables["MealTiming"]);
                    dv.RowFilter = "MealID = " + MealID;
                    dv.Sort = "MealStartTime, MealEndTime";
                    if (dv.Count > 0)
                    {
                        MealStartTime = Convert.ToDateTime(dv[0]["MealStartTime"]);
                        MealEndTime = Convert.ToDateTime(dv[0]["MealEndTime"]);
                        sDay = Convert.ToInt16(dv[0]["Day"]);
                        eDay = Convert.ToInt16(dv[0]["Day"]);

                        foreach (DataRowView drv in dv)
                        {
                            if ((Convert.ToDateTime(drv["MealStartTime"]) != MealStartTime) || (Convert.ToDateTime(drv["MealEndTime"]) != MealEndTime))
                            {
                                From = ((DayOfWeek)(sDay % 7)).ToString();
                                if (sDay == eDay)
                                    To = "";
                                else
                                    To = " - " + ((DayOfWeek)(eDay % 7)).ToString();

                                //if (blMealType)
                                //    strDay = "          :" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                                //else
                                //{
                                //    strDay = MealType.PadRight(10, ' ') + ":" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                                //    blMealType = true;
                                //}
                                strDay = From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();

                                drnew = dtMeal.NewRow();
                                drnew["MealID"] = MealID;
                                drnew["MealType"] = MealType;
                                drnew["MealTime"] = strDay;
                                dtMeal.Rows.Add(drnew);

                                //dtMeal.Rows.Add(strDay);
                                MealStartTime = Convert.ToDateTime(drv["MealStartTime"]);
                                MealEndTime = Convert.ToDateTime(drv["MealEndTime"]);
                                sDay = Convert.ToInt16(drv["Day"]);
                                eDay = Convert.ToInt16(drv["Day"]);
                            }
                            else
                            {
                                eDay = Convert.ToInt16(drv["Day"]);
                            }
                            //if ((MealID == 3) && (ToDay == eDay))
                            //{
                            //    OpenUntil = MealEndTime.ToShortTimeString();
                            //    dtMeal.Rows.Add(OpenUntil);
                            //}
                        }
                        From = ((DayOfWeek)(sDay % 7)).ToString();
                        if (sDay == eDay)
                            To = "";
                        else
                            To = " - " + ((DayOfWeek)(eDay % 7)).ToString();

                        //if (blMealType)
                        //    //strDay = "          :" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                        //    strDay = From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                        //else
                        //{
                        //    //strDay = MealType.PadRight(10, ' ') + ":" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                        //    strDay = From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                        //    blMealType = true;
                        //}
                        strDay = From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();

                        drnew = dtMeal.NewRow();
                        drnew["MealID"] = MealID;
                        drnew["MealType"] = MealType;
                        drnew["MealTime"] = strDay;
                        dtMeal.Rows.Add(drnew);
                    }
                }
                dtMeal.TableName = "Meal";
                return dtMeal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the Restaurant Settings to the DB
        /// </summary>
        /// <param name="RestaurantSettings">Class object with the values for restaurant Settings</param>
        /// <returns>returns 1 on saving the password , or 0 on error</returns>
        public bool SaveRestaurantSettings(RestaurantSettingsDTO RestaurantSettings)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantSettings", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantSettings.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@DiningTime", RestaurantSettings.DiningTime);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Geofence", RestaurantSettings.Geofence);
                        tvpParam2.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@TableNowCapacity", RestaurantSettings.TableNowCapacity);
                        tvpParam3.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@DefaultTableNowPrice", RestaurantSettings.DefaultTableNowPrice);
                        tvpParam4.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@MinimumTableNowPrice", RestaurantSettings.MinimumTableNowPrice);
                        tvpParam5.SqlDbType = SqlDbType.Int;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();

                        if (status == -1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
        }

        /// <summary>
        /// get the restaurant Settings details for edit
        /// </summary>
        /// <param name="RestaurantID">Restaurant ID as Input</param>
        /// <returns>returns a DataTable with the Restaurant settings details</returns>
        public DataTable GetRestaurantSettings(int RestaurantID)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantSettings", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;


                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtsendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dtsendResponse;
        }

        #endregion

        #region TruflAdminLoginController

       /// <summary>
        /// This method 'SaveProfilePassword' will Save Profile Password 
        /// </summary>
        /// <param name="restPasswordInput"></param>
        /// <returns>returns 1 on saving the password , or 0 on error</returns>
        public bool SaveProfilePassword(RestPasswordDTO restPasswordInput)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SaveProfilePassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@UserID", restPasswordInput.UserID);
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserName", restPasswordInput.UserName);
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@UserEmail", restPasswordInput.UserEmail);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        //SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@LoginPassword", DBNull.Value);

                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@NewLoginPassword", restPasswordInput.NewLoginPassword);
                        tvpParam4.SqlDbType = SqlDbType.Text;

                        int status = cmd.ExecuteNonQuery();

                        if (status == -1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //string s = ex.ToString();
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        #endregion

        public bool SaveUserRestFavoutrite(UserFavoutiteRestaurant userFavoutiteRestaurant)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveUserRestFavoutrite", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", userFavoutiteRestaurant.TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", userFavoutiteRestaurant.RestaurantID);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@IsFav", userFavoutiteRestaurant.IsFav);
                        tvpParam2.SqlDbType = SqlDbType.Bit;

                        int status = cmd.ExecuteNonQuery();
                        if (status == -1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
        }

        public DataSet CalculatedWaittime(int RestaurantID, int PartySize)
        {
            int TotalAvailable, currAvailable, cntWaitlist = 0, cntSeated = 0, count = 0;
            int AddWaitTime = 0, calTtype, CalculatedWaittime = 0;
            string WStabletype = "";
            char TType;
            string strSeatedMin = "";
            int maxTableType, ReqTableType = 0;
            bool sizefound = false;
            int RemDiningTime, DiningTime = 0, nextDining = 0, cntGetTableNow, WaitTime = 0;
            DataSet ds_ReturnWaitTime = new DataSet();

            DataSet dsCalcWaitTime = new DataSet();
            DataTable dt_GetWaitTIme = new DataTable();
            dt_GetWaitTIme.Columns.Add("TableTypeUsed", typeof(string));
            dt_GetWaitTIme.Columns.Add("CalculatedWaittime", typeof(int));
            try
            {

           
            dsCalcWaitTime = GetRestaurantWaitTimeData(RestaurantID);
            if (dsCalcWaitTime.Tables["DiningTime"].Rows.Count > 0)
                DiningTime = Convert.ToInt16(dsCalcWaitTime.Tables["DiningTime"].Rows[0]["DiningTime"]);

            //CheckNoWait
            sizefound = false;
            for (int i = 0; i <= dsCalcWaitTime.Tables["WaitTimeStatus"].Rows.Count - 1; i++)
            {
                DataView dv = new DataView(dsCalcWaitTime.Tables["AvblByTable"]);
                DataView dvTableNow = new DataView(dsCalcWaitTime.Tables["GetSeatedNow"]);
                calTtype = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]);
                TType = Convert.ToChar(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"].ToString());
                dv.RowFilter = "TableType = " + calTtype;
                dvTableNow.RowFilter = "TableType = " + calTtype;
                maxTableType = calTtype;
                if (dvTableNow.Count > 0)
                    cntGetTableNow = Convert.ToInt16(dvTableNow[0]["NumberOfTables"]);
                else
                    cntGetTableNow = 0;

                if (dv.Count > 0)
                    currAvailable = Convert.ToInt16(dv[0]["Available"]) - cntGetTableNow;
                else
                    currAvailable = 0;
                for (int j = 0; j < dsCalcWaitTime.Tables["Waitlist"].Rows.Count; j++)
                {
                    WStabletype = dsCalcWaitTime.Tables["Waitlist"].Rows[j]["WaitListTableType"].ToString();
                    count = WStabletype.Split(TType).Length - 1;
                    cntWaitlist += count;
                }
                for (int j = 0; j < dsCalcWaitTime.Tables["Seated"].Rows.Count; j++)
                {
                    WStabletype = dsCalcWaitTime.Tables["Seated"].Rows[j]["SeatedTableType"].ToString();
                    count = WStabletype.Split(TType).Length - 1;
                    if (count > 0)
                    {
                        cntSeated += count;
                        for (int k = 0; k < count; k++)
                        {
                            strSeatedMin = strSeatedMin + dsCalcWaitTime.Tables["Seated"].Rows[j]["TimeRemaining"].ToString() + ",";
                        }
                    }
                }

                string[] SeatedMin = strSeatedMin.Split(',');
                TotalAvailable = currAvailable - cntWaitlist;
                dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["Available"] = TotalAvailable;
                dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["GetTableNow"] = cntGetTableNow;
                dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["SeatedList"] = strSeatedMin;


                if (TotalAvailable > 0)
                {
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"] = 0;
                }
                else
                {
                    int TotalTables = cntSeated + currAvailable;
                    while ((cntWaitlist >= TotalTables) && (TotalTables > 0))
                    {
                        cntWaitlist -= TotalTables;
                        AddWaitTime += DiningTime + 10;
                    }
                    RemDiningTime = 0;
                    if (cntSeated > 0)
                    {
                        if ((cntSeated > cntWaitlist) && (cntWaitlist - currAvailable) >= 0)
                        {
                            RemDiningTime = Convert.ToInt16(SeatedMin[cntWaitlist - currAvailable]);
                            RemDiningTime += 10;
                            RemDiningTime = Convert.ToInt16(Math.Round(RemDiningTime / 5.0) * 5);
                        }
                        if ((cntSeated > cntWaitlist) && (cntWaitlist - currAvailable) >= 1)
                        {
                            try
                            {
                                nextDining = Convert.ToInt16(SeatedMin[cntWaitlist - currAvailable + 1]);
                                nextDining += 10;
                                nextDining = Convert.ToInt16(Math.Round(RemDiningTime / 5.0) * 5);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    WaitTime = RemDiningTime + AddWaitTime;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"] = WaitTime;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["NextWT"] = nextDining + AddWaitTime;

                }
                if (PartySize <= Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]) & (!sizefound))
                {
                    ReqTableType = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]);
                    sizefound = true;
                    CalculatedWaittime = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"]);
                }
                else
                {
                    //PartySize -= maxTableType;

                }
                strSeatedMin = "";
                cntWaitlist = 0;
                cntSeated = 0;
                AddWaitTime = 0;
            }


            DataRow drnew = dt_GetWaitTIme.NewRow();
            drnew["TableTypeUsed"] = ReqTableType;
            drnew["CalculatedWaittime"] = CalculatedWaittime;

            dt_GetWaitTIme.Rows.Add(drnew);

                ds_ReturnWaitTime.Tables.Add(dt_GetWaitTIme);
                ds_ReturnWaitTime.Tables.Add(dsCalcWaitTime.Tables["WaitTimeStatus"].Copy());

                ds_ReturnWaitTime.Tables[0].TableName = "CalculatedWaittime";
                ds_ReturnWaitTime.Tables[1].TableName = "WaitTime";
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return ds_ReturnWaitTime;
        }

        private DataSet GetRestaurantWaitTimeData(int RestaurantID)
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetWaitTime", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                        dsResponse.Tables[0].TableName = "DefineSection";
                        dsResponse.Tables[1].TableName = "AvblByTable";
                        dsResponse.Tables[2].TableName = "GetSeatedNow";
                        dsResponse.Tables[3].TableName = "Waitlist";
                        dsResponse.Tables[4].TableName = "Seated";
                        dsResponse.Tables[5].TableName = "WaitTimeStatus";
                        dsResponse.Tables[6].TableName = "DiningTime";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResponse;
        }

        public DataSet RestaurantWaittime()
        {
            int TotalAvailable, currAvailable, cntWaitlist = 0, cntSeated = 0, count = 0;
            int AddWaitTime = 0, calTtype;
            string WStabletype = "";
            char TType;
            string strSeatedMin = "";
            int maxTableType, RestaurantID;
            int RemDiningTime, DiningTime = 0, nextDining = 0, cntGetTableNow, WaitTime = 0;
            DataSet ds_ReturnWaitTime = new DataSet();

            DataSet dsCalcWaitTime = new DataSet();
            try
            {
                dsCalcWaitTime = GetAllRestaurantWaitTimeData();
                for (int a = 0; a <= dsCalcWaitTime.Tables["Restaurant"].Rows.Count - 1; a++)
                {
                    RestaurantID = Convert.ToInt16(dsCalcWaitTime.Tables["Restaurant"].Rows[a]["RestaurantID"]);
                    DataView dvDining = new DataView(dsCalcWaitTime.Tables["DiningTime"]);
                    dvDining.RowFilter = "RestaurantID = " + RestaurantID;

                    if (dvDining.Count > 0)
                    DiningTime = Convert.ToInt16(dvDining[0]["DiningTime"]);

                //CheckNoWait
                for (int i = 0; i <= dsCalcWaitTime.Tables["WaitTimeStatus"].Rows.Count - 1; i++)
                {
                    DataView dv = new DataView(dsCalcWaitTime.Tables["AvblByTable"]);
                    DataView dvTableNow = new DataView(dsCalcWaitTime.Tables["GetSeatedNow"]);

                    calTtype = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]);
                    TType = Convert.ToChar(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"].ToString());

                        dv.RowFilter = "RestaurantID = " + RestaurantID + " AND TableType = " + calTtype;
                    dvTableNow.RowFilter = "RestaurantID = " + RestaurantID + " AND TableType = " + calTtype;
                    maxTableType = calTtype;
                    if (dvTableNow.Count > 0)
                        cntGetTableNow = Convert.ToInt16(dvTableNow[0]["NumberOfTables"]);
                    else
                        cntGetTableNow = 0;

                    if (dv.Count > 0)
                        currAvailable = Convert.ToInt16(dv[0]["Available"]) - cntGetTableNow;
                    else
                        currAvailable = 0;
                    for (int j = 0; j < dsCalcWaitTime.Tables["Waitlist"].Rows.Count; j++)
                    {
                            DataView dvWaitList = new DataView(dsCalcWaitTime.Tables["Waitlist"]);
                            dvWaitList.RowFilter = "RestaurantID = " + RestaurantID;

                            WStabletype = dvWaitList[j]["WaitListTableType"].ToString();
                        count = WStabletype.Split(TType).Length - 1;
                        cntWaitlist += count;
                    }
                    for (int j = 0; j < dsCalcWaitTime.Tables["Seated"].Rows.Count; j++)
                    {
                            DataView dvSeated = new DataView(dsCalcWaitTime.Tables["Seated"]);
                            dvSeated.RowFilter = "RestaurantID = " + RestaurantID;

                            WStabletype = dvSeated[j]["SeatedTableType"].ToString();
                        count = WStabletype.Split(TType).Length - 1;
                        if (count > 0)
                        {
                            cntSeated += count;
                            for (int k = 0; k < count; k++)
                            {
                                strSeatedMin = strSeatedMin + dvSeated[j]["TimeRemaining"].ToString() + ",";
                            }
                        }
                    }

                    string[] SeatedMin = strSeatedMin.Split(',');
                    TotalAvailable = currAvailable - cntWaitlist;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["RestaurantID"] = RestaurantID;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["Available"] = TotalAvailable;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["GetTableNow"] = cntGetTableNow;
                    dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["SeatedList"] = strSeatedMin;


                    if (TotalAvailable > 0)
                    {
                        dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"] = 0;
                    }
                    else
                    {
                        int TotalTables = cntSeated + currAvailable;
                        while ((cntWaitlist >= TotalTables) && (TotalTables > 0))
                        {
                            cntWaitlist -= TotalTables;
                            AddWaitTime += DiningTime + 10;
                        }
                        RemDiningTime = 0;
                        if (cntSeated > 0)
                        {
                            if ((cntSeated > cntWaitlist) && (cntWaitlist - currAvailable) >= 0)
                            {
                                RemDiningTime = Convert.ToInt16(SeatedMin[cntWaitlist - currAvailable]);
                                RemDiningTime += 10;
                                RemDiningTime = Convert.ToInt16(Math.Round(RemDiningTime / 5.0) * 5);
                            }
                            if ((cntSeated > cntWaitlist) && (cntWaitlist - currAvailable) >= 1)
                            {
                                try
                                {
                                    nextDining = Convert.ToInt16(SeatedMin[cntWaitlist - currAvailable + 1]);
                                    nextDining += 10;
                                    nextDining = Convert.ToInt16(Math.Round(RemDiningTime / 5.0) * 5);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        WaitTime = RemDiningTime + AddWaitTime;
                        dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"] = WaitTime;
                        dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["NextWT"] = nextDining + AddWaitTime;

                    }
                    //if (PartySize <= Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]) & (!sizefound))
                    //{
                    //    ReqTableType = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TableType"]);
                    //    sizefound = true;
                    //    CalculatedWaittime = Convert.ToInt16(dsCalcWaitTime.Tables["WaitTimeStatus"].Rows[i]["TStatus"]);
                    //}
                    //else
                    //{
                    //    //PartySize -= maxTableType;

                    //}
                    strSeatedMin = "";
                    cntWaitlist = 0;
                    cntSeated = 0;
                    AddWaitTime = 0;
                }


                ds_ReturnWaitTime.Tables.Add(dsCalcWaitTime.Tables["WaitTimeStatus"].Copy());

                ds_ReturnWaitTime.Tables[0].TableName = "WaitTime";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return ds_ReturnWaitTime;
        }

        private DataSet GetAllRestaurantWaitTimeData()
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetWaitTime", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                        dsResponse.Tables[0].TableName = "DefineSection";
                        dsResponse.Tables[1].TableName = "AvblByTable";
                        dsResponse.Tables[2].TableName = "GetSeatedNow";
                        dsResponse.Tables[3].TableName = "Waitlist";
                        dsResponse.Tables[4].TableName = "Seated";
                        dsResponse.Tables[5].TableName = "WaitTimeStatus";
                        dsResponse.Tables[6].TableName = "DiningTime";
                        dsResponse.Tables[7].TableName = "AllRestaurantWaitTime";
                        dsResponse.Tables[8].TableName = "Restaurant";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResponse;
        }

        public DataSet GetRestaurantImageUrls(int RestaurantID)
        {
            DataSet dsImageUrls = new DataSet();
            var dtRestImageUri = new DataTable();
            dtRestImageUri.Columns.Add("ImageUri", typeof(string));

            var dtMainLogo = new DataTable();
            dtMainLogo.Columns.Add("MainLogo", typeof(string));

            string key = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
            string blobcontainerName = ConfigurationManager.AppSettings["blobContainerName"].ToString();
            string blobDirectoryName = ConfigurationManager.AppSettings["blobDirectoryName"].ToString();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(key);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobcontainer = blobClient.GetContainerReference(blobcontainerName);
            //foreach (IListBlobItem item in blobcontainer.ListBlobs(null, false))
            //{
            //    if (item.GetType() == typeof(CloudBlockBlob))
            //    {
            //        CloudBlockBlob blob = (CloudBlockBlob)item;
            //        MessageBox.Show("Block blob of length " + blob.Properties.Length + ":" + blob.Uri);
            //    }
            //}

            CloudBlobDirectory blobDirectory = blobcontainer.GetDirectoryReference(blobDirectoryName + RestaurantID.ToString());
            //List<Uri> allBlobs = new List<Uri>();
            foreach (IListBlobItem item in blobDirectory.ListBlobs(true, BlobListingDetails.All, null))
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    if (blob.Name.ToUpper() == blobDirectoryName + RestaurantID.ToString() + "/MAINLOGO.JPG")
                        dtMainLogo.Rows.Add(blob.Uri);
                    else
                        dtRestImageUri.Rows.Add(blob.Uri);
                    //allBlobs.Add(blob.Uri);
                }
            }
            dsImageUrls.Tables.Add(dtMainLogo);
            dsImageUrls.Tables.Add(dtRestImageUri);
            dsImageUrls.Tables[0].TableName = "MainLogo";
            dsImageUrls.Tables[1].TableName = "RestImageUri";
            return dsImageUrls;// View(allBlobs);
        }

    }
}
