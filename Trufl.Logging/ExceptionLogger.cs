using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trufl.Logging
{
    public static class ExceptionLogger
    {
        /// <summary>
        /// Writes the Exception to the FileSystem Watcher Error Log File
        /// </summary>
        public static void WriteToErrorLogFile(Exception ex, string message = "")
        {
            DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            string weekStartDate = dt.ToShortDateString();

            string Ropotpath = AppDomain.CurrentDomain.BaseDirectory;
            string ErrorLogpath = "ErrorLog";
            string path = Path.Combine(Ropotpath, ErrorLogpath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "/ErrorLog_" + weekStartDate.Replace("/", string.Empty) + ".txt";
            FileStream fs = null;
            StreamWriter sw = null;
            if (!File.Exists(path))
                fs = File.Create(path);
            else
                fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

            try
            {

                sw = new StreamWriter(fs);
                sw.WriteLine("==================================================================");
                sw.WriteLine("Date and Time: " + DateTime.Now.ToString());

                if (ex.Source != null)
                    sw.WriteLine("ERROR OCCOURED IN:" + ex.Source);

                if (ex.Message != null)
                    sw.WriteLine("ERROR MESSAGE:" + ex.Message);

                if (ex.InnerException != null)
                    sw.WriteLine("ERROR INNER EXCEPTION:" + ex.InnerException.Message);

                if (ex.StackTrace != null)
                    sw.WriteLine("ERROR DESCRPTION:" + ex.StackTrace);
                sw.WriteLine("File PAth:" + message);
                sw.WriteLine("");
                sw.WriteLine("");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static void WriteToLogFile(string message = "")
        {
            DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            string weekStartDate = dt.ToShortDateString();

            string Ropotpath = AppDomain.CurrentDomain.BaseDirectory;
            string ErrorLogpath = "ErrorLogDebug";
            string path = Path.Combine(Ropotpath, ErrorLogpath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + "/ErrorLog_" + weekStartDate.Replace("/", string.Empty) + ".txt";
            FileStream fs = null;
            StreamWriter sw = null;
            if (!File.Exists(path))
                fs = File.Create(path);
            else
                fs = new FileStream(path, FileMode.Append, FileAccess.Write);

            try
            {

                sw = new StreamWriter(fs);
                sw.WriteLine("==================================================================");
                sw.WriteLine("Date and Time: " + DateTime.Now.ToString());

                sw.WriteLine("File PAth:" + message);
                sw.WriteLine("");
                sw.WriteLine("");
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }

                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
    }
}
