using SurveyWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models.wrapper;
using System.Data.Entity;
using SurveyWeb.Models.Resturan;

namespace SurveyWeb.Biz
{
    public class CartableBiz : RepositoryBase<Models.Cartable>
    {
        public static readonly CartableBiz Instance = new CartableBiz();
        public List<SelectListItem> DropDown()
        {
            using (var ctx = new Context())
                return ctx.Cartable.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name + "/" + i.CartableType }).ToList();
        }
        public override async Task<Cartable> Save(Cartable model, bool changeDate = true)
        {
            if (model.IsFirstState && model.IsLastState)
            {
                throw new HandledException("یک کارتابل نمی تواند هم مرحله اول و هم مرحله آخر باشد");
            }

            if (model.IsFirstState || model.IsLastState)
            {
                using (var ctx = new Context())
                {
                    if (ctx.Cartable.Count(x => x.CartableType == model.CartableType && x.Id != model.Id && (model.IsLastState && x.IsLastState) && (model.IsFirstState && x.IsFirstState)) > 0)
                    {
                        if (model.IsFirstState)
                            throw new HandledException("مرحله اول این نوع کارتابل وجود دارد");
                        else
                            throw new HandledException("مرحله آخر این نوع کارتابل وجود دارد");
                    }
                }
                if (model.IsFirstState)
                    model.Order = -1;
                else
                    model.Order = 255;
            }
            return await base.Save(model, changeDate);
        }
    }

    public class CartableRelationBiz : RepositoryBase<Models.CartableRelation>
    {
        public static readonly CartableRelationBiz Instance = new CartableRelationBiz();
        public JqGrid.PagedList<CartableLogVM> GetAllPagedListVm(GridSettings grid)
        {
            using (var context = new Context())
                return context.CartableRelation.Select(x => new CartableLogVM() { CartableType = x.FromCartable.CartableType, Id = x.Id, CreatedDate = x.CreatedDate, From = x.FromCartable.Name, To = x.ToCartable.Name }).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<CartableRelation> Save(CartableRelation model, bool changeDate = true)
        {
            if (model.From == model.To)
            {
                throw new HandledException(" مرحله اول و مرحله آخر نمی توانند برابر باشند");
            }
            using (var ctx = new Context())
            {
                if (ctx.Cartable.Where(x => x.Id == model.To).Select(x => x.CartableType).First() != ctx.Cartable.Where(x => x.Id == model.From).Select(x => x.CartableType).First())
                {
                    throw new HandledException(" نوع کارتابل ها باید باهم برابر باشد");
                }
                if (ctx.CartableRelation.Count(x => x.From == model.From && x.To == model.To && x.Id != model.Id) > 0)
                {
                    throw new HandledException("  این نوع ارتباط وجود دارد");
                }
            }
            return await base.Save(model, changeDate);
        }
    }

    public class CartableLogBiz : RepositoryBase<Models.CartableLog>
    {
        public static readonly CartableLogBiz Instance = new CartableLogBiz();
        public JqGrid.PagedList<CartableLogVM> GetAllPagedListByEntityId(GridSettings grid, int entityId, CartableType cartableType)
        {
            using (var context = new Context())
                return context.CartableLog.Where(x => x.EntityId == entityId && x.CartableType == cartableType).Select(x => new CartableLogVM() { Id = x.Id, Description = x.Description, CreatedDate = x.CreatedDate, From = x.FromCartable.Name, To = x.ToCartable.Name, User = x.User.Name + " " + x.User.Family }).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<CartableLog> Save(CartableLog model, bool changeDate = true)
        {
            using (var ctx = new Context())
            {
                Cartable from = ctx.Cartable.FirstOrDefault(x => x.Id == model.From);
                if (from == null)
                {
                    throw new HandledException("مرحله جاری کارتابل معتبر نیست دارد");
                }
                Cartable to = ctx.Cartable.FirstOrDefault(x => x.Id == model.To);
                if (to == null)
                {
                    throw new HandledException("مرحله بعدی کارتابل معتبر نیست دارد");
                }
                switch (from.CartableType)
                {
                    case CartableType.Suggestion:
                        Suggestion obj = ctx.Suggestion.FirstOrDefault(x => x.Id == model.EntityId);
                        if (obj == null)
                        {
                            throw new HandledException("رکورد پیشنهادات ها پیدا نشد");
                        }
                        if (obj.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        obj.CartableId = model.To;
                        break;
                    case CartableType.Idea:
                        Idea idea = ctx.Idea.FirstOrDefault(x => x.Id == model.EntityId);
                        if (idea == null)
                        {
                            throw new HandledException("رکورد ایده ها پیدا نشد");
                        }
                        if (idea.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        idea.CartableId = model.To;
                        break;
                    case CartableType.ContactUs:
                        ContactUs contactUs = ctx.ContactUs.FirstOrDefault(x => x.Id == model.EntityId);
                        if (contactUs == null)
                        {
                            throw new HandledException("رکورد پیشنهادات ها پیدا نشد");
                        }
                        if (contactUs.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        contactUs.CartableId = model.To;
                        break;
                    case CartableType.Complaint:
                        Complaint complaint = ctx.Complaint.FirstOrDefault(x => x.Id == model.EntityId);
                        if (complaint == null)
                        {
                            throw new HandledException("رکورد پیشنهادات ها پیدا نشد");
                        }
                        if (complaint.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        complaint.CartableId = model.To;
                        break;
                    case CartableType.Resturant:
                        Resturant resturant = ctx.Resturant.FirstOrDefault(x => x.Id == model.EntityId);
                        if (resturant == null)
                        {
                            throw new HandledException("رکورد مرکزپذیرایی پیدا نشد");
                        }
                        if (resturant.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        if (!to.IsFirstState)// && !from.IsFirstState
                        {
                            List<CheckListTypeCartable> res = await CheckListTypeCartableBiz.Instance.GetAll(x => x.CartableId == model.From && x.ResturantId == model.EntityId);
                            if (res.Count == 0)
                            {
                                throw new HandledException("لطفا موارد چک لیست را بررسی کنید");
                            }
                            if (res.Any(x => x.Accepted == false))
                            {
                                throw new HandledException(" موارد چک لیست تایید نشده وجود دارد");
                            }
                            List<int> checkListIsReq = await ctx.CheckListType.Where(x => x.IsReq).Select(x => x.Id).ToListAsync();

                            //var uploaded = ctx.ResturantCheckList.Where(x => x.ResturantId == resturant.Id && checkListIsReq.Contains(x.CheckListId)).Select(x => x.CheckListType).ToList();

                            foreach (var reqId in checkListIsReq)
                            {
                                if (!res.Any(x => x.CheckListId == reqId))
                                {
                                    throw new HandledException("مدارک ضروری پیوست نشده است");
                                }
                            }
                            if (!(await Biz.ResturantBiz.Instance.ResturantPersonelValidation(model.EntityId)))
                            {
                                throw new HandledException("حداقل بایستی اطلاعات دو نفر از پرسنل یا مدیران ثبت گردد");
                            }
                        }

                        resturant.CartableId = model.To;
                        break;
                    case CartableType.Shekayat:
                        Shekayat shekayat = ctx.Shekayat.FirstOrDefault(x => x.Id == model.EntityId);
                        if (shekayat == null)
                        {
                            throw new HandledException("رکورد پیشنهادات ها پیدا نشد");
                        }
                        if (shekayat.CartableId != model.From)
                        {
                            throw new HandledException("مرحله جاری کارتابل با مرحله جاری یکسان نیست");
                        }
                        shekayat.CartableId = model.To;
                        break;
                }
                await ctx.SaveChangesAsync();
                return await base.Save(model, changeDate);
            }

        }
    }

    public class CartableUserAccessBiz : RepositoryBase<Models.CartableUserAccess>
    {
        public static readonly CartableUserAccessBiz Instance = new CartableUserAccessBiz();
        public JqGrid.PagedList<CartableUserAccessVm> GetAllPagedListVm(GridSettings grid)
        {
            using (var context = new Context())
                return context.CartableUserAccess.Select(x => new CartableUserAccessVm() { CartableType = x.Cartable.CartableType, Id = x.Id, CreatedDate = x.CreatedDate, Cartable = x.Cartable.Name, User = x.User.Name + " " + x.User.Family }).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<CartableUserAccess> Save(CartableUserAccess model, bool changeDate = true)
        {
            using (var ctx = new Context())
            {
                if (ctx.CartableUserAccess.Count(x => x.UserId == model.UserId && x.CartableId == model.CartableId && x.Id != model.Id) > 0)
                {
                    throw new HandledException("  این نوع دسترسی وجود دارد");
                }
            }
            return await base.Save(model, changeDate);
        }
        public async Task<List<Cartable>> GetAllAccess(int userId, CartableType type)
        {
            if (userId <= 0)
            {
                return new List<Cartable>() { new Cartable() { Name = "پیگیری" } };
            }
            using (var ctx = new Context())
            {
                List<Cartable> ret = await ctx.Cartable.Where(x => x.Users.Any(z => z.UserId == userId) && type == x.CartableType).Include(x => x.From).ToListAsync();
                foreach (var item in ret)
                {
                    foreach (var from in item.From)
                    {
                        if (from.ToCartable != null)
                        {
                            from.ToName = from.ToCartable.Name;
                        }
                        else
                        {
                            from.ToName = ctx.Cartable.Where(x => x.Id == from.To).Select(x => x.Name).First();
                        }

                        from.ToCartable = null;
                        from.FromCartable = null;
                    }
                }
                ret.Insert(0, new Cartable() { Name = "پیگیری" });
                return ret;
            }
        }
    }
}