using System;
using System.Threading.Tasks;

namespace Sms
{
    public class Send
    {
        public static readonly Send Instance = new Send();
        private FaraPayamakSendSMS.SendSoapClient GetService()
        {
            try
            {
                var endpointAdress = new System.ServiceModel.EndpointAddress("http://api.payamak-panel.com/post/Send.asmx");
                var binding1 = new System.ServiceModel.BasicHttpBinding();
                return new FaraPayamakSendSMS.SendSoapClient(binding1, endpointAdress);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public double GetCredit()
        {
            try
            {
                var setting = SiteSetting.GetSetting.Instance.Get();
                var sms = GetService();
                return sms.GetCredit(setting.FaraPayamakUser, setting.FaraPayamakPass);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public async Task<Tuple<bool, string, long, string>> send(string phoneNumber, string content)
        {
            string message = "پیامک با موفقیت ارسال شد";
            var sms = GetService();

            //string result = await sms.SendSimpleSMS2Async(userName, pass, phoneNumber, HeadNumber, content, false);
            //return new Tuple<bool, string>(true, message);
            //string[] resultArr = await sms.SendSimpleSMSAsync(userName, pass, new string[] { phoneNumber }, HeadNumber, content, false);
            //if (int.Parse(resultArr[0]) > 100)
            //{
            //    return true;
            //}
            long[] rec = null;///دارد delivery  این کد یکتا برای دریافت ،  هرخانه آرایه یک کد یکتا به ازای هر گیرنده تولید می کند
            byte[] status = null;// Sent=0, Failed=1
            // int retval = sms.SendSms(userName, pass, new string[] { phoneNumber }, HeadNumber, content, false, "", ref rec, ref status);
            var setting = SiteSetting.GetSetting.Instance.Get();
            FaraPayamakSendSMS.SendSmsResponse retvalObj = await sms.SendSmsAsync(new FaraPayamakSendSMS.SendSmsRequest(setting.FaraPayamakUser, setting.FaraPayamakPass, new string[] { phoneNumber }, setting.FaraPayamakNumber, content, false, "", rec, status));
            rec = retvalObj.recId;
            status = retvalObj.status;
            int retval = retvalObj.SendSmsResult;
            long recId = 0;
            if (rec != null)
            {
                recId = rec[0];
            }
            //retval :
            // Invalid User Pass=0,
            // Successfull = 1,
            // No Credit = 2,
            // DailyLimit = 3,
            // SendLimit = 4,
            // Invalid Number = 5
            // System IS Disable = 6
            // Bad Words= 7
            // Pardis Minimum Receivers=8
            // Number Is Public=9



            //:نام کاربری یا رمز عبور اشتباه می باشد 0.
            //:درخواست با موفقیت انجام شد 1.
            // :اعتبار کافی نمی باشد 2.
            // محدودیت در ارسال روزانه 3.
            // محدودیت در حجم ارسال 4.
            // .شماره فرستنده معتبر نمی باشد 5
            //  .سامانه در حال بروزرسانی می باشد 6.
            // .متن حاوی کلمه فیلتر شده می باشد 7.
            // .ارسال از خطوط عمومی از طریق وب سرویس امکان پذیر نمی باشد 9.
            // .کاربر مورد نظر فعال نمی باشد 10
            //.ارسال نشده 11.
            // .مدارک کاربر کامل نمی باشد 12
            switch (retval)
            {
                case 0:
                    message = "نام کاربری یا رمز عبور اشتباه می باشد";
                    break;
                case 1:

                    //for (int i = 0; i < rec.Length; i++)
                    //{
                    //    var delivery = sms.GetDelivery(rec[i]);
                    //    // ارسال شده به مخابرات=0

                    //    // رسیده به گوشی=1

                    //    // نرسیده به گوشی=2

                    //    // خطای مخابراتی=3

                    //    // خطای نا مشخص=5

                    //    // رسیده به مخابرات=8

                    //    // نرسیده به مخابرات=16

                    //    // نا مشخص=100
                    //}
                    if (status != null && status.Length == 1 && status[0] != 0)
                    {
                        //Status :
                        // Sent=0,
                        // Failed=1
                        message = "وب سرویس با موفقیت فراخوانی شد ولی پیامک ارسال نشد";
                    }
                    else
                    {
                        return new Tuple<bool, string, long, string>(true, message, recId, setting.FaraPayamakNumber);
                    }
                    break;
                case 2:
                    message = "اعتبار کافی نمی باشد";
                    break;
                case 3:
                    message = "محدودیت در ارسال روزانه";
                    break;
                case 4:
                    message = "محدودیت در حجم ارسال";
                    break;
                case 5:
                    message = "شماره فرستنده معتبر نمی باشد";
                    break;
                case 6:
                    message = "سامانه در حال بروزرسانی می باشد";
                    break;
                case 7:
                    message = "متن حاوی کلمه فیلتر شده می باشد";
                    break;
                case 9:
                    message = "ارسال از خطوط عمومی از طریق وب سرویس امکان پذیر نمی باشد";
                    break;
                case 10:
                    message = "کاربر مورد نظر فعال نمی باشد";
                    break;
                case 11:
                    message = "ارسال نشده";
                    break;
                case 12:
                    message = "مدارک کاربر کامل نمی باشد";
                    break;
                default:
                    message = "خطا در ارسال پیامک";
                    break;
            }
            return new Tuple<bool, string, long, string>(false, message, recId, setting.FaraPayamakNumber);

            //var count1 = sms.GetInboxCount(userName, pass, false);
            //var msg = sms.getMessages(userName, pass, 1,HeadNumber, 5, 5);
        }
    }
}