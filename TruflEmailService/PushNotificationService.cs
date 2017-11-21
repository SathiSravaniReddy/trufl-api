using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Http;
using System.Configuration;

namespace TruflEmailService
{
    public class PushNotificationService
    {

        //[Route("SendNotificationFromFirebaseCloud")]
        //[HttpGet]
        public string SendNotificationFromFirebaseCloud(string DeviceID, string Message)
        {
            var result = "-1";
            string serverKey = ConfigurationManager.AppSettings["PushNotificationServerKey"];

            //serverKey = "AIzaSyCZy2efY1j8A3QmTm79OjJFcVyUfcqN9GM";
            //Message = "It's your turn to be seated! Please see our hosts right away. They will show you to your table. Let's do this!";
            string Your_Notif_Title = "Trufl Services";

            try
            {

                var webAddr = "https://fcm.googleapis.com/fcm/send";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + DeviceID + "\",\"notification\": {\"body\": \"" + Message + "\",\"title\":\"" + Your_Notif_Title + "\",}}";
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;      // httpResponse.StatusDescription;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
                return result;      // httpResponse.StatusDescription;
        }
    }
}
