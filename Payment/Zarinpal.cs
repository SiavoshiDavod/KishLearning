using System;
using System.Collections.Generic;

namespace Payment
{
    public static class StatusZarinPal
    {

        public static Dictionary<long, string> Dic = new Dictionary<long, string>()
        {
            {0,". در صورت کسر وجه مبلغ به حساب شما باز می گردد"},
            {100,"ﻋﻤﻠﻴﺎﺕ پرداخت ﺑﺎ ﻣﻮﻓﻘﻴﺖ ﺍﻧﺠﺎﻡ ﮔﺮﺩﻳﺪﻩ ﺍﺳﺖ"},
            {-1,"ﺍﻃﻼﻋﺎﺕ ﺍﺭﺳﺎﻝ ﺷﺪﻩ ﻧﺎﻗﺺ ﺍﺳﺖ"},
            {-2,"ﻭ ﻳﺎ ﻣﺮﭼﻨﺖ ﻛﺪ ﭘﺬﻳﺮﻧﺪﻩ ﺻﺤﻴﺢ ﻧﻴﺴﺖ.IP"},
            {-3,"ﺑﺎ ﺗﻮﺟﻪ ﺑﻪ ﻣﺤﺪﻭﺩﻳﺖ ﻫﺎﻱ ﺷﺎﭘﺮﻙ ﺍﻣﻜﺎﻥ ﭘﺮﺩﺍﺧﺖ ﺑﺎ ﺭﻗﻢ ﺩﺭﺧﻮﺍﺳﺖ ﺷﺪﻩ ﻣﻴﺴﺮ ﻧﻤﻲ ﺑﺎﺷﺪ"},
            {-4,"ﺳﻄﺢ ﺗﺎﻳﻴﺪ ﭘﺬﻳﺮﻧﺪﻩ ﭘﺎﻳﻴﻦ ﺗﺮ ﺍﺯ ﺳﻄﺢ ﻧﻘﺮﻩ ﺍﻱ ﺍﺳﺖ"},
            {-11," ﺩﺭﺧﻮﺍﺳﺖ ﻣﻮﺭﺩ ﻧﻈﺮ ﻳﺎﻓﺖ ﻧﺸﺪ"},
            {-12,"ﺍﻣﻜﺎﻥ ﻭﻳﺮﺍﻳﺶ ﺩﺭﺧﻮﺍﺳﺖ ﻣﻴﺴﺮ ﻧﻤﻲ ﺑﺎﺷﺪ"},
            {-21,"ﻫﻴﭻ ﻧﻮﻉ ﻋﻤﻠﻴﺎﺕ ﻣﺎﻟﻲ ﺑﺮﺍﻱ ﺍﻳﻦ ﺗﺮﺍﻛﻨﺶ ﻳﺎﻓﺖ ﻧﺸﺪ"},
            {-22," ﺗﺮﺍﻛﻨﺶ ﻧﺎ ﻣﻮﻓﻖ ﻣﻲﺑﺎﺷﺪ."},
            {-33," ﺭﻗﻢ ﺗﺮﺍﻛﻨﺶ ﺑﺎ ﺭﻗﻢ ﭘﺮﺩﺍﺧﺖ ﺷﺪﻩ ﻣﻄﺎﺑﻘﺖ ﻧﺪﺍﺭﺩ"},
            {-34," ﺳﻘﻒ ﺗﻘﺴﻴﻢ ﺗﺮﺍﻛﻨﺶ ﺍﺯ ﻟﺤﺎﻅ ﺗﻌﺪﺍﺩ ﻳﺎ ﺭﻗﻢ ﻋﺒﻮﺭ ﻧﻤﻮﺩﻩ ﺍﺳﺖ"},
            {-40," ﺍﺟﺎﺯﻩ ﺩﺳﺘﺮﺳﻲ ﺑﻪ ﻣﺘﺪ ﻣﺮﺑﻮﻃﻪ ﻭﺟﻮﺩ ﻧﺪﺍﺭﺩ."},
            {-41,". ﻏﻴﺮﻣﻌﺘﺒﺮ ﻣﻲﺑﺎﺷﺪAdditionalData - ﺍﻃﻼﻋﺎﺕ ﺍﺭﺳﺎﻝ ﺷﺪﻩ ﻣﺮﺑﻮﻁ ﺑﻪ "},
            {-42," ﺭﻭﺯ ﻣﻲ ﺑﺎﺷﺪ. 45ﺩﻗﻴﻪ ﺗﺎ  30 - ﻣﺪﺕ ﺯﻣﺎﻥ ﻣﻌﺘﺒﺮ ﻃﻮﻝ ﻋﻤﺮ  ﺷﻨﺎﺳﻪ  ﭘﺮﺩﺍﺧﺖ ﺑﺎﻳﺪ ﺑﻴﻦ "},
            {-54," ﺩﺭﺧﻮﺍﺳﺖ ﻣﻮﺭﺩ ﻧﻈﺮ ﺁﺭﺷﻴﻮ ﺷﺪﻩ ﺍﺳﺖ"},
            {101,"  ﺗﺮﺍﻛﻨﺶ ﺍﻧﺠﺎﻡ ﺷﺪﻩ ﺍﺳﺖ.PaymentVerification  ﻋﻤﻠﻴﺎﺕ ﭘﺮﺩﺍﺧﺖ ﻣﻮﻓﻖ ﺑﻮﺩﻩ ﻭ ﻗﺒﻼ"}
        };
    }


    public class Zarinpal
    {
        private zarinpal.PaymentGatewayImplementationServicePortTypeClient GetService()
        {
            try
            {
                var endpointAdress = new System.ServiceModel.EndpointAddress("https://www.zarinpal.com/pg/services/WebGate/service");
                System.ServiceModel.Channels.Binding binding1 = new System.ServiceModel.BasicHttpBinding() { Security = { Mode = System.ServiceModel.BasicHttpSecurityMode.Transport } };
                return new zarinpal.PaymentGatewayImplementationServicePortTypeClient(binding1, endpointAdress);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static readonly Zarinpal Instance = new Zarinpal();
        public string PaymentRequest(int Amount, string Description, string Email, string Mobile, out string autohority)
        {
            var setting = SiteSetting.GetSetting.Instance.Get();
            string CallbackURL = setting.SiteUrl + "Zarinpal/CallbackURL";

            autohority = string.Empty;
            int value = GetService().PaymentRequest(setting.ZarinPalMerchantID, Amount / 10, Description, Email, Mobile, CallbackURL, out autohority);
            //var pay = new zarinpal.pay(MerchantID, Amount, Description, CallbackURL, Email, Mobile);
            // : (ﺷﻨﺎﺳﻪ ﻳﻜﺘﺎﻳﻲ ﻛﻪ ﺳﺎﻳﺖ ﺯﺭﻳﻦ Authority ﺷﻨﺎﺳﻪ ﻣﺮﺟﻊ ) • ﭘﺎﻝ ﺑﻪ ﺍﺯﺍﻱ ﻫﺮ ﺩﺭﺧﻮﺍﺳﺖ ﺧﺮﻳﺪ ﺑﻪ ﭘﺬﻳﺮﻧﺪﻩ ﺍﺭﺳﺎﻝ ﻣﻲﻛﻨﺪ، ﺟﻨﺲ ﺍﻳﻦ  ﻛﺎﺭﺍﻛﺘﺮ ﻣﻲﺑﺎﺷﺪ. 36 ﺑﺎ ﻃﻮﻝ RFC( ﺑﻮﺩﻩ ﻛﻪ ﻣﻄﺎﺑﻖ universally unique identifie) UUID ﭘﺎﺭﺍﻣﺘﺮ ﺍﺯ ﻧﻮﻉ 
            //var autohority = pay.StartPay();
            if (value == 100)
            {
                //autohorityLong = long.Parse(autohority);
                var urlWebGate = "https://www.zarinpal.com/pg/StartPay/" + autohority;
                var urlZarinGate = urlWebGate + "/ZarinGate";
                if (setting.isPecPayment)
                {
                    urlWebGate = urlWebGate + "/Pec";
                }

                //new System.Threading.Thread(() =>
                //{
                //    CheckPaymentStatus(autohority,Amount);
                //}).Start();
                return urlWebGate;
            }
            else
            {
                return StatusZarinPal.Dic[value]; ;
            }
        }

        public Dictionary<int, string> PaymentRequestNew(int Amount, string Description, string Email, string Mobile, out string autohority)
        {
            var setting = SiteSetting.GetSetting.Instance.Get();
            string CallbackURL = setting.SiteUrl + "Zarinpal/CallbackURL";

            autohority = string.Empty;
            int value = GetService().PaymentRequest(setting.ZarinPalMerchantID, Amount / 10, Description, Email, Mobile, CallbackURL, out autohority);
            //var pay = new zarinpal.pay(MerchantID, Amount, Description, CallbackURL, Email, Mobile);
            // : (ﺷﻨﺎﺳﻪ ﻳﻜﺘﺎﻳﻲ ﻛﻪ ﺳﺎﻳﺖ ﺯﺭﻳﻦ Authority ﺷﻨﺎﺳﻪ ﻣﺮﺟﻊ ) • ﭘﺎﻝ ﺑﻪ ﺍﺯﺍﻱ ﻫﺮ ﺩﺭﺧﻮﺍﺳﺖ ﺧﺮﻳﺪ ﺑﻪ ﭘﺬﻳﺮﻧﺪﻩ ﺍﺭﺳﺎﻝ ﻣﻲﻛﻨﺪ، ﺟﻨﺲ ﺍﻳﻦ  ﻛﺎﺭﺍﻛﺘﺮ ﻣﻲﺑﺎﺷﺪ. 36 ﺑﺎ ﻃﻮﻝ RFC( ﺑﻮﺩﻩ ﻛﻪ ﻣﻄﺎﺑﻖ universally unique identifie) UUID ﭘﺎﺭﺍﻣﺘﺮ ﺍﺯ ﻧﻮﻉ 
            //var autohority = pay.StartPay();
            if (value == 100)
            {
                //autohorityLong = long.Parse(autohority);
                var urlWebGate = "https://www.zarinpal.com/pg/StartPay/" + autohority;
                var urlZarinGate = urlWebGate + "/ZarinGate";
                if (setting.isPecPayment)
                {
                    urlWebGate = urlWebGate + "/Pec";
                }

                //new System.Threading.Thread(() =>
                //{
                //    CheckPaymentStatus(autohority,Amount);
                //}).Start();
                var Result = new Dictionary<int, string> {
                    { 100, urlWebGate }
                };
                return Result;
            }
            else
            {
                var Result = new Dictionary<int, string> {
                    { value, StatusZarinPal.Dic[value] }
                };
                return Result; 
            }
        }

        public int Verification(string Status, string autohority, int amount, out long refId)
        {
            //long autohorityLong = long.Parse(autohority);
            var setting = SiteSetting.GetSetting.Instance.Get();
            int value = GetService().PaymentVerification(setting.ZarinPalMerchantID, autohority, amount / 10, out refId);
            if (value != 100)
            {
                refId = 0;
            }
            return value;
        }
        //public delegate void PArgs(object sender, PayArgs e);
        //public event PArgs OnPaymentAction;
        //private void CheckPaymentStatus(string autohority, int Amount )
        //{
        //    zarinpal.PaymentGatewayImplementationServicePortTypeClient request = new zarinpal.PaymentGatewayImplementationServicePortTypeClient();
        //    long refID = -1;
        //    bool stopit = false;
        //    long curtick = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        //    while (true)
        //    {
        //        if (stopit)
        //            break;
        //        int verf = -21;
        //        try
        //        {
        //            verf = request.PaymentVerification(MerchantID, autohority, Amount, out refID);
        //        }
        //        catch
        //        {

        //        }
        //        if (verf > 0)
        //        {
        //            stopit = true;
        //            if (OnPaymentAction != null)
        //            {
        //                OnPaymentAction(this, new PayArgs(verf, autohority, refID));
        //            }
        //        }
        //        else
        //        {
        //            if (!stopit && verf != -21)
        //            {
        //                stopit = true;
        //                if (OnPaymentAction != null)
        //                {
        //                    OnPaymentAction(this, new PayArgs(verf, autohority, refID));
        //                }
        //            }
        //        }
        //        long curtime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        //        if ((curtime - curtick) > 1850) // 30 * 60 +- 50
        //        {
        //            if (!stopit)
        //            {
        //                OnPaymentAction(this, new PayArgs(-22, autohority, refID));
        //                stopit = true;
        //            }
        //        }
        //    }
        //}

    }
    //public class PayArgs
    //{
    //    private int _Status;
    //    private string _Autohority;
    //    private long _RefID;
    //    public PayArgs(int Status, string Autohority, long RefID)
    //    {
    //        _Status = Status;
    //        _Autohority = Autohority;
    //        _RefID = RefID;
    //    }
    //    public int Status
    //    {
    //        get { return _Status; }
    //    }
    //    public string Autohority
    //    {
    //        get { return _Autohority; }
    //    }
    //    public long RefID
    //    {
    //        get { return _RefID; }
    //    }
    //    private string GetFromLastSlash(string text)
    //    {
    //        int where = text.LastIndexOf('/');
    //        return text.Substring(where);
    //    }
    //}
}
