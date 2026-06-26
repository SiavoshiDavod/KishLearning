using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.Resturan;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class ResturantPaymentBiz : RepositoryBase<ResturantPayment>
    {
        public static readonly ResturantPaymentBiz Instance = new ResturantPaymentBiz();
        public async Task<bool> AcceptResturantPayment(int Id, bool Accepted, bool Active, string desc)
        {
            using (var ctx = new Models.Context())
            {
                var foundEntity = await ctx.ResturantPayment.FirstOrDefaultAsync(x => x.Id == Id);
                if (foundEntity == null)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(desc))
                {
                    foundEntity.AdminDescription = desc;
                }
                else if (Accepted)
                {
                    foundEntity.AdminDescription = "تایید شد";
                }
                foundEntity.IsAccepted = Accepted;
               // foundEntity.Active = Active;
                foundEntity.UpdateDate = DateTime.Now;
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public override async Task<ResturantPayment> Save(ResturantPayment model, bool changeDate = true)
        {
            if (model.Id > 0)
            {
                using (var ctx = new Models.Context())
                {
                    ResturantPayment payment = await ctx.ResturantPayment.FindAsync(model.Id);
                    if (payment == null)
                    {
                        throw new HandledException("رکورد يافت نشد");
                    }
                    if (payment.IsAccepted)
                    {
                        throw new HandledException("امکان ويرايش پس از تاييد وجود ندارد");
                    }
                    payment.PaymentDate = model.PaymentDate;
                    payment.FishPic = model.FishPic;
                    payment.VarizKonande = model.VarizKonande;
                    payment.Price = model.Price;
                    payment.PaymentTypeId = model.PaymentTypeId;
                    await ctx.SaveChangesAsync();
                    return payment;
                }
            }
            return await base.Save(model, changeDate);
        }

        public SurveyWeb.JqGrid.PagedList<ResturantPayment> GetAllPagedList(GridSettings grid, bool IsAccepted)
        {
            using (var ctx = new Models.Context())
                return ctx.ResturantPayment.Where(x => x.IsAccepted == IsAccepted).Include(x => x.Resturant).Include(x => x.PaymentType).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public SurveyWeb.JqGrid.PagedList<ResturantPayment> GetAllPagedListByUser(GridSettings grid, int current_UserId)
        {
            using (var ctx = new Models.Context())
                return ctx.ResturantPayment.Where(x => x.UserId == current_UserId).Include(x => x.PaymentType).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
    }
}