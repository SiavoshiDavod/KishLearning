namespace AdobeConnectSDK.Common
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using AdobeConnectSDK.Interfaces;
    using AdobeConnectSDK.Model;

    public class HttpCommunicationProvider : ICommunicationProvider
    {
        //private string m_SessionInfo = string.Empty;
        //private string m_SessionDomain = string.Empty;

        public ISdkSettings Settings { get; set; }

        public ApiStatus ProcessRequest(string pAction, string qParams)
        {
            if (this.Settings == null)
            {
                throw new InvalidOperationException("This provider is not configured.");
            }

            ApiStatus operationApiStatus = new ApiStatus();
            operationApiStatus.Code = StatusCodes.NotSet;

            if (qParams == null)
                qParams = string.Empty;

            string url = this.Settings.ServiceURL + string.Format(@"?action={0}&{1}", pAction, qParams);
            
            try
            {
                //FIX: Invalid SSL passing behavior
                //(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                ServicePointManager.ServerCertificateValidationCallback = delegate
                {
                    return true;
                };

                //if (!this.Settings.UseSessionParam)
                //{
                //    if (!string.IsNullOrEmpty(m_SessionInfo) && !string.IsNullOrEmpty(m_SessionDomain))
                //        this.Settings.httpClient.CookieContainer.Add(new Cookie("BREEZESESSION", this.m_SessionInfo, "/", this.m_SessionDomain));
                //}
                var receiveStream = this.Settings.httpClient.GetStreamAsync(url).Result;
                if (receiveStream==null || !receiveStream.CanRead)
                {
                    return null;
                }
                //if (this.Settings.UseSessionParam)
                //{
                //if (response.Headers.Server.Cookies["BREEZESESSION"] != null)
                //{
                //    this.m_SessionInfo = HttpWResp.Cookies["BREEZESESSION"].Value;
                //    this.m_SessionDomain = HttpWResp.Cookies["BREEZESESSION"].Domain;
                //}
                //}
                using (var readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    //#if DEBUG
                    //          string buf = readStream.ReadToEnd();
                    //          File.WriteAllText("httpproviderdump.txt", buf);
                    //          operationApiStatus = Helpers.ResolveOperationStatusFlags(new XmlTextReader(new StringReader(buf)));
                    //#else
                    operationApiStatus = Helpers.ResolveOperationStatusFlags(new XmlTextReader(readStream));
                    //#endif
                }

                //if (this.Settings.UseSessionParam)
                //{
                //    operationApiStatus.SessionInfo = this.m_SessionInfo;
                //}

                return operationApiStatus;
            }
            catch (Exception ex)
            {
                throw new Exception("Adobe-Common-HttpCommunicationProvider-ProcessRequest", ex.InnerException ?? ex);
                //throw ex.InnerException;
            }

            //return null;
        }
    }
}