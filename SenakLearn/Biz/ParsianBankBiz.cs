using SenakLearn.Models;
using SenakLearn.Models.Parsian;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace SenakLearn.Biz
{
    public class ParsianBankBiz
    {
        public static readonly ParsianBankBiz Instance = new ParsianBankBiz();

        private readonly string _loginAccount;
        private readonly string _callbackUrl;
        private readonly string _gateUrl;

        private const string SaleServiceUrl = "https://pec.shaparak.ir/NewIPGServices/Sale/SaleService.asmx";
        private const string ConfirmServiceUrl = "https://pec.shaparak.ir/NewIPGServices/Confirm/ConfirmService.asmx";
        private const string ReversalServiceUrl = "https://pec.shaparak.ir/NewIPGServices/Reverse/ReversalService.asmx";

        private ParsianBankBiz()
        {
            _loginAccount = ConfigurationManager.AppSettings["Parsian_LoginAccount"] ?? "";
            _callbackUrl = ConfigurationManager.AppSettings["Parsian_CallbackUrl"] ?? "";
            _gateUrl = ConfigurationManager.AppSettings["Parsian_GateUrl"] ?? "https://pec.shaparak.ir/NewIPG/?Token=";
        }

        /// <summary>
        /// ارسال درخواست توکن به درگاه پارسیان (SalePaymentRequest)
        /// </summary>
        public ParsianPaymentResponse PerPaymentFactor(int amount, string description, string email, string mobile, int userId, long factorId, object currentUser)
        {
            var response = new ParsianPaymentResponse();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string orderId = factorId.ToString();
                string additionalData = $"UserId={userId};Mobile={mobile}";

                // توجه: حرف S در Shaparak بزرگ است
                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <SalePaymentRequest xmlns=""https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService"">
      <requestData>
        <LoginAccount>{_loginAccount}</LoginAccount>
        <Amount>{amount}</Amount>
        <OrderId>{orderId}</OrderId>
        <CallBackUrl>{_callbackUrl}</CallBackUrl>
        <AdditionalData>{additionalData}</AdditionalData>
        <Originator>{mobile}</Originator>
      </requestData>
    </SalePaymentRequest>
  </soap:Body>
</soap:Envelope>";

                string soapResult = ExecuteSoapRequest(
                    SaleServiceUrl,
                    "https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentRequest",
                    soapEnvelope
                );

                var xDoc = XDocument.Parse(soapResult);

                var saleResponse = xDoc.Descendants().FirstOrDefault(d =>
                    d.Name.LocalName == "SalePaymentRequestResult" ||
                    d.Name.LocalName == "SalePaymentResponseData");

                if (saleResponse != null)
                {
                    short status = short.Parse(saleResponse.Elements().FirstOrDefault(e => e.Name.LocalName == "Status")?.Value ?? "-1");
                    string token = saleResponse.Elements().FirstOrDefault(e => e.Name.LocalName == "Token")?.Value;
                    string message = saleResponse.Elements().FirstOrDefault(e => e.Name.LocalName == "Message")?.Value;

                    response.Status = status;
                    response.Token = token;
                    response.Message = message;

                    if (status == 0 && !string.IsNullOrEmpty(token) && token != "0")
                    {
                        response.BankUrl = _gateUrl + token;
                    }
                    else
                    {
                        response.Message = !string.IsNullOrEmpty(message) ? message : GetParsianErrorMessage(status);
                    }
                }
                else
                {
                    var fault = xDoc.Descendants().FirstOrDefault(d => d.Name.LocalName == "Fault");
                    string faultMsg = fault?.Elements().FirstOrDefault(e => e.Name.LocalName == "faultstring")?.Value;

                    response.Status = -1;
                    response.Message = !string.IsNullOrEmpty(faultMsg) ? faultMsg : "خطا در ساختار پاسخ درگاه پارسیان.";
                }
            }
            catch (Exception ex)
            {
                response.Status = -999;
                response.Message = "خطا در پردازش درگاه پارسیان: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// تأیید نهایی تراکنش (ConfirmPayment)
        /// </summary>
        public (bool IsSuccess, short Status, long RRN, string Message) VerifyPayment(long token)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <ConfirmPayment xmlns=""https://pec.Shaparak.ir/NewIPGServices/Confirm/ConfirmService"">
      <requestData>
        <LoginAccount>{_loginAccount}</LoginAccount>
        <Token>{token}</Token>
      </requestData>
    </ConfirmPayment>
  </soap:Body>
</soap:Envelope>";

                string soapResult = ExecuteSoapRequest(
                    ConfirmServiceUrl,
                    "https://pec.Shaparak.ir/NewIPGServices/Confirm/ConfirmService/ConfirmPayment",
                    soapEnvelope
                );

                var xDoc = XDocument.Parse(soapResult);

                var confirmResponse = xDoc.Descendants().FirstOrDefault(d =>
                    d.Name.LocalName == "ConfirmPaymentResult" ||
                    d.Name.LocalName == "ConfirmPaymentResponseData");

                if (confirmResponse != null)
                {
                    short status = short.Parse(confirmResponse.Elements().FirstOrDefault(e => e.Name.LocalName == "Status")?.Value ?? "-1");
                    long rrn = long.Parse(confirmResponse.Elements().FirstOrDefault(e => e.Name.LocalName == "RRN")?.Value ?? "0");

                    if (status == 0)
                    {
                        return (true, status, rrn, "تراکنش با موفقیت تأیید شد.");
                    }
                    return (false, status, rrn, GetParsianErrorMessage(status));
                }

                return (false, -1, 0, "پاسخی از سرویس وریفای دریافت نشد.");
            }
            catch (Exception ex)
            {
                return (false, -999, 0, ex.Message);
            }
        }

        private string ExecuteSoapRequest(string url, string soapAction, string soapEnvelope)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Headers.Add("SOAPAction", $"\"{soapAction}\"");
            req.ContentType = "text/xml; charset=utf-8";
            req.Accept = "text/xml";
            req.Method = "POST";
            req.Timeout = 45000;

            byte[] bytes = Encoding.UTF8.GetBytes(soapEnvelope);
            req.ContentLength = bytes.Length;

            using (Stream st = req.GetRequestStream())
            {
                st.Write(bytes, 0, bytes.Length);
            }

            try
            {
                using (WebResponse response = req.GetResponse())
                using (StreamReader rd = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return rd.ReadToEnd();
                }
            }
            catch (WebException webEx)
            {
                if (webEx.Response != null)
                {
                    using (var reader = new StreamReader(webEx.Response.GetResponseStream(), Encoding.UTF8))
                    {
                        string faultString = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(faultString))
                        {
                            return faultString;
                        }
                    }
                }
                throw;
            }
        }

        public string GetParsianErrorMessage(short status)
        {
            switch (status)
            {
                case -1: return "پارامترهای ارسالی نامعتبر است.";
                case -2: return "پذیرنده غیرفعال است.";
                case -3: return "آدرس آی‌پی (IP) پذیرنده نامعتبر است یا در شاپرک تعریف نشده.";
                case -4: return "کد پذیرنده (LoginAccount / PIN) اشتباه است.";
                case -112: return "شماره فاکتور/سفارش تکراری است (OrderId Duplicate).";
                case -126: return "کد شناسایی پذیرنده (LoginAccount) معتبر نمی‌باشد.";
                case -127: return "آدرس اینترنتی (URL) یا IP معتبر نمی‌باشد.";
                case -130: return "توکن منقضی یا نامعتبر است.";
                case -131: return "توکن قبلاً مصرف/وریفای شده است.";
                case -138: return "مبلغ تراکنش خارج از محدوده مجاز است.";
                case -1531: return "تراکنش لغو یا ناموفق بوده است.";
                default: return $"خطای درگاه پارسیان (کد: {status})";
            }
        }
    }
}
