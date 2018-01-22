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
using TruflEmailService;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Trufl.Data_Access_Layer
{
    public class HostessDL
    {
        #region Db Connection 
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["TraflConnection"]);
        string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
        #endregion

        #region SeatedUserController

        /// <summary>
        /// This method 'RetrieveUser ' returns User details
        /// </summary>
        /// <returns>User List</returns>
        public DataTable GetRestaurantSeatedUsers(int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantSeatedUsers", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
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
        /// This method 'SaveSeatBooking' will save Seat data
        /// </summary>
        /// <param name="passParkingLots"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveSeatBooking(List<RestaurantSeatedUsersDTO> restaurantSeatedUsersInputDTO)
        {
            try
            {
                var dtClient = new DataTable();

                dtClient.Columns.Add("RestaurantID", typeof(Int32));
                dtClient.Columns.Add("TruflUserID", typeof(Int32));
                dtClient.Columns.Add("AmenitiName", typeof(string));
                dtClient.Columns.Add("AmenitiChecked", typeof(bool));

                for (int i = 0; restaurantSeatedUsersInputDTO.Count > i; i++)
                {
                    dtClient.Rows.Add(restaurantSeatedUsersInputDTO[i].RestaurantID,
                                   restaurantSeatedUsersInputDTO[i].TruflUserID,
                                   restaurantSeatedUsersInputDTO[i].AmenitiName,
                                   restaurantSeatedUsersInputDTO[i].AmenitiChecked
                                   );

                }

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantUserAmenities", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantUserAmenitiesTY", dtClient);
                        tvpParam.SqlDbType = SqlDbType.Structured;


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
                string s = ex.ToString();
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// Updates the extratime fields by +5/-5 min for the guests/ customer in seated on the click of jump +5
        /// </summary>
        /// <param name="BookingID">BookingID as Input</param>
        /// <param name="AddTime">AddTime as Input </param>
        /// <returns>returns 1 on updating the time, 0 on error</returns>
        public bool UpdateExtraTime(int BookingID, int AddTime)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateExtraTime", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Addtime", AddTime);
                        tvpParam1.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Update the status of Check Received to 1 if a check is dropped against the table by the customer
        /// </summary>
        /// <param name="BookingID">BookingID as Input</param>
        /// <returns>returns 1 on updating the checkReceived 0 on error</returns>
        public bool UpdateCheckReceived(int BookingID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateCheckReceived", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Used to set the status of the booking to emptied after completing the dinning
        /// </summary>
        /// <param name="BookingID">BookingID as Input</param>
        /// <returns>returns 1 on updating the booking status, 0 on error</returns>
        public bool UpdateEmptyBookingStatus(int BookingID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateEmptyBookingStatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
       /// Verify whether the user is already seated.
       /// </summary>
       /// <param name="BookingID"></param>
       /// <param name="TruflUserID"></param>
       /// <param name="RestaurantID"></param>
       /// <returns></returns>
        public bool VerifySeatedUsers(int BookingID, int TruflUserID, int RestaurantID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spVerifySeatedUsers", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@TruflUserID", TruflUserID);
                        tvpParam2.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam3.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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

        #endregion

        #region LoginController

        /// <summary>
        /// This method 'GetUserTypes ' returns User type details
        /// </summary>
        /// <returns>user type list</returns>
        public DataTable GetUserTypes(string UserType, int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetUserTypes", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@TruflUserType", UserType);
                        tvpParam1.SqlDbType = SqlDbType.Char;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'Save Register User' will save Register user data
        /// </summary>
        /// <param name="SaveSignUpUserInfo"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveSignUpUserInfo(TruflUserDTO registerUserInfo)
        {
            try
            {
                var dtClient = new DataTable();

                dtClient.Columns.Add("TruflUserID", typeof(Int32));
                dtClient.Columns.Add("RestaurantID", typeof(Int32));
                dtClient.Columns.Add("FullName", typeof(string));
                dtClient.Columns.Add("Email", typeof(string));
                dtClient.Columns.Add("pic", typeof(Byte[]));
                dtClient.Columns.Add("PhoneNumber", typeof(string));
                dtClient.Columns.Add("Password", typeof(string));
                //dtClient.Columns.Add("Salt", typeof(string));
                dtClient.Columns.Add("DOB", typeof(DateTime));
                dtClient.Columns.Add("ActiveInd", typeof(char));
                dtClient.Columns.Add("RestaurantEmpInd", typeof(Int32));
                dtClient.Columns.Add("TruflMemberType", typeof(string));
                dtClient.Columns.Add("TruflRelationship", typeof(Int32));
                dtClient.Columns.Add("TruflshareCode", typeof(string));
                dtClient.Columns.Add("ReferTruflUserID", typeof(Int32));
                dtClient.Columns.Add("ModifiedDate", typeof(DateTime));
                dtClient.Columns.Add("ModifiedBy", typeof(Int32));
                dtClient.Columns.Add("Waited", typeof(TimeSpan));

                //dtClient.Columns.Add("LoggedInUserType", typeof(string));
                dtClient.Rows.Add(
                                  DBNull.Value,
                                  DBNull.Value,
                                  registerUserInfo.FullName,
                                  registerUserInfo.Email,
                                  DBNull.Value,
                                  registerUserInfo.PhoneNumber,
                                  registerUserInfo.Password,
                                  //DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value,
                                  DBNull.Value
                                  );

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveTruflUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserTY", dtClient);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@LoggedInUserType", registerUserInfo.LoggedInUserType);

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'LoginAuthentication' will check the login authentication
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public DataTable LoginAuthentication(LoginDTO loginInput)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spLoginAuthentication", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@EMAIL_ID", loginInput.emailid);
                        tvpParam.SqlDbType = SqlDbType.Text;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@PASSWORD", loginInput.password);
                        tvparam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@UserType", loginInput.usertype);
                        tvparam2.SqlDbType = SqlDbType.Text;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'GetForgetPassword' will ForgetPassword 
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns ForgetPassword Details </returns>
        public DataTable ForgetPassword(string LoginEmail)
        {
            DataTable sendResponse = new DataTable();
            MailUtility email = new MailUtility();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetForgetPassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@LoginEmail", LoginEmail);
                        tvpParam.SqlDbType = SqlDbType.Text;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                            ResetPasswordEmailDTO data = new ResetPasswordEmailDTO();
                            data.To = sendResponse.Rows[0]["To"].ToString();
                            data.Subject = sendResponse.Rows[0]["Subject"].ToString();
                            data.Body = sendResponse.Rows[0]["Body"].ToString();
                            data.BodyFormat = sendResponse.Rows[0]["BodyFormat"].ToString();
                            email.sendMail(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'SaveRestPassword' will RestPassword 
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns RestPassword  </returns>
        public DataTable SaveRestPassword(RestPasswordDTO restPasswordInput)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SavePassword", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //SqlParameter tvpParam = cmd.Parameters.AddWithValue("@UserID", restPasswordInput.UserID);
                        //SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserName", restPasswordInput.UserName);
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserEmail", restPasswordInput.UserEmail);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@LoginPassword", restPasswordInput.LoginPassword);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@NewLoginPassword", restPasswordInput.NewLoginPassword);
                        tvpParam3.SqlDbType = SqlDbType.Text;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'spGetTruflUserDetails' will TruflUserDetails 
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns Get Trufl User Details  </returns>
        public DataTable GetTruflUserDetails(int TruflUserID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetTruflUserDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'GetRestaurantDetails' will RestaurantDetails 
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns Get Restaurant Details  </returns>
        public DataTable GetRestaurantDetails(int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        #endregion

        #region HostessSettingsController

        /// <summary>
        /// This method 'Save User Bio Events' will save User Bio Events data
        /// </summary>
        /// <param name="SaveSignUpUserInfo"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveUserBioEvents(SaveUserBioEventsDTO saveUserBioEvents)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveUserBioEvents", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                         SqlParameter tvparam = cmd.Parameters.AddWithValue("@TruflUserID", saveUserBioEvents.TruflUserID);
                        tvparam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Relationship", saveUserBioEvents.Relationship);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@ThisVisit", saveUserBioEvents.ThisVisit);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@FoodAndDrink", saveUserBioEvents.FoodAndDrink);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@SeatingPreferences", saveUserBioEvents.SeatingPreferences);
                        tvpParam4.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@Description", saveUserBioEvents.Description);
                        tvpParam5.SqlDbType = SqlDbType.Text;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //var s = ex.Message;
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saveUserCardDetailsDTO"></param>
        /// <returns></returns>
        public bool SaveUserCardDetails(SaveUserCardDetailsDTO saveUserCardDetails)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveUserCardDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvparam = cmd.Parameters.AddWithValue("@TruflUserID", saveUserCardDetails.TruflUserID);
                        tvparam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@CardNo", saveUserCardDetails.CardNo);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@NameOnCard", saveUserCardDetails.NameOnCard);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@BillingAddress1", saveUserCardDetails.BillingAddress1);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@BillingAddress2", saveUserCardDetails.BillingAddress2);
                        tvpParam4.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@City", saveUserCardDetails.City);
                        tvpParam5.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@State", saveUserCardDetails.State);
                        tvpParam6.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@Zip", saveUserCardDetails.Zip);
                        tvpParam7.SqlDbType = SqlDbType.Text;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //var s = ex.Message;
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        public bool SaveTruflUserCardData(SaveUserCardDetailsDTO saveUserCardDetails)
        {
            try
            {
                var dtClient = new DataTable();

                

        dtClient.Columns.Add("TruflUserCardDataID", typeof(Int32));
                dtClient.Columns.Add("TruflUserID", typeof(Int32));
                dtClient.Columns.Add("CardType", typeof(string));
                dtClient.Columns.Add("CardNumber", typeof(string));
                dtClient.Columns.Add("Salt", typeof(string));
                dtClient.Columns.Add("Zipcode", typeof(string));
                dtClient.Columns.Add("CreatedDate", typeof(DateTime));
                dtClient.Columns.Add("CreatedBy", typeof(string));

                dtClient.Rows.Add(1,
                                   saveUserCardDetails.TruflUserID,
                                   1,
                                   saveUserCardDetails.CardNo,
                                   1,
                                   saveUserCardDetails.Zip,
                                   DateTime.UtcNow,
                                   12
                                   );

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveTruflUserCardData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserCardDataTY", dtClient);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@TruflUserID", saveUserCardDetails.TruflUserID);
                        tvparam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@CardNo", saveUserCardDetails.CardNo);
                        tvparam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvparam3 = cmd.Parameters.AddWithValue("@LoggedInUser", 12);
                        tvparam3.SqlDbType = SqlDbType.Int;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'spGetEmployeConfigration' will Get Employe Configration details
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns Get EmployeConfigration Details  </returns>
        public DataTable GetEmployeConfiguration(string TruflUserType, int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetEmployeConfigration", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserType", TruflUserType);
                        tvpParam.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam1.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }


        public DataSet GetCustomerRewards(CustomerRewards customerRewards)
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetCustomerRewards", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", customerRewards.TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", customerRewards.RestaurantID);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@QueryType", customerRewards.QueryType);
                        tvpParam2.SqlDbType = SqlDbType.Text;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                        if (customerRewards.QueryType.ToUpper() == "RESTREWARD")
                        dsResponse.Tables[0].TableName = "Rewards";
                        else if (customerRewards.QueryType.ToUpper() == "CREWARDDET")
                            dsResponse.Tables[0].TableName = "AllRestaurantRewards";
                        else if (customerRewards.QueryType.ToUpper() == "RESTREWARD")
                            dsResponse.Tables[0].TableName = "RestaurantRewards";

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dsResponse;
        }

        #endregion

        #region HostessRestaurantEmployeeController

        /// <summary>
        /// This method 'GetRestaurantUserDetails ' returns Restaurant User details
        /// </summary>
        /// <returns>Notifications List</returns>
        public SettingsDTO GetRestaurantUserDetails(int? RestaurantID, int TruflUserID, string UserType)
        {
            SettingsDTO sendResponse = new SettingsDTO();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantUserDetails", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;


                        if (RestaurantID == null)
                        {
                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", DBNull.Value);
                        }
                        else
                        {
                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                            tvpParam.SqlDbType = SqlDbType.Int;
                        }
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@TruflUserID", TruflUserID);
                        tvparam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@UserType", UserType);
                        tvparam2.SqlDbType = SqlDbType.Char;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            sendResponse.UserLoginInformation = ds.Tables[0];
                            sendResponse.UsersInformation = ds.Tables[1];
                            sendResponse.RegisteredRestaurants = ds.Tables[2];
                            sendResponse.RestaurantUserDetailswithHistory = ds.Tables[3];
                            sendResponse.UserProfielFullName = ds.Tables[4];
                            sendResponse.BioData = ds.Tables[5];
                            sendResponse.BookingHistory = ds.Tables[6];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'Update Restaurant Host Status' will Update Restaurant Host Status data.
        /// </summary>
        /// <param name="UpdateRestaurantHostStatus"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool UpdateRestaurantHostStatus(UpdateRestaurantHostStatusDTO UpdateRestaurantHost)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantHostStatus", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserType", UpdateRestaurantHost.TruflUserType);
                        tvpParam.SqlDbType = SqlDbType.Text;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@RestaurantID", UpdateRestaurantHost.RestaurantID);
                        tvparam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@UserID", UpdateRestaurantHost.UserID);
                        tvparam2.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam3 = cmd.Parameters.AddWithValue("@ActiveStatus", UpdateRestaurantHost.ActiveStatus);
                        tvparam3.SqlDbType = SqlDbType.Bit;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'GetRestaurantTables' will Get Restaurant Tables info
        /// </summary>
        /// <param name="spGetRestaurantTables"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public DataTable GetRestaurantTables(int RestaurantID, int UserID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantTables", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@UserID", UserID);
                        tvparam1.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'GetVerifyEmailID' will Get Trufl User Details used to verify the email Id for Duplication
        /// </summary>
        /// <returns></returns>
        public DataTable GetVerifyEmailID()
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetVerifyEmailID", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'spGetEmployeConfigration' will Get Employe Configration details
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns Get EmployeConfigration Details  </returns>
        public bool UpdateRestaurantEmployee(EmployeeConfigDTO employeeConfigDTO)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantEmployee", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", employeeConfigDTO.TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserName", employeeConfigDTO.UserName);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Email", employeeConfigDTO.Email);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@PhoneNumber", employeeConfigDTO.PhoneNumber);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@UserType", employeeConfigDTO.UserType);
                        tvpParam4.SqlDbType = SqlDbType.Text;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// Add New guest directly to the waitlist or as a reservation in a restaurant 
        /// </summary>
        /// <param name="SaveRestaurantGuest">Sends the details of the guest to by saved as a class Object</param>
        /// <returns>Returns 1 on saving the details or 0 on error</returns>
        public bool SaveRestaurantGuest(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantGuest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", SaveRestaurantGuest.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@FullName", SaveRestaurantGuest.FullName);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Email", SaveRestaurantGuest.Email);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@ContactNumber", SaveRestaurantGuest.ContactNumber);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@UserType", SaveRestaurantGuest.UserType);
                        tvpParam4.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@PartySize", SaveRestaurantGuest.PartySize);
                        tvpParam5.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@QuotedTime", SaveRestaurantGuest.QuotedTime);
                        tvpParam6.SqlDbType = SqlDbType.Int;
                        if (SaveRestaurantGuest.BookingStatus == 2)
                        {
                            SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@WaitListTime", DBNull.Value);
                            tvpParam7.SqlDbType = SqlDbType.DateTime;
                            SqlParameter tvpParam14 = cmd.Parameters.AddWithValue("@OfferType", 1);
                            tvpParam14.SqlDbType = SqlDbType.Int;
                        }
                        else
                        {
                            SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@WaitListTime", SaveRestaurantGuest.WaitListTime);
                            tvpParam7.SqlDbType = SqlDbType.DateTime;
                            SqlParameter tvpParam14 = cmd.Parameters.AddWithValue("@OfferType", 2);
                            tvpParam14.SqlDbType = SqlDbType.Int;
                        }
                        SqlParameter tvpParam8 = cmd.Parameters.AddWithValue("@Relationship", SaveRestaurantGuest.Relationship);
                        tvpParam8.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam9 = cmd.Parameters.AddWithValue("@ThisVisit", SaveRestaurantGuest.ThisVisit);
                        tvpParam9.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam10 = cmd.Parameters.AddWithValue("@FoodAndDrink", SaveRestaurantGuest.FoodAndDrink);
                        tvpParam10.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam11 = cmd.Parameters.AddWithValue("@SeatingPreferences", SaveRestaurantGuest.SeatingPreferences);
                        tvpParam11.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam12 = cmd.Parameters.AddWithValue("@Description", SaveRestaurantGuest.Description);
                        tvpParam12.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam13 = cmd.Parameters.AddWithValue("@BookingStatus", SaveRestaurantGuest.BookingStatus);
                        tvpParam13.SqlDbType = SqlDbType.Int;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Updates the details of the Guest/customer who is already in the waitlist
        /// </summary>
        /// <param name="UpdateRestaurantGuest">Input class object with details of the guest to by Updated</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateRestaurantGuest(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantGuest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", UpdateRestaurantGuest.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@FullName", UpdateRestaurantGuest.FullName);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Email", UpdateRestaurantGuest.Email);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@ContactNumber", UpdateRestaurantGuest.ContactNumber);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@TruflUserID", UpdateRestaurantGuest.TruflUserID);
                        tvpParam4.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@Relationship", UpdateRestaurantGuest.Relationship);
                        tvpParam5.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@ThisVisit", UpdateRestaurantGuest.ThisVisit);
                        tvpParam6.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@FoodAndDrink", UpdateRestaurantGuest.FoodAndDrink);
                        tvpParam7.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam8 = cmd.Parameters.AddWithValue("@SeatingPreferences", UpdateRestaurantGuest.SeatingPreferences);
                        tvpParam8.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam9 = cmd.Parameters.AddWithValue("@Description", UpdateRestaurantGuest.Description);
                        tvpParam9.SqlDbType = SqlDbType.Text;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Get the details of the Guest/ customer from the waitlist to be edited
        /// </summary>
        /// <param name="RestaurantID">Restaurant ID of the restaurant Visited by the guest</param>
        /// <param name="UserId">Guest ID</param>
        /// <param name="UserType">User type for the guest is 'TU'</param>
        /// <returns>returns the details of the guest</returns>
        public DataSet GetRestaurantGuest(int RestaurantID, int UserId, string UserType)
        {
            DataSet dssendResponse = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantGuest", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserType", UserType);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@TruflUserID", UserId);
                        tvpParam2.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dssendResponse);
                        }
                        dssendResponse.Tables[0].TableName = "TruflUser";
                        dssendResponse.Tables[1].TableName = "UserBioEvent";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dssendResponse;
        }

        /// <summary>
        /// Add New guest directly to the Seated in a restaurant after the process of Seat A Guest
        /// </summary>
        /// <param name="SaveRestaurantGuest">Sends the details of the guest to by saved as a class Object</param>
        /// <returns>Returns 1 on Saving the details or 0 on error</returns>
        public bool SaveRestaurantGuestImmediately(SaveRestaurantGuestDTO SaveRestaurantGuest)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantGuestImmediately", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", SaveRestaurantGuest.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@FullName", SaveRestaurantGuest.FullName);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Email", SaveRestaurantGuest.Email);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@ContactNumber", SaveRestaurantGuest.ContactNumber);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@UserType", SaveRestaurantGuest.UserType);
                        tvpParam4.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@PartySize", SaveRestaurantGuest.PartySize);
                        tvpParam5.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@TableNumbers", SaveRestaurantGuest.TableNumbers);
                        tvpParam6.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@Relationship", SaveRestaurantGuest.Relationship);
                        tvpParam7.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam8 = cmd.Parameters.AddWithValue("@ThisVisit", SaveRestaurantGuest.ThisVisit);
                        tvpParam8.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam9 = cmd.Parameters.AddWithValue("@FoodAndDrink", SaveRestaurantGuest.FoodAndDrink);
                        tvpParam9.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam10 = cmd.Parameters.AddWithValue("@SeatingPreferences", SaveRestaurantGuest.SeatingPreferences);
                        tvpParam10.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam11 = cmd.Parameters.AddWithValue("@Description", SaveRestaurantGuest.Description);
                        tvpParam11.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam12 = cmd.Parameters.AddWithValue("@OfferType", 1);
                        tvpParam12.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam13 = cmd.Parameters.AddWithValue("@SeatedTableType", SaveRestaurantGuest.SeatedTableType);
                        tvpParam13.SqlDbType = SqlDbType.Text;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Updates the details of the Guest/customer from the waitlist and add it directly into seated after the process of Seat A Guest
        /// </summary>
        /// <param name="UpdateRestaurantGuest">Input class object with details of the guest to by Updated</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateRestaurantGuestImmediately(UpdateRestaurantGuestDTO UpdateRestaurantGuest)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantGuestImmediately", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", UpdateRestaurantGuest.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@FullName", UpdateRestaurantGuest.FullName);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@Email", UpdateRestaurantGuest.Email);
                        tvpParam2.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@ContactNumber", UpdateRestaurantGuest.ContactNumber);
                        tvpParam3.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@TruflUserID", UpdateRestaurantGuest.TruflUserID);
                        tvpParam4.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@Relationship", UpdateRestaurantGuest.Relationship);
                        tvpParam5.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@ThisVisit", UpdateRestaurantGuest.ThisVisit);
                        tvpParam6.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@FoodAndDrink", UpdateRestaurantGuest.FoodAndDrink);
                        tvpParam7.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam8 = cmd.Parameters.AddWithValue("@SeatingPreferences", UpdateRestaurantGuest.SeatingPreferences);
                        tvpParam8.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam9 = cmd.Parameters.AddWithValue("@Description", UpdateRestaurantGuest.Description);
                        tvpParam9.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam10 = cmd.Parameters.AddWithValue("@TableNumbers", UpdateRestaurantGuest.TableNumbers);
                        tvpParam10.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam11 = cmd.Parameters.AddWithValue("@BookingID", UpdateRestaurantGuest.BookingID);
                        tvpParam11.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam12 = cmd.Parameters.AddWithValue("@SeatedTableType", UpdateRestaurantGuest.SeatedTableType);
                        tvpParam12.SqlDbType = SqlDbType.Text;
                       
                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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

        #endregion

        #region HostessWaitListUserController
        
        #region WaitList

        /// <summary>
        /// This method 'RetrieveUser ' returns User details
        /// </summary>
        /// <returns>User List</returns>
        public List<UserProfile> RetrieveUser()
        {
            List<UserProfile> sourceapilist = new List<UserProfile>();
            try
            {

                con.Open();
                using (SqlCommand command1 = new SqlCommand("spGetTruflUser", con))
                {
                    command1.CommandTimeout = TruflConstants.DBResponseTime;

                    SqlDataAdapter da = new SqlDataAdapter();
                    // command1.Parameters.AddWithValue("@SourceAPIName", "clever");
                    // command1.Parameters.AddWithValue("@IsWinService", false);

                    da.SelectCommand = command1;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UserProfile userprofile = new UserProfile();
                        userprofile.RestaurantID = ds.Tables[0].Rows[i]["RestaurantID"].ToString();
                        userprofile.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        userprofile.PartySize = ds.Tables[0].Rows[i]["PartySize"].ToString();
                        userprofile.Quoted = ds.Tables[0].Rows[i]["Quoted"].ToString();
                        userprofile.Waited = ds.Tables[0].Rows[i]["Waited"].ToString();
                        userprofile.OfferAmount = ds.Tables[0].Rows[i]["OfferAmount"].ToString();
                        userprofile.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        userprofile.pic = ds.Tables[0].Rows[i]["pic"].ToString();
                        userprofile.Contact1 = ds.Tables[0].Rows[i]["Contact1"].ToString();
                        userprofile.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        userprofile.DOB = ds.Tables[0].Rows[i]["DOB"].ToString();
                        userprofile.ActiveInd = ds.Tables[0].Rows[i]["ActiveInd"].ToString();
                        userprofile.ResauranEmpInd = Convert.ToInt32(ds.Tables[0].Rows[i]["RestaurantEmpInd"].ToString());
                        userprofile.TruffMemberType = Convert.ToInt32(ds.Tables[0].Rows[i]["TruflMemberType"].ToString());
                        userprofile.TruflRelationship = Convert.ToInt32(ds.Tables[0].Rows[i]["TruflRelationship"].ToString());
                        userprofile.TruflshareCode = ds.Tables[0].Rows[i]["TruflshareCode"].ToString();
                        userprofile.ReferTruflUserID = Convert.ToInt32(ds.Tables[0].Rows[i]["ReferTruflUserID"].ToString());
                        userprofile.ModifiedDate = ds.Tables[0].Rows[i]["ModifiedDate"].ToString();
                        userprofile.ModifiedBy = Convert.ToInt32(ds.Tables[0].Rows[i]["ModifiedBy"].ToString());


                        sourceapilist.Add(userprofile);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return sourceapilist;
        }
       
        /// <summary>
        /// Get the details of the waitlist Guests / Customers
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public DataTable GetWaitListUsers(int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spGetWaitListUsers", sqlcon))
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
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// Reset the waitlisttime to synchronize with the current System Time
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public bool ResetWaitList(int RestaurantID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spResetWaitList", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'UpdateAcceptOffer' will update the waited user info
        /// </summary>
        /// <param name="UpdateAcceptOffer"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool UpdateAcceptOffer(int BookingID, int BookingStatus)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateBookingStatus", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@BookingStatus", BookingStatus);
                        tvparam1.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'UpdateWaitListAcceptNotify' will update the waited user info
        /// </summary>
        /// <param name="UpdateWaitListAcceptNotify"></param>
        /// <returns>Returns DataTable</returns>
        public DataTable UpdateWaitListAcceptNotify(int RestaurantID, int BookingID, string UpdateType)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateWaitListAcceptNotify", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@BookingID", BookingID);
                        tvparam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@UpdateType", UpdateType);
                        tvparam2.SqlDbType = SqlDbType.Text;

                        //int status = cmd.ExecuteNonQuery();
                        //if (status == 0)
                        //{
                        //    return false;
                        //}
                        //else
                        //{
                        //    return true;
                        //}

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method 'SaveWaitedlistBooking' will save all the waited list users
        /// </summary>
        /// <param name=" data"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool SaveWaitedlistBooking(BookingTableDTO bookingTableInput)
        {
            try
            {
                var dtClient = new DataTable();

                dtClient.Columns.Add("BookingID", typeof(Int32));
                dtClient.Columns.Add("TruflUserID", typeof(Int32));
                dtClient.Columns.Add("RestaurantID", typeof(Int32));
                dtClient.Columns.Add("PartySize", typeof(Int32));
                dtClient.Columns.Add("OfferType", typeof(Int32));
                dtClient.Columns.Add("OfferAmount", typeof(Int32));
                dtClient.Columns.Add("Quoted", typeof(Int32));
                dtClient.Columns.Add("BookingStatus", typeof(Int32));
                dtClient.Columns.Add("TruflUserCardDataID", typeof(Int32));
                dtClient.Columns.Add("TruflTCID", typeof(Int32));

                dtClient.Rows.Add(bookingTableInput.BookingID,
                                   bookingTableInput.TruflUserID,
                                   bookingTableInput.RestaurantID,
                                   bookingTableInput.PartySize,
                                   bookingTableInput.OfferType,
                                   bookingTableInput.OfferAmount,
                                   bookingTableInput.Quoted,
                                   bookingTableInput.BookingStatus,
                                   bookingTableInput.TruflUserCardDataID,
                                   bookingTableInput.TruflTCID
                                   );

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveBooking", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingTY", dtClient);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@LoggedInUser", bookingTableInput.LoggedInUser);
                        tvparam1.SqlDbType = SqlDbType.Int;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// This method 'GetRestaurantTableAmount' will Get Restaurant Tables amount info
        /// </summary>
        /// <param name="spGetRestaurantTables"></param>
        /// <returns>Returns amount</returns>
        public DataTable GetRestaurantTableAmount(int RestaurantID, int TableNumber)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantTableAmount", con))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@TableNumber", TableNumber);
                        tvparam1.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(sendResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }
        
        /// <summary>
        /// This method 'Update Booking' will Update Booking data.
        /// </summary>
        /// <param name="SaveSignUpUserInfo"></param>
        /// <returns>Returns 1 if Success, 0 for failure</returns>
        public bool UpdateBooking(UpdateBookingTableNumberDTO updateBookingTableNumber)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateBooking", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", updateBookingTableNumber.BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam1 = cmd.Parameters.AddWithValue("@UserID", updateBookingTableNumber.UserID);
                        tvparam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam2 = cmd.Parameters.AddWithValue("@RestaurantID", updateBookingTableNumber.RestaurantID);
                        tvparam2.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam3 = cmd.Parameters.AddWithValue("@BStatus", updateBookingTableNumber.BStatus);
                        tvparam3.SqlDbType = SqlDbType.Int;
                        SqlParameter tvparam4 = cmd.Parameters.AddWithValue("@TableNumbers", updateBookingTableNumber.TableNumbers);
                        tvparam4.SqlDbType = SqlDbType.Text;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Get Details of the Available Tables with details like as Server assigned to the table, Table Type etc
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>Returns the details of the available tables</returns>
        public DataTable GetSeatAGuest(int RestaurantID)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetTablewiseSnapshot", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Status", "SAG");
                        tvpParam1.SqlDbType = SqlDbType.VarChar;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtsendResponse);
                        }
                        dtsendResponse.TableName = "SeatAGuest";
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

        /// <summary>
        /// get details of GetSeatedNow
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns></returns>
        public DataSet GetRestaurantGetSeatedNow(int RestaurantID)
        {
            DataSet dssendResponse = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantGetSeatedNow", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dssendResponse);
                        }
                        dssendResponse.Tables[0].TableName = "TableType";
                        dssendResponse.Tables[1].TableName = "GetSeatedNow";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dssendResponse;
        }

        /// <summary>
        /// saves or updates the details of GetSeatedNow
        /// </summary>
        /// <param name="saveGetSeatedNow"></param>
        /// <returns>returns 1 on Saving/updating the GetSeatedNow Details, 0 on error</returns>
        public bool SaveRestaurantGetSeatedNow(SaveGetSeatedNow saveGetSeatedNow)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantGetSeatedNow", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", saveGetSeatedNow.RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@TableType", saveGetSeatedNow.TableType);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@NoOfTables", saveGetSeatedNow.NoOfTables);
                        tvpParam2.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@Amount", saveGetSeatedNow.Amount);
                        tvpParam3.SqlDbType = SqlDbType.Float;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Updates the customer status to seated after the process of Seat A Guest
        /// </summary>
        /// <param name="BookingID">Booking ID as Input</param>
        /// <param name="TableNumbers">TableNumbers as Input</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateWaitListSeated(SeatAGuest seatAGuest)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateWaitListSeated", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@BookingID", seatAGuest.BookingID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@TableNumbers", seatAGuest.TableNumbers);
                        tvpParam1.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@SeatedTableType", seatAGuest.SeatedTableType);
                        tvpParam2.SqlDbType = SqlDbType.Text;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Sends the Push Notification message to Request the guest to the process of Seat A Guest
        /// </summary>
        /// <param name="UserId">User ID as Input</param>
        /// <returns>returns the details of the guest who was sent the Notification</returns>
        public DataTable SendPushNotification(int UserId)
        {
            string DeviceID;
            DataTable dt = new DataTable();
            string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
            //string Serverkey = ConfigurationManager.AppSettings["PushNotificationServerKey"];

            PushNotificationService pushNotificationService = new PushNotificationService();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spNotificationMessage", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID ", UserId);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DeviceID = dt.Rows[0]["DeviceID"].ToString();
                                string Message = "It's your turn to be seated! Please see our hosts right away. They will show you to your table. Let's do this!";
                                if (DeviceID.ToString() != "")
                                    pushNotificationService.SendNotificationFromFirebaseCloud(DeviceID, Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// Sends the Push Notification message to Request the guest to the process of Seat A Guest
        /// </summary>
        /// <param name="pushNotification">push Notification Object with Customer ID & Message</param>
        /// <returns>Returns the Output result of the push notification sent</returns>
        public string SendPushNotification(PushNotification pushNotification)
        {
            string DeviceID;
            DataTable dt = new DataTable();
            string result = "";
            string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
            //string Serverkey = ConfigurationManager.AppSettings["PushNotificationServerKey"];

            PushNotificationService pushNotificationService = new PushNotificationService();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spNotificationMessage", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID ", pushNotification.TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DeviceID = dt.Rows[0]["DeviceID"].ToString();
                                if (DeviceID.ToString() == "")
                                    result = "";
                                else
                                    result = pushNotificationService.SendNotificationFromFirebaseCloud(DeviceID, pushNotification.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return result;
        }

        #endregion

        #region StartService

        /// This method GetRestaurantOpenSections ' returns RestaurantOpenSections details
        /// </summary>
        /// <param name="RestaurantID"> takes Restaurant ID as input</param>
        /// <returns>returns Details of the all Restaurant Sections</returns>
        public DataTable GetRestaurantOpenSections(int RestaurantID)
        {
            DataTable sendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantOpenSections", sqlcon))
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
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return sendResponse;
        }

        /// <summary>
        /// This method UpdateRestaurantActiveSections updates RestaurantOpenSection isActive and isDeleted fields 
        /// and sets the isActive fields of open floors to true and closed to false
        /// </summary>
        /// <param name="restaurantActiveSections">RestaurantActiveSectionsDTO restaurantActiveSections</param>
        /// <returns>Updates Details of the all Restaurant Sections which are Open & Closed</returns>
        public bool UpdateRestaurantActiveSections(List<RestaurantActiveSectionsDTO> restaurantActiveSections)
        {
            try
            {
                var dtrestActiveSect = new DataTable();
                dtrestActiveSect.Columns.Add("RestaurantID", typeof(Int32));
                dtrestActiveSect.Columns.Add("FloorNumber", typeof(Int32));
                dtrestActiveSect.Columns.Add("IsActive", typeof(Boolean));
                dtrestActiveSect.Columns.Add("IsDelete", typeof(Boolean));

                for (int i = 0; restaurantActiveSections.Count > i; i++)
                {
                    dtrestActiveSect.Rows.Add(restaurantActiveSections[i].RestaurantID,
                                       restaurantActiveSections[i].FloorNumber,
                                       restaurantActiveSections[i].IsActive,
                                       restaurantActiveSections[i].IsDelete);
                }


                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantActiveSections", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantActiveSectionsTY", dtrestActiveSect);
                        tvpParam.SqlDbType = SqlDbType.Structured;

                        SqlParameter pvNewId = new SqlParameter();
                        pvNewId.ParameterName = "@RetVal";
                        pvNewId.DbType = DbType.Int32;
                        pvNewId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvNewId);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// This method UpdateRestaurantActiveSections updates RestaurantOpenSection isActive and isDeleted fields 
        /// and sets the isActive fields of open floors to true and closed to false
        /// </summary>
        /// <param name="restaurantActiveSections">RestaurantActiveSectionsDTO restaurantActiveSections</param>
        /// <returns></returns>
        public bool SaveRestaurantOpenSectionStaff(List<RestaurantSectionStaffDTO> restaurantSectionStaff)
        {
            try
            {
                var dtrestSectStaff = new DataTable();
                dtrestSectStaff.Columns.Add("RestaurantID", typeof(Int32));
                dtrestSectStaff.Columns.Add("TruflUserID", typeof(Int32));
                dtrestSectStaff.Columns.Add("RestaurantFloorNumber", typeof(Int32));


                for (int i = 0; restaurantSectionStaff.Count > i; i++)
                {
                    dtrestSectStaff.Rows.Add(restaurantSectionStaff[i].RestaurantID,
                                       restaurantSectionStaff[i].TruflUserID,
                                       restaurantSectionStaff[i].RestaurantFloorNumber);
                }


                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantOpenSectionStaff", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantSectionStaffTY", dtrestSectStaff);
                        tvpParam.SqlDbType = SqlDbType.Structured;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// This Method GetRestaurantWaitTimeOpenSectionStaff is used to get the details to be displayed in review selection screen
        /// </summary>
        /// <param name="RestaurantID"> takes Restaurant ID as input</param>
        /// <returns>Get three datatable for Restaurant Open Time, Selected Staff & Closed Sections</returns>
        public DataSet GetRestaurantWaitTimeOpenSectionStaff(int RestaurantID)
        {
            DataTable dtTableRange = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsSendResponse = new DataSet();

            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantWaitTimeOpenSectionStaff", sqlcon))
                    {
                        cmd.CommandTimeout = TruflConstants.DBResponseTime;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                        dtTableRange = ds.Tables[1].Clone();

                        int RestID, UserId, NextUserId, StartTno, EndTno;
                        string strUserId = "";

                        RestID = Convert.ToInt16(ds.Tables[0].Rows[0]["RestaurantID"]);
                        UserId = Convert.ToInt16(ds.Tables[0].Rows[0]["HostessID"]);
                        StartTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);
                        EndTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            NextUserId = Convert.ToInt16(ds.Tables[0].Rows[i]["HostessID"]);

                            if (UserId == NextUserId)
                            {
                                EndTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                            }
                            else
                            {
                                DataRow[] findRows = ds.Tables[1].Select("TruflUserID = " + UserId);

                                if (StartTno > EndTno)
                                    EndTno = StartTno;

                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["TruflUserID"] = UserId;
                                    drNew["FullName"] = findRows[0]["FullName"];
                                    drNew["pic"] = findRows[0]["pic"];
                                    drNew["ActiveInd"] = findRows[0]["ActiveInd"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;

                                    dtTableRange.Rows.Add(drNew);
                                }
                                StartTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                                strUserId += UserId + ", ";
                                UserId = NextUserId;
                            }
                            if (i == ds.Tables[0].Rows.Count - 1)
                            {

                                DataRow[] findRows = ds.Tables[1].Select("TruflUserID = " + UserId);
                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["TruflUserID"] = UserId;
                                    drNew["FullName"] = findRows[0]["FullName"];
                                    drNew["pic"] = findRows[0]["pic"];
                                    drNew["ActiveInd"] = findRows[0]["ActiveInd"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;
                                    dtTableRange.Rows.Add(drNew);
                                    strUserId += UserId + ", ";
                                }
                            }
                        }

                        dsSendResponse.Tables.Add(ds.Tables[2].Copy());
                        dsSendResponse.Tables.Add(dtTableRange);
                        dsSendResponse.Tables.Add(ds.Tables[3].Copy());

                        dsSendResponse.Tables[0].TableName = "RestaurantWaitListOpen";
                        dsSendResponse.Tables[1].TableName = "RestaurantOpenSectionStaff";
                        dsSendResponse.Tables[2].TableName = "RestaurantOpenSection";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dsSendResponse;
        }

        /// <summary>
        /// This Method SaveRestaurantOpenTime is used to save the Restaurant Opening Time into the DB
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="Time"></param>
        /// <returns>Saves the Restaurant Open Time for the start Service</returns>
        public bool SaveRestaurantOpenTime(int RestaurantID, string Time)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveRestaurantOpenTime", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@OpenTime", Time);
                        tvpParam1.SqlDbType = SqlDbType.Text;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
                //return false;
            }
        }

        /// <summary>
        /// Not In Use
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public DataTable GetRestaurantHostessOpenSectionDetails(int RestaurantID, string UserType)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantHostesOpenSectionDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@UserType", UserType);
                        tvpParam1.SqlDbType = SqlDbType.Text;

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

         /// <summary>
        /// Get the details of the Staff of a restaurant for selection in start service process
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>returns all the servers with no table assigned</returns>
        public DataSet GetRestaurantSelectStaff(int RestaurantID)
        {
            DataSet dsSendResponse = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantSelectStaff", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsSendResponse);
                        }
                    }
                }
                dsSendResponse.Tables[0].TableName = "TableRange";
                dsSendResponse.Tables[1].TableName = "SelectStaff";
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dsSendResponse;
        }
      
        /// <summary>
        /// Get all the Sections/ Floor in a restaurant
        /// </summary>
        /// <param name="RestaurantID">Restaurant ID as Input</param>
        /// <returns>Get the details of all the sections and the range of the tables allocated to it</returns>
        public DataSet GetRestaurantSectionTables(int RestaurantID)
        {
            DataTable dtTableRange = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsSendResponse = new DataSet();

            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantSectionTables", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                        dtTableRange = ds.Tables[1].Clone();

                        int RestID, FloorNo, NextFloorNo, StartTno, EndTno;
                        string strFloorNo = "";

                        RestID = Convert.ToInt16(ds.Tables[0].Rows[0]["RestaurantID"]);
                        FloorNo = Convert.ToInt16(ds.Tables[0].Rows[0]["FloorNumber"]);
                        StartTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);
                        EndTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            NextFloorNo = Convert.ToInt16(ds.Tables[0].Rows[i]["FloorNumber"]);

                            if (FloorNo == NextFloorNo)
                            {
                                EndTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                            }
                            else
                            {
                                DataRow[] findRows = ds.Tables[1].Select("FloorNumber = " + FloorNo);
                                if (StartTno > EndTno)
                                    EndTno = StartTno;

                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["FloorNumber"] = FloorNo;
                                    drNew["FloorName"] = findRows[0]["FloorName"];
                                    drNew["FloorImage"] = findRows[0]["FloorImage"];
                                    drNew["IsActive"] = findRows[0]["IsActive"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;

                                    dtTableRange.Rows.Add(drNew);
                                }
                                StartTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                                strFloorNo += FloorNo + ", ";
                                FloorNo = NextFloorNo;
                            }
                            if (i == ds.Tables[0].Rows.Count - 1)
                            {
                                DataRow[] findRows = ds.Tables[1].Select("FloorNumber = " + FloorNo);

                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["FloorNumber"] = FloorNo;
                                    drNew["FloorName"] = findRows[0]["FloorName"];
                                    drNew["FloorImage"] = findRows[0]["FloorImage"];
                                    drNew["IsActive"] = findRows[0]["IsActive"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;
                                    dtTableRange.Rows.Add(drNew);
                                }
                                strFloorNo += FloorNo + ", ";
                            }
                        }

                        DataRow[] findRemRows;
                        if (strFloorNo.Length > 2)
                        {
                            strFloorNo = strFloorNo.Substring(0, strFloorNo.Length - 2);
                            findRemRows = ds.Tables[1].Select("FloorNumber NOT IN (" + strFloorNo + ")");
                        }
                        else
                        {
                            findRemRows = ds.Tables[1].Select("FloorNumber <> 0");
                        }

                        foreach (DataRow dr in findRemRows)
                        {
                            dtTableRange.Rows.Add(dr.ItemArray);
                        }

                        dsSendResponse.Tables.Add(ds.Tables[2].Copy());
                        dtTableRange.DefaultView.Sort = "FloorNumber";
                        // Store in new Dataset
                        dsSendResponse.Tables.Add(dtTableRange.DefaultView.ToTable());

                        dsSendResponse.Tables[0].TableName = "TableRange";
                        dsSendResponse.Tables[1].TableName = "DefineSection";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dsSendResponse;
        }

        /// <summary>
        /// Assigns the provided colors to the servers to be used  t0 show the  in Seat A Guest & Snapshot Table
        /// </summary>
        /// <param name="strColors">set of colors as a string</param>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>returns the datatable object with the colors assigned to the servers</returns>
        public DataTable AssignColorsToServer(string strColors, int RestaurantID)
        {
            int b, c;
            int UserID;
            string Scolor = "";
            DataTable dt = new DataTable();

            var dtClient = new DataTable();
            dtClient.Columns.Add("UserID", typeof(string));
            dtClient.Columns.Add("backgroundcolor", typeof(string));
            dtClient.Columns.Add("border", typeof(string));
            dtClient.Columns.Add("borderradius", typeof(string));
            try
            {
                List<string> colors = strColors.Split(',').ToList<string>();
                dt = GetRestaurantStaffTables(RestaurantID).Tables[1];
                b = 0;
                c = colors.Count;
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    UserID = Convert.ToInt16(dt.Rows[i]["TruflUserID"]);
                    Scolor = colors[b];
                    dtClient.Rows.Add(UserID.ToString(), "#" + Scolor, "1px solid #" + Scolor, "20px");
                    b += 1;
                    if (b >= c)
                        b = 0;
                }
                dtClient.Rows.Add("true", "#" + Scolor, "1px solid #" + Scolor, "20px");

                return dtClient;
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
        }
       
        /// <summary>
        /// Updates the Restaurant OpenDate to current date after the complition of start service process
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateRestaurantOpenDate(int RestaurantID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantOpenDate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Reset the start Service used for demo purpose
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public bool ResetRestaurantOpenDate(int RestaurantID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spResetRestaurantOpenDate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// checks whether the start service process is already completed for the day
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>returns 1 if start service process is completed else 0</returns>
        public int GetRestaurantOpenDate(int RestaurantID)
        {
            DataTable dtRes = new DataTable();
            int iResult = 0;
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantOpenDate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        SqlParameter RetVal = new SqlParameter();
                        RetVal.ParameterName = "@RetVal";
                        RetVal.DbType = DbType.Int32;
                        RetVal.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(RetVal);

                        int status = cmd.ExecuteNonQuery();
                        //if (status == 0)
                        //{
                        //    return 0;
                        //}
                        //else
                        //{
                        //    return RetVal;
                        //}
                        iResult = Convert.ToInt16(cmd.Parameters["@RetVal"].Value);
                        return iResult;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
        }

        #endregion

        #region Snapshot Settings
     
        /// <summary>
        /// Get the details of the Staff of a restaurant
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>returns all the servers along with the servers assigned the tables</returns>
        public DataSet GetRestaurantStaffTables(int RestaurantID)
        {
            DataTable dtTableRange = new DataTable();
            DataSet ds = new DataSet();
            DataSet dsSendResponse = new DataSet();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantStaffTables", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }
                        dtTableRange = ds.Tables[1].Clone();

                        int RestID, UserId, NextUserId, StartTno, EndTno;
                        string strUserId = "";

                        RestID = Convert.ToInt16(ds.Tables[0].Rows[0]["RestaurantID"]);
                        UserId = Convert.ToInt16(ds.Tables[0].Rows[0]["HostessID"]);
                        StartTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);
                        EndTno = Convert.ToInt16(ds.Tables[0].Rows[0]["TableNumber"]);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            NextUserId = Convert.ToInt16(ds.Tables[0].Rows[i]["HostessID"]);

                            if (UserId == NextUserId)
                            {
                                EndTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                            }
                            else
                            {
                                DataRow[] findRows = ds.Tables[1].Select("TruflUserID = " + UserId);

                                if (StartTno > EndTno)
                                    EndTno = StartTno;

                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["TruflUserID"] = UserId;
                                    drNew["FullName"] = findRows[0]["FullName"];
                                    drNew["pic"] = findRows[0]["pic"];
                                    drNew["ActiveInd"] = findRows[0]["ActiveInd"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;

                                    dtTableRange.Rows.Add(drNew);
                                }
                                StartTno = Convert.ToInt16(ds.Tables[0].Rows[i]["TableNumber"]);
                                strUserId += UserId + ", ";
                                UserId = NextUserId;
                            }
                            if (i == ds.Tables[0].Rows.Count - 1)
                            {

                                DataRow[] findRows = ds.Tables[1].Select("TruflUserID = " + UserId);
                                if (findRows.Count() > 0)
                                {
                                    DataRow drNew = dtTableRange.NewRow();
                                    drNew["RestaurantID"] = RestID;
                                    drNew["TruflUserID"] = UserId;
                                    drNew["FullName"] = findRows[0]["FullName"];
                                    drNew["pic"] = findRows[0]["pic"];
                                    drNew["ActiveInd"] = findRows[0]["ActiveInd"];
                                    drNew["StartTableNumber"] = StartTno;
                                    drNew["EndTableNumber"] = EndTno;
                                    dtTableRange.Rows.Add(drNew);
                                }
                                strUserId += UserId + ", ";
                            }
                        }

                        DataRow[] findRemRows;
                        if (strUserId.Length > 2)
                        {
                            strUserId = strUserId.Substring(0, strUserId.Length - 2);
                            findRemRows = ds.Tables[1].Select("TruflUserID NOT IN (" + strUserId + ")");
                        }
                        else
                        {
                            findRemRows = ds.Tables[1].Select("TruflUserID <> 0");

                        }

                        foreach (DataRow dr in findRemRows)
                        {
                            dr["ActiveInd"] = 0;
                            dtTableRange.Rows.Add(dr.ItemArray);
                        }

                        dsSendResponse.Tables.Add(ds.Tables[2].Copy());
                        dsSendResponse.Tables.Add(dtTableRange);
                        dsSendResponse.Tables[0].TableName = "TableRange";
                        dsSendResponse.Tables[1].TableName = "ManageServer";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.WriteToErrorLogFile(ex);
                throw ex;
            }
            return dsSendResponse;
        }

        /// <summary>
        /// Get Serverwise Details of the Tables Available and Seated
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>Returns the details of the tables grouped by server</returns>
        public DataTable GetServerwiseSnapshot(int RestaurantID)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetServerwiseSnapshot", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtsendResponse);
                        }
                        dtsendResponse.TableName = "Serverwise";
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

        /// <summary>
        /// Get Capacitywise(2-Top, 4-Top etc) Details of the Tables Available and Seated
        /// </summary>
        /// <param name="RestaurantID">RestaurantID as Input</param>
        /// <returns>Returns the details of the tables grouped by Capacity</returns>
        public DataTable GetCapacitywiseSnapshot(int RestaurantID)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetCapacitywiseSnapshot", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtsendResponse);
                        }
                        dtsendResponse.TableName = "Capacitywise";
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

        /// <summary>
        /// Get Details of the Tables with details like Server assigned to the table, Table Type, Is it seated etc
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <returns>Returns the details of all the tables</returns>
        public DataTable GetTablewiseSnapshot(int RestaurantID)
        {
            DataTable dtsendResponse = new DataTable();
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetTablewiseSnapshot", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@Status", "ALL");
                        tvpParam1.SqlDbType = SqlDbType.VarChar;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtsendResponse);
                        }
                        dtsendResponse.TableName = "Tablewise";
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

        /// <summary>
        /// Assigns the Tables from the clockout server to a new server
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <param name="CurrentUserID">Current UserID as Input</param>
        /// <param name="NewUserID">New UserID as Input</param>
        /// <returns>returns 1 on updating the record, 0 on error</returns>
        public bool UpdateServerClockOut(int RestaurantID, int CurrentUserID, int NewUserID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateServerClockOut", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@CurrentUserID", CurrentUserID);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@NewUserID", NewUserID);
                        tvpParam2.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Switch a server from a table by allocating the selected table to another server
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <param name="TableNumber">TableNumber as Input</param>
        /// <param name="NewUserID">New UserID as Input</param>
        /// <returns>returns 1 on updating the server ID, 0 on error</returns>
        public bool UpdateSwitchServer(int RestaurantID, int TableNumber, int NewUserID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateSwitchServer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@TableNumber", TableNumber);
                        tvpParam1.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@NewUserID", NewUserID);
                        tvpParam2.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Saves the Tables assigned to the section from DefineSections
        /// </summary>
        /// <param name="restaurantDefineSections">List of Sections/ floors with the tables assigned to it.</param>
        /// <returns>Return 1 on saving the details, 0 on error</returns>
        public bool SaveDefineSections(List<RestaurantDefineSections> restaurantDefineSections)
        {
            try
            {
                var dtRestDefineSections = new DataTable();
                dtRestDefineSections.Columns.Add("RestaurantID", typeof(Int32));
                dtRestDefineSections.Columns.Add("FloorNumber", typeof(Int32));
                dtRestDefineSections.Columns.Add("StartTableNumber", typeof(Int32));
                dtRestDefineSections.Columns.Add("EndTableNumber", typeof(Int32));

                for (int i = 0; restaurantDefineSections.Count > i; i++)
                {
                    dtRestDefineSections.Rows.Add(restaurantDefineSections[i].RestaurantID,
                                       restaurantDefineSections[i].FloorNumber,
                                       restaurantDefineSections[i].StartTableNumber,
                                       restaurantDefineSections[i].EndTableNumber);
                }

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveDefineSections", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantDefineSectionsTY", dtRestDefineSections);
                        tvpParam.SqlDbType = SqlDbType.Structured;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Saves the Tables assigned to the Servers from DefineSections
        /// </summary>
        /// <param name="restaurantManageServer">List of Servers with the tables assigned to it.</param>
        /// <returns>Return 1 on saving the details, 0 on error</returns>
        public bool SaveManageServer(List<RestaurantManageServer> restaurantManageServer)
        {
            try
            {
                var dtRestManageServer = new DataTable();
                dtRestManageServer.Columns.Add("RestaurantID", typeof(Int32));
                dtRestManageServer.Columns.Add("HostessID", typeof(Int32));
                dtRestManageServer.Columns.Add("StartTableNumber", typeof(Int32));
                dtRestManageServer.Columns.Add("EndTableNumber", typeof(Int32));

                for (int i = 0; restaurantManageServer.Count > i; i++)
                {
                    dtRestManageServer.Rows.Add(restaurantManageServer[i].RestaurantID,
                                       restaurantManageServer[i].HostessID,
                                       restaurantManageServer[i].StartTableNumber,
                                       restaurantManageServer[i].EndTableNumber);
                }

                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spSaveManageServers", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantManageServersTY", dtRestManageServer);
                        tvpParam.SqlDbType = SqlDbType.Structured;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Update the Table status of Empty & Check drop by the selection of pop up menu on the table from Snapshot - Table directly
        /// it Updates the Empty and Check Drop one at a time and depends on the value of UpdateType.
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <param name="TableNumber">TableNumber as Input</param>
        /// <param name="UpdateType">UpdateType as string Input for empty table as "EMPTY" and Check Drop as "CHECK"</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateSnapshotTableEmptyAndCheck(int RestaurantID, int TableNumber, string UpdateType)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateSnapshotTableEmptyAndCheck", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@TableNumber", TableNumber);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@UpdateType", UpdateType);
                        tvpParam2.SqlDbType = SqlDbType.Text;

                        SqlParameter pvRetVal = new SqlParameter();
                        pvRetVal.ParameterName = "@RetVal";
                        pvRetVal.DbType = DbType.Int32;
                        pvRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(pvRetVal);

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Update the status of the Restaurant section to open as 1 and close as 0
        /// </summary>
        /// <param name="RestaurantID"> RestaurantID as Input</param>
        /// <param name="FloorNumber">FloorNumber as Input</param>
        /// <param name="ActiveStatus">ActiveStatus as Input</param>
        /// <returns>Returns 1 on updating the details or 0 on error</returns>
        public bool UpdateRestaurantSectionOpenClose(int RestaurantID, int FloorNumber, int ActiveStatus)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spUpdateRestaurantSectionOpenClose", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@FloorNumber", FloorNumber);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@ActiveStatus", ActiveStatus);
                        tvpParam2.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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
        /// Verify the records in Snapshot For
        /// 1. Sections are Open.
        /// 2. Server are assigned Tables.
        /// 3. Servers are assigned to Open Sections.
        /// 4. Network Error.
        /// </summary>
        /// <param name="RestaurantID"></param>
        /// <returns></returns>
        public bool GetVerifySnapShot(int RestaurantID)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings["TraflConnection"];
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetVerifySnapShot", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam.SqlDbType = SqlDbType.Int;

                        int status = cmd.ExecuteNonQuery();
                        if (status == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
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

        #endregion

        public DataSet GetRestaurantRewards(int TruflUserID, int RestaurantID)
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spGetRestaurantRewards", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", TruflUserID);
                        tvpParam.SqlDbType = SqlDbType.Int;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam1.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResponse;
        }


        public bool SaveRestaurantRewards(RestaurantRewards restaurantRewards)
        {
            try
            {
                int TotalRewardsPoints = 0, RewardsCalc = 0, RestMembership = 0;
                int TotalTruflRewardsPoints = 0, TruflRewardsCalc = 0, TruflMembership = 0, TruflBonusPer=0, TruflCashBack = 0;
                int RewardTypeID = 0;
                bool isWinBid = false;
                string OperationType, TruflOperationType;

                DataSet dsUserRewards = GetRestaurantRewards(restaurantRewards.TruflUserID, restaurantRewards.RestaurantID);

                if (dsUserRewards.Tables[0].Rows.Count >= 1)
                {
                    OperationType = "UPDATE";
                    RestMembership = Convert.ToInt16(dsUserRewards.Tables[0].Rows[0]["MembershipTypeID"]);
                    TotalRewardsPoints = Convert.ToInt16(dsUserRewards.Tables[0].Rows[0]["RewardPoints"]);
                    isWinBid = Convert.ToBoolean(dsUserRewards.Tables[0].Rows[0]["IsWinBid"]);
                }
                else
                    OperationType = "INSERT";

                if (dsUserRewards.Tables[1].Rows.Count >= 1)
                {
                    TruflOperationType = "UPDATE";
                    TruflMembership = Convert.ToInt16(dsUserRewards.Tables[1].Rows[0]["MembershipTypeID"]);
                    TotalTruflRewardsPoints = Convert.ToInt32(dsUserRewards.Tables[1].Rows[0]["RewardPoints"]);
                }
                else
                    TruflOperationType = "INSERT";

                switch (restaurantRewards.RewardType.ToUpper())
                {
                    case "AUCTION":
                        RewardTypeID = 1;
                        RewardsCalc = 25;
                        break;
                    case "WIN_AUCTION":
                        if (!isWinBid)
                        {
                            RewardTypeID = 2;
                            RewardsCalc = 100;
                            isWinBid = true;
                        }
                        break;

                    case "SEATED":
                        RewardTypeID = 3;
                        RewardsCalc = 25;
                        break;
                    case "BILL_AMOUNT":
                        RewardTypeID = 4;
                        if (restaurantRewards.BillAmount < 100)
                            RewardsCalc = 0;
                        else if (restaurantRewards.BillAmount < 250)
                            RewardsCalc = 100;
                        else if (restaurantRewards.BillAmount < 500)
                            RewardsCalc = 500;
                        else if (restaurantRewards.BillAmount < 1000)
                            RewardsCalc = 1500;
                        else
                            RewardsCalc = 4000;
                        break;
                    case "INVITE":
                        RewardTypeID = 5;
                        RewardsCalc = 500;
                        break;
                    case "GIFT":
                        RewardTypeID = 6;
                        RewardsCalc = 5000;
                        break;
                }
                TotalRewardsPoints += RewardsCalc;

                DataView dv = new DataView(dsUserRewards.Tables[2]);
                dv.RowFilter = "MembershipCode = 'R'";

                foreach(DataRowView drv in dv)
                    if (TotalTruflRewardsPoints >= Convert.ToInt16(drv["Points"]))
                    {
                        RestMembership = Convert.ToInt16(drv["MembershipTypeID"]);
                    }

                dv.RowFilter = "MembershipCode = 'T'";

                foreach (DataRowView drv in dv)
                    if (TotalTruflRewardsPoints >= Convert.ToInt32(drv["Points"]))
                    {
                        TruflMembership = Convert.ToInt32(drv["MembershipTypeID"]);
                        TruflBonusPer = Convert.ToInt32(drv["BonusPointsPer"]);
                        if (RewardTypeID == 4)
                            TruflCashBack = Convert.ToInt32(drv["TruflDiscount"]);
                    }
                TruflRewardsCalc = RewardsCalc + Convert.ToInt32((RewardsCalc * TruflBonusPer) / 100);
                TotalTruflRewardsPoints += TruflRewardsCalc;
                int status = 1;
                if (RewardTypeID > 0)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("spSaveRestaurantRewards", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@TruflUserID", restaurantRewards.TruflUserID);
                            tvpParam.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", restaurantRewards.RestaurantID);
                            tvpParam1.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam2 = cmd.Parameters.AddWithValue("@RestaurantMembershipID", RestMembership);
                            tvpParam2.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam3 = cmd.Parameters.AddWithValue("@RestaurantRewardPoints", RewardsCalc);
                            tvpParam3.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam4 = cmd.Parameters.AddWithValue("@IsWinBid", isWinBid);
                            tvpParam4.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam5 = cmd.Parameters.AddWithValue("@OperationType", OperationType);
                            tvpParam5.SqlDbType = SqlDbType.Text;
                            SqlParameter tvpParam6 = cmd.Parameters.AddWithValue("@TruflCashBack", TruflCashBack);
                            tvpParam6.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam7 = cmd.Parameters.AddWithValue("@TruflMemberShipID", TruflMembership);
                            tvpParam7.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam8 = cmd.Parameters.AddWithValue("@TruflRewardPoints", TruflRewardsCalc);
                            tvpParam8.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam9 = cmd.Parameters.AddWithValue("@RewardTypeID", RewardTypeID);
                            tvpParam9.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam10 = cmd.Parameters.AddWithValue("@BillAmount", restaurantRewards.BillAmount);
                            tvpParam10.SqlDbType = SqlDbType.Int;
                            SqlParameter tvpParam11 = cmd.Parameters.AddWithValue("@TruflOperationType", TruflOperationType);
                            tvpParam11.SqlDbType = SqlDbType.Text;

                            SqlParameter pvNewId = new SqlParameter();
                            pvNewId.ParameterName = "@RetVal";
                            pvNewId.DbType = DbType.Int32;
                            pvNewId.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(pvNewId);

                            status = cmd.ExecuteNonQuery();
                           
                        }
                    }
                }
                if (status == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public DataSet GetTruflCustomer(string QueryType, int RestaurantID)
        {
            DataSet dsResponse = new DataSet();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("spZGetTruflCustomer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter tvpParam = cmd.Parameters.AddWithValue("@QueryType", QueryType);
                        tvpParam.SqlDbType = SqlDbType.Text;
                        SqlParameter tvpParam1 = cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
                        tvpParam1.SqlDbType = SqlDbType.Int;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dsResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResponse;
        }

    }
}