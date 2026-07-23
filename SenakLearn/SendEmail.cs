using System;
using System.Net;
using System.Net.Mail;
using NLog;

namespace SenakLearn
{
    public static class SendEmail
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static Tuple<bool, string, string> AlertForClass(string toEmail, string subject, string body, SiteSetting.SiteSetting setting, bool isBodyHtml = true)
        {
            bool result = true;
            string message = "عملیات با موفقیت انجام شد";
            //Logger.Info("AlertForClass started. ToEmail={0}, Subject={1}", toEmail, subject);
            try
            {
                body = (body + $"<div dir='rtl' style='text-align: right;'><br>با تشکر<br>مدیریت {setting.NameFa}<br><a href='{setting.SiteUrl}'><img src='{setting.SiteUrl}images/logo.png' style='width:95px; padding: 5px;' title='{setting.NameFa}'></a></div>").Replace("'", "\"");
                var mailmessage = new MailMessage
                {
                    From = new MailAddress(setting.EmailUser+"@"
                    + setting.EmailServer),
                    To = { toEmail },
                    Subject = subject,
                    Body = body,
                    IsBodyHtml= isBodyHtml,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                //mailmessage.To.Add(new MailAddress(toEmail));

                using (SmtpClient smtpClient = new SmtpClient("mail."+setting.EmailServer))
                {
                    smtpClient.Credentials = new NetworkCredential(setting.EmailUser, setting.EmailPass);
                    smtpClient.Port = 25;
                    smtpClient.EnableSsl = false;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Timeout = 30000;

                    Logger.Info("SMTP Host: {0}", setting.EmailServer);
                    Logger.Info("SMTP User: {0}", setting.EmailUser + "@"
                    + setting.EmailServer);
                    Logger.Info("To: {0}", toEmail);
                    Logger.Info("From: {0}", mailmessage.From.Address);

                    smtpClient.Send(mailmessage);
                }
                Logger.Info("Email sent successfully. ToEmail={0}", toEmail);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to send email to {0}. Error: {1}", toEmail, ex.Message);
                //throw ex;
                message = ex.Message;
                result = false;
                
            }

            return new Tuple<bool, string, string>(result, message, body); 
        }
    }
}