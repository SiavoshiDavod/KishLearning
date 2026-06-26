using System;
using System.Threading.Tasks;

namespace SenakLearn.Biz
{
    public class EmaiSmslBiz : RepositoryBase<SenakLearn.Models.EmailSms>
    {
        public static readonly EmaiSmslBiz Instance = new EmaiSmslBiz();
        public bool AlertForClass(string toEmail, string subject, string body, SiteSetting.SiteSetting setting, bool isBodyHtml = true)
        {
            using (var context = new SWEntities())
            {
                var res = SenakLearn.SendEmail.AlertForClass(toEmail, subject, body, setting, isBodyHtml);
                Save(new Models.EmailSms()
                {
                    Body = res.Item3,
                    From = setting.EmailUser,
                    To = toEmail,
                    Subject = subject,
                    CreatedDate = DateTime.Now,
                    IsSend = res.Item1,
                    SendResult = res.Item2,
                    EmailSmsType = Models.EmailSmsType.Email
                });
                return res.Item1;
            }
        }
        public async Task<Tuple<bool, string>> SendSms(string mobile, string content)
        {
          
            if (!string.IsNullOrEmpty(mobile) && mobile.Length == 10)
            {
                mobile = "0" + mobile;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, @"09\d{9}"))
                throw new Exception("موبایل معتبر نمی باشد");

            var res = await Sms.Send.Instance.send(mobile, content);
            Save(new Models.EmailSms()
            {
                Body = content,
                From = res.Item4,
                To = mobile,
                Subject = res.Item3.ToString(),
                CreatedDate = DateTime.Now,
                IsSend = res.Item1,
                SendResult = res.Item2,
                EmailSmsType = Models.EmailSmsType.Sms
            });
            return new Tuple<bool, string>(res.Item1, res.Item2);
        }
    }
}