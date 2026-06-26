using System;
using System.Net;
using System.Net.Mail;

namespace SurveyWeb
{
    public static class SendEmail
    {
        public static Tuple<bool, string, string> AlertForClass(string toEmail, string subject, string body, SiteSetting.SiteSetting setting, bool isBodyHtml = true)
        {
            bool result = true;
            string message = "عملیات با موفقیت انجام شد";
            try
            {
                body = (body + $"<div  dir='rtl' style='text-align: right; ><br>با تشکر<br>مدیریت {setting.NameFa}<br><a href='{setting.SiteUrl}' ><img src='{setting.SiteUrl}lib/SeoHub/images/kish1400.png' style='width:95px; padding: 5px;' title='{setting.NameFa}'></a></div>").Replace("'", "\"");
                var mailmessage = new MailMessage
                {
                    From = new MailAddress(setting.EmailUser + "@"
                    + setting.EmailServer),
                    To = { toEmail },
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isBodyHtml,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                //mailmessage.To.Add(new MailAddress(toEmail));

                using (SmtpClient smtpClient = new SmtpClient(setting.EmailServer))
                {
                    smtpClient.Credentials = new NetworkCredential(setting.EmailUser, setting.EmailPass);
                    smtpClient.Port = 25;
                    smtpClient.EnableSsl = false;
                    smtpClient.Send(mailmessage);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                message = ex.Message;
                result = false;
            }

            return new Tuple<bool, string, string>(result, message, body);
        }
    }
}