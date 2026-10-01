using SenakLearn.Models;
using System;
using System.Linq;

namespace SenakLearn.Biz
{
    public class FactorPaymentRequestBiz
    {
        public static readonly FactorPaymentRequestBiz Instance = new FactorPaymentRequestBiz();

        public long CreateRequest(long factorId, int? userId, decimal amount, string userIp)
        {
            using (var db = new SWEntities())
            {
                var request = new FactorePaymentRequest
                {
                    FactorId = factorId,
                    UserId = userId,
                    Amount = amount,
                    RequestDateTime = DateTime.Now,
                    StatusId = PaymentRequestStatusEnum.Initiated,
                    IpAddress = userIp
                };
                db.FactorePaymentRequests.Add(request);
                db.SaveChanges();
                return request.Id;
            }
        }

        public void UpdateToken(long paymentRequestId, string token, short status, string rawRequest,string responseContentRequest = null)
        {
            using (var db = new SWEntities())
            {
                var item = db.FactorePaymentRequests.FirstOrDefault(x => x.Id == paymentRequestId);
                if (item != null)
                {
                    item.Token = token;
                    item.TerminalStatus = status;
                    item.RawRequestData = rawRequest;
                    item.StatusId = (status == 0 && !string.IsNullOrEmpty(token))
                                    ? PaymentRequestStatusEnum.TokenReceived
                                    : PaymentRequestStatusEnum.ErrorOnRequest;
                    item.ResponseContentRequest = responseContentRequest;
                    db.SaveChanges();
                }
            }
        }

        public FactorePaymentRequest GetById(long id)
        {
            using (var db = new SWEntities())
            {
                return db.FactorePaymentRequests.Include("Factor").FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
