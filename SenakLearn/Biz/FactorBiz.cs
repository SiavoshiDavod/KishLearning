//using MVC.Controls;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    item.Descript = model.Descript;
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
            using (SWEntities db = new SWEntities())
            {
                var query = GetAllQuery(db, userId, isAdmin);
                var list = query.FilterAndSortJqGrid(grid).ToPagedList(grid);

                list.ForEach(item =>
                {
                    if (item.PaymentDatetime.HasValue)
                        item.PaymentDatetimePer = item.PaymentDatetime.Value.ToPersianDateTime();
                    if (!string.IsNullOrEmpty(item.IdForSale) && !string.IsNullOrEmpty(item.ServiceName))
                        item.TitleForSale = item.ServiceName == FactorServiceName.Factor_ServiceName_Book ?
                                                BookBiz.Instance.Get(int.Parse(item.IdForSale))?.TitleF :
                                                  string.Empty;
                });
                return list;
            }
        }

        public async Task<List<FactorViewModel>> GetAllList(int userId, bool isAdmin, bool? onlySuccess = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var query = GetAllQuery(db, userId, isAdmin);
                if (onlySuccess == true)
                    query = query.Where(w => w.StatusId == FactorStatusEnum.Factor_Status_Success);
                var list = await query.ToListAsync();
                list.ForEach(item =>
                {
                    if (item.PaymentDatetime.HasValue)
                        item.PaymentDatetimePer = item.PaymentDatetime.Value.ToPersianDateTime();
                    if (!string.IsNullOrEmpty(item.IdForSale) && !string.IsNullOrEmpty(item.ServiceName))
                        item.TitleForSale = item.ServiceName == FactorServiceName.Factor_ServiceName_Book ?
                                                BookBiz.Instance.Get(int.Parse(item.IdForSale))?.TitleF :
                                                  string.Empty;
                });
                return list;
            }
        }
        public async Task<MemoryStream> GetAllReportExcel(int userId, bool isAdmin)
        {
            var list = await GetAllList(userId, isAdmin);

            var excelService = new ExcelService();
            var memoryStream = excelService.GenerateExcelFile(list);
            return memoryStream;

        }
        public IQueryable<FactorViewModel> GetAllQuery(SWEntities db, int userId, bool isAdmin)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                //var lid = db.Factors.Where(w => (isAdmin || w.UserId == userId)).ToList();
                var list = (from i in db.Factors
                            join u in db.learn_user on i.UserId equals u.id
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
                                UserId = i.UserId,
                                UserName = u.user_name,
                                IdForSale = i.IdForSale,
                            }).Where(w => (isAdmin || w.UserId == userId));

                return list;

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
                var item = context.Factors.Where(x => x.Id == id).FirstOrDefault();
                item.CreatedDate = item.CreateDate;
                if (item.UserId != null && item.UserId != 0)
                {
                    var user = UserBiz.Instance.Find(item.UserId.Value);
                    item.UserName = user?.user_name;
                    item.FullName = (!string.IsNullOrEmpty(user.Name) || !string.IsNullOrEmpty(user.Family)) ? (user.Name + " " + user.Family) : string.Empty;
                }
                if (item.ServiceName == FactorServiceName.Factor_ServiceName_Book)
                {
                    int bookid = int.Parse(item.IdForSale);
                    var book = BookBiz.Instance.Get(bookid);
                    item.TitleForSale = book.TitleF;

                }
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
                var item = context.Factors.Where(x => x.UserId == userId && x.IdForSale == idForSale && x.ServiceName == serviceName && x.StatusId != FactorStatusEnum.Factor_Status_Removed).FirstOrDefault();


                if (item == null) return null;
                if (serviceName == FactorServiceName.Factor_ServiceName_Book)
                {
                    int bookid = int.Parse(idForSale);
                    var book = BookBiz.Instance.Get(bookid);
                    item.TitleForSale = book.TitleF;

                }
                if (item.UserId != null && item.UserId != 0)
                {
                    var user = UserBiz.Instance.Find(item.UserId.Value);
                    item.UserName = user?.user_name;
                    item.FullName = (!string.IsNullOrEmpty(user.Name) || !string.IsNullOrEmpty(user.Family)) ? (user.Name + " " + user.Family) : string.Empty;
                }
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
            else if (servicename == "کتاب")
                code = "B-" + new Random().Next(10000, 99999);
            return code;
        }
        public FactorModel LoadNewFactore(FactorModel model)
        {


            if (model.ServiceName == FactorServiceName.Factor_ServiceName_Course)
            { }
            else if (model.ServiceName == FactorServiceName.Factor_ServiceName_Podcast)
            { }
            else if (model.ServiceName == FactorServiceName.Factor_ServiceName_Book)
            {
                var book = BookBiz.Instance.Get(int.Parse(model.IdForSale));
                if (book == null)
                    return null;

                int bookid = int.Parse(model.IdForSale);
                model.TitleForSale = book.TitleF;


                model.TitleForSale = book.TitleF;
                model.Descript = book.TitleF;
                model.Amount = book.PriceAfterDiscount ?? book.Price;
            }

            return model;
        }
    }
}