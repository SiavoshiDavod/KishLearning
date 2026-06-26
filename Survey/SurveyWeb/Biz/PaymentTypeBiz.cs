using MVC.Controls;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.Resturan;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using SurveyWeb.Models;

namespace SurveyWeb.Biz
{
    public class PaymentTypeBiz : RepositoryBase<PaymentType>
    {
        public static readonly PaymentTypeBiz Instance = new PaymentTypeBiz();

        //public List<SelectListItem> PaymentTypSelectList()
        //{
        //    using (var ctx = new Models.Context())
        //        return ctx.PaymentType.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Desc + " به مبلغ:" + i.Price }).ToList();
        //}
        public async Task<PaymentType> FindByDegreeAndType(Resturant model)
        {
            using (var ctx = new Models.Context())
                return await ctx.PaymentType.FirstOrDefaultAsync(x => x.ResturantTypeId == model.ResturantTypeId && x.Degree == model.Degree && !x.Archive&& x.PaymentTypeEnumId==PaymentTypeEnum.YearlyByDegree);
        }
        public async Task<PaymentType> FindByMeter(Resturant model)
        {
            using (var ctx = new Models.Context())
                return await ctx.PaymentType.FirstOrDefaultAsync(x => !x.Archive && x.PaymentTypeEnumId == PaymentTypeEnum.YearlyByMeter);
        }
        public override Task<PaymentType> Save(PaymentType model, bool changeDate = true)
        {
            if ( model.PaymentTypeEnumId == PaymentTypeEnum.YearlyByDegree)
            {
                if (model.ResturantTypeId == null)
                    throw new HandledException("لطفا نوع مرکزپذیرایی را از لیست انتخاب کنید");
                model.Title = model.PaymentTypeEnumName;
            }
          
            using (var ctx = new Models.Context())
            {
                if (model.PaymentTypeEnumId == PaymentTypeEnum.YearlyByDegree && ctx.PaymentType.Any(x => x.ResturantTypeId == model.ResturantTypeId && x.Degree == model.Degree && !x.Archive && x.Id != model.Id && x.PaymentTypeEnumId==PaymentTypeEnum.YearlyByDegree))
                {
                    throw new HandledException("تنظیمات با این نوع مرکزپذیرایی و رتبه وجود دارد ");
                }
                if (model.PaymentTypeEnumId == PaymentTypeEnum.YearlyByMeter && ctx.PaymentType.Any(x => !x.Archive && x.Id != model.Id && x.PaymentTypeEnumId == PaymentTypeEnum.YearlyByMeter))
                {
                    throw new HandledException("تنظیمات حق پرداخت سالیانه بر اساس متراژ وجود دارد ");
                }
            }
            return base.Save(model, changeDate);
        }
        public override SurveyWeb.JqGrid.PagedList<PaymentType> GetAllPagedList(GridSettings grid)
        {
            using (var ctx = new Models.Context())
                return ctx.PaymentType/*.Where(x => x.Archive)*/.Include(x => x.ResturantType).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        internal async Task SetArchive(int id)
        {
            using (var ctx = new Models.Context())
            {
                var payment = await ctx.PaymentType.FindAsync(id);
                if (!payment.Archive)
                {
                    payment.Archive = true;
                    payment.UpdateDate = System.DateTime.Now;
                    await ctx.SaveChangesAsync();
                }
            }
        }
    }
}