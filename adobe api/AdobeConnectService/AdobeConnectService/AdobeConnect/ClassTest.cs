using AdobeConnectSDK;
using AdobeConnectSDK.Common;
using AdobeConnectSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdobeConnect
{
    public class ClassTest
    {
        private static CookieContainer cookieContainer;
        private static HttpClientHandler clienthandler;
        private HttpClient httpClient;
        private string user;
        private string pass;
        public bool IsLogin { get;private set; }
        public ClassTest(string user = "rahmatymahdi@gmail.com", string pass = "123456")
        {
            cookieContainer = new CookieContainer();
            clienthandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = cookieContainer };
            httpClient = new HttpClient(clienthandler);
            this.user = user;
            this.pass = pass;
            var response = httpClient.PostAsync(server + "?action=login&login=" + user + "&password=" + pass, new StringContent("", Encoding.UTF8, "application/json")).Result;
            if (response.StatusCode==HttpStatusCode.OK)
            {
                var token = response.Content.ReadAsStringAsync().Result;
                if (token.Contains("<status code=\"ok\"/>"))
                {
                    IsLogin = true;
                }
            }
        }
        private const string server = "http://46.209.20.165/api/xml";
        public void GetUserInfo()
        {
            if (IsLogin)
            {
                var responseUser = httpClient.PostAsync(server + "?action=common-info", new StringContent("", Encoding.UTF8, "application/json")).Result;
                var userInfo = responseUser.Content.ReadAsStringAsync().Result;
                var model = userInfo.GetType();
            }
            else
            {
                throw new Exception("you are not login");
            }
               // XmlSerializerHelpersGeneric.FromXML<UserInfo>(userInfo.ResultDocument.Descendants("user").FirstOrDefault().CreateReader());
        }
        public bool LogOut()
        {
            if (IsLogin)
            {
                var response = httpClient.PostAsync(server + "?action=logout", new StringContent("", Encoding.UTF8, "application/json")).Result;
                var res = response.Content.ReadAsStringAsync().Result;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
