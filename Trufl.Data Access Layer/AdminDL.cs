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
                if (QType == "REST")
                {
                    dt_MealTiming = CalcMealTime(ID);
                    sendResponse.Tables.Add(dt_MealTiming);
                    sendResponse.Tables[0].TableName = "RestaurantDetails";
                    sendResponse.Tables[1].TableName = "RestaurantMealTimings";
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

            var dtMeal = new DataTable();
            dtMeal.Columns.Add("MealTime", typeof(string));

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

                                if (blMealType)
                                    strDay = "          :" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                                else
                                {
                                    strDay = MealType.PadRight(10, ' ') + ":" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                                    blMealType = true;
                                }

                                dtMeal.Rows.Add(strDay);
                                MealStartTime = Convert.ToDateTime(drv["MealStartTime"]);
                                MealEndTime = Convert.ToDateTime(drv["MealEndTime"]);
                                sDay = Convert.ToInt16(drv["Day"]);
                                eDay = Convert.ToInt16(drv["Day"]);
                            }
                            else
                            {
                                eDay = Convert.ToInt16(drv["Day"]);
                            }
                        }
                        From = ((DayOfWeek)(sDay % 7)).ToString();
                        if (sDay == eDay)
                            To = "";
                        else
                            To = " - " + ((DayOfWeek)(eDay % 7)).ToString();

                        if (blMealType)
                            strDay = "          :" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                        else
                        {
                            strDay = MealType.PadRight(10, ' ') + ":" + From + To + " from " + MealStartTime.ToShortTimeString() + " to " + MealEndTime.ToShortTimeString();
                            blMealType = true;
                        }

                        dtMeal.Rows.Add(strDay);
                    }
                }
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

        public bool GetImages(int RestaurantID)
        {

            //string appdata = "https://truflimages.blob.core.windows.net/images/download.jpg";

            string imageName = "download.jpg";

            //string ImagePath = "https://truflimages.blob.core.windows.net/images/";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
            CloudFileShare share = fileClient.GetShareReference("hrms");
            CloudFileDirectory root = share.GetRootDirectoryReference();
            CloudFileDirectory dir = root.GetDirectoryReference(RestaurantID.ToString());
            dir.CreateIfNotExistsAsync();
            CloudFile cloudfile = dir.GetFileReference(imageName);

            cloudfile.FetchAttributes();

            long fileByteLength = cloudfile.Properties.Length;
            Byte[] myByteArray = new Byte[fileByteLength];

            cloudfile.DownloadToByteArray(myByteArray, 0);

            //string text = "";



            //text = convertByteToString(myByteArray);
            ////return appdata;

            //CloudBlobClient blobClient;
            //const string blobContainerName = "webappstoragedotnet-imagecontainer";
            //CloudBlobContainer blobContainer;

            //// Retrieve storage account information from connection string
            //// How to create a storage connection string - http://msdn.microsoft.com/en-us/library/azure/ee758697.aspx
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString"));

            //// Create a blob client for interacting with the blob service.
            //blobClient = storageAccount.CreateCloudBlobClient();
            //blobContainer = blobClient.GetContainerReference(blobContainerName);
            //await blobContainer.CreateIfNotExistsAsync();

            //// To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
            //// access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
            //// to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
            //// using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
            //await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //// Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
            //List<Uri> allBlobs = new List<Uri>();
            //foreach (IListBlobItem blob in blobContainer.ListBlobs())
            //{
            //    if (blob.GetType() == typeof(CloudBlockBlob))
            //        allBlobs.Add(blob.Uri);
            //}

            return true;// View(allBlobs);
        }

    }
}
