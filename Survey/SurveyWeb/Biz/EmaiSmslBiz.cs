using SurveyWeb.Models.TicketNotice;
using System;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class EmaiSmslBiz : RepositoryBase<EmailSms>
    {
        public static readonly EmaiSmslBiz Instance = new EmaiSmslBiz();
        public async Task<bool> AlertForClass(string toEmail, string subject, string body, SiteSetting.SiteSetting setting, bool isBodyHtml = true)
        {
            var res = SendEmail.AlertForClass(toEmail, subject, body, setting, isBodyHtml);
            await Save(new EmailSms()
            {
                Body = res.Item3.Length > 1000 ? res.Item3.Substring(0, 1000) : res.Item3,
                From = setting.EmailUser,
                To = toEmail,
                Subject = subject,
                CreatedDate = DateTime.Now,
                IsSend = res.Item1,
                SendResult = res.Item2.Length>200? res.Item2.Substring(0,200): res.Item2,
                EmailSmsType = EmailSmsType.Email
            });
            return res.Item1;
        }
        public async Task<Tuple<bool, string>> SendSms(string mobile, string content)
        {
            if (!string.IsNullOrEmpty(mobile))
            {
                if (mobile.StartsWith("0") && mobile.Length == 11)
                {
                }
                else if (mobile.StartsWith("98") && mobile.Length == 12)
                {
                    mobile = "0" + mobile.Substring(2, 10);
                }
                else if (mobile.StartsWith("9") && mobile.Length == 10)
                {
                    mobile = "0" + mobile;
                }
                else
                {
                    return new Tuple<bool, string>(false, "موبایل معتبر نمی باشد");
                }
            }else
                return new Tuple<bool, string>(false, "موبایل معتبر نمی باشد");

            if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, @"09\d{9}"))
                return new Tuple<bool, string>(false, "موبایل معتبر نمی باشد");

            var res = await Sms.Send.Instance.send(mobile, content);
            await Save(new EmailSms()
            {
                Body = content,
                From = res.Item4,
                To = mobile,
                Subject = res.Item3.ToString(),
                CreatedDate = DateTime.Now,
                IsSend = res.Item1,
                SendResult = res.Item2.Length > 200 ? res.Item2.Substring(0, 200) : res.Item2,
                EmailSmsType = EmailSmsType.Sms
            });
            return new Tuple<bool, string>(res.Item1, res.Item2);
        }
    }
}