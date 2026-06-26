//using MVC.Controls;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class FactorBiz : RepositoryBase<FactorModel>
    {
        public static readonly FactorBiz Instance = new FactorBiz();

        public long AddFactor(FactorModel model)
        {
            try
            {
                using (var context = new SWEntities())
                {
                    context.Factors.Add(model);
                    context.SaveChanges();
                    return model.Id;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        public bool UpdateFactor(FactorModel model)
        {
            try
            {
                using (var context = new SWEntities())
                {
                    var item = context.Factors.FirstOrDefault(a => a.Id == model.Id);
                    if (item == null)
                        return false;
                    item.StatusId = model.StatusId;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool RemovedFactor(long id)
        {
            try
            {
                using (var context = new SWEntities())
                {
                    var item = context.Factors.FirstOrDefault(a => a.Id == id);
                    if (item == null)
                        return false;
                    item.StatusId = FactorStatusEnum.Factor_Status_Removed;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public PagedList<FactorViewModel> GetAllPagedList(GridSettings grid, int userId, bool isAdmin)
        {
            try
            {
                using (SWEntities db = new SWEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    //var lid = db.Factors.Where(w => (isAdmin || w.UserId == userId)).ToList();
                    var list = (from i in db.Factors
                                select new FactorViewModel
                                {
                                    Id = i.Id,
                                    ServiceName = i.ServiceName,
                                    StatusId = i.StatusId,
                                    CreateDate = i.CreateDate,
                                    CreatedDate = i.CreateDate,
                                    Amount = i.Amount,
                                    Descript = i.Descript,
                                    Discount = i.Discount,
                                    Email = i.Email,
                                    Mobile = i.Mobile,
                                    PaymentTrace = i.PaymentTrace,
                                    PaymentTransaction = i.PaymentTransaction,
                                    PaymentDatetime = i.PaymentDatetime,
                                    UserId = i.UserId
                                }).Where(w => (isAdmin || w.UserId == userId)).FilterAndSortJqGrid(grid).ToPagedList(grid);
                    return list;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public FactorModel Get(long id)
        {
            // var context =  MyConstructor();
            if (id == 0)
            {
                return null;
            }
            using (var context = new SWEntities())
            {
                var item = context.Set<FactorModel>().Where(x => x.Id == id).FirstOrDefault();
                item.CreatedDate = item.CreateDate;
                if (item.UserId != null && item.UserId != 0)
                    item.UserName = UserBiz.Instance.Find(item.UserId.Value)?.user_name;
                return item;
            }
            //return result;
        }
        public FactorModel LoadFactor(int userId, string idForSale, string serviceName)
        {
            // var context =  MyConstructor();
            if (userId == 0 || string.IsNullOrEmpty(idForSale) || idForSale == "0" || string.IsNullOrEmpty(serviceName))
            {
                return null;
            }
            using (var context = new SWEntities())
            {
                var item = context.Factors.Where(x => x.UserId == userId && x.IdForSale == idForSale && x.ServiceName==serviceName).FirstOrDefault();
                if (item == null) return null;
                if (item.UserId != null && item.UserId != 0)
                    item.UserName = UserBiz.Instance.Find(item.UserId.Value)?.user_name;
                return item;
            }
            //return result;
        }
        public string GetFactorCode(string servicename)
        {
            string code = null;
            if (servicename == "دوره آموزشی")
                code = "C-" + new Random().Next(10000, 99999);
            else if (servicename == "پادکست")
                code = "P-" + new Random().Next(10000, 99999);
            return code;
        }
    }
}