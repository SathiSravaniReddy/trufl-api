using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using System.Net.Mail;
using System.Net.Configuration;
using System.Net;

namespace TruflEmailService
{
    public class MailUtility
    {
        public MailUtility()
        {

        }
        private void sendMailUsingSMTP(ResetPasswordEmailDTO user, TruflEmailDetails emailSetting)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailSetting.emailFrom);
                mail.To.Add(emailSetting.emailTo.Replace(";", ","));
                mail.Subject = emailSetting.subject;
                mail.Body = emailSetting.body;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(emailSetting.host, emailSetting.port))
                {
                    smtp.Credentials = new NetworkCredential(emailSetting.emailFrom, emailSetting.smtpPassword);
                    smtp.EnableSsl = emailSetting.enableSSL;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);
                }
            }
        }
        static object getbaseSettings()
        {
            //Configuration configurationFile = null;
            //if (HttpContext.Current != null)
            //    configurationFile = WebConfigurationManager.
            //    OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            //else
            //    configurationFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //MailSettingsSectionGroup SMTPSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            var cappemail = new TruflEmailDetails
            {
                //emailFrom = CAPPParameter.getValue<string>("EmailuserName"),
                //host = CAPPParameter.getValue<string>("networkhost"),
                //port = CAPPParameter.getValue<int>("port"),
                //smtpPassword = CAPPParameter.getValue<string>("Emailpassword"),
                //enableSSL = CAPPParameter.getValue<bool>("enableSSL")
                emailFrom = "azure_d1b6fbbef4de5f104373672b535cac66@azure.com",
                host = "smtp.sendgrid.net",
                port = 587,
                smtpPassword = "1ns!tuc@ppM",
                enableSSL = true
            };
            return cappemail;

        }

        private object GetMailSettingForResetPassword(ResetPasswordEmailDTO email)
        {
            var cappemail = getbaseSettings() as TruflEmailDetails;
            cappemail.emailTo = email.To;
            cappemail.subject = email.Subject;
            cappemail.body = email.Body;
            return cappemail;


        }
        public void sendMail(ResetPasswordEmailDTO email)
        {
            sendMailUsingSMTP(email, GetMailSettingForResetPassword(email) as TruflEmailDetails);

        }


        class TruflEmailDetails
        {
            internal string body;
            internal string emailFrom;
            internal string emailTo;
            internal bool enableSSL;
            internal string host;
            internal int port;
            internal string smtpPassword;
            internal MailSettingsSectionGroup SMTPSettings;
            internal string subject;
        }


    }
}
