using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SurveyWeb.Biz
{
    public class ResturantBiz : RepositoryBase<Models.Resturant>
    {
        public static readonly ResturantBiz Instance = new ResturantBiz();
        public List<SelectListItem> ResturantSelectList()
        {
            using (var ctx = new Context())
                return ctx.Resturant.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name + (string.IsNullOrEmpty(i.Code) ? "" : " کد:" + i.Code) }).ToList();
        }


        public List<SelectListItem> CheckListType()
        {
            using (var ctx = new Context())
                return ctx.CheckListType.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle + (i.IsReq ? "(اجباری)" : "") }).ToList();
        }

        internal PagedList<ResturantMenu> GetAllResturantMenuPagedList(GridSettings grid, bool archive)
        {
            using (var ctx = new Models.Context())
                return ctx.ResturantMenu.Where(x => x.Accepted == archive).Include(x => x.Resturant).FilterAndSortJqGrid(grid).ToPagedList(grid);

        }

        public List<SelectListItem> ResturantType()
        {
            using (var ctx = new Context())
                return ctx.ResturantType.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }


        public JqGrid.PagedList<Resturant> GetAllPagedListByCartable(GridSettings grid, int cartable, bool? LastDateExtendedLicenseFilter = null)
        {
            var now = DateTime.Now;
            var nextmonth = DateTime.Now.AddDays(30);
            using (var ctx = new Context())
                return ctx.Resturant.Where(x => (cartable == 0 || x.CartableId == cartable) && (LastDateExtendedLicenseFilter == null || (LastDateExtendedLicenseFilter == true && x.LastDateExtendedLicense != null && x.LastDateExtendedLicense < nextmonth && x.LastDateExtendedLicense > now) || (LastDateExtendedLicenseFilter == false && x.LastDateExtendedLicense != null && x.LastDateExtendedLicense < now))).Include(x => x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public async Task<Resturant> FindByUserId(int userId, string include = null)
        {
            using (var ctx = new Context())
            {
                var res = ctx.Resturant.Include(x => x.Cartable).Where(x => x.UserId == userId);
                if (!string.IsNullOrEmpty(include))
                {
                    res = res.Include(include);
                }
                return await res.FirstOrDefaultAsync();
            }
        }


        private int GetFirstStepofCartable(Context ctx)
        {
            int? cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.Resturant).Select(x => x.Id).FirstOrDefault();
            if (cartable == null)
            {
                throw new HandledException("مرحله اول کارتابل تعریف نشده است");
            }
            return cartable.Value;
        }
        public async Task SendToAdmin(Resturant rest)
        {
            try
            {
                var mobiles = SiteSetting.GetSetting.Instance.Get().AdminMobiles;
                if (!string.IsNullOrEmpty(mobiles))
                {
                    var res = mobiles.Replace(";",",").Split(',');
                    foreach (var item in res)
                    {
                        await EmaiSmslBiz.Instance.SendSms(item.Trim(), $"مرکز {rest.Name} وضعیت: {rest.AddorEditnoteDesc}");
                    }
                }
            }
            catch (Exception)
            {
            }
           
        }
        public async Task<Resturant> FindandResturantChanges(int id, ResturantAddorEditnote editnote)
        {
            if (id <= 0)
                return null;
            using (var ctx = new Context())
            {
                var resturant = await ctx.Resturant.FirstOrDefaultAsync(x => x.Id == id);
                if (resturant == null)
                {
                    return null;
                }
                if (resturant.AddorEditnote == editnote || (resturant.AddorEditnote == ResturantAddorEditnote.Add && editnote != ResturantAddorEditnote.none))
                {
                    return resturant;
                }

                resturant.AddorEditnote = editnote;
                await ctx.SaveChangesAsync();
                await SendToAdmin(resturant);
                return resturant;
            }
        }
        public async Task<Resturant> SaveByCurrentUser(Resturant model)
        {
            using (var ctx = new Context())
            {
                var cartable = GetFirstStepofCartable(ctx);
                if (model.Id == 0)
                {
                    if (ctx.Resturant.Any(x => x.UserId == model.UserId))
                    {
                        throw new HandledException("شما قبلا ثبت نام کرده اید");
                    }
                    model.CartableId = cartable;
                    model.AddorEditnote = ResturantAddorEditnote.Add;
                }
                else
                {
                    if (model.CartableId != cartable)
                    {
                        throw new HandledException(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                    }
                    model.AddorEditnote = ResturantAddorEditnote.Edit;
                }

                if (!string.IsNullOrEmpty(model.Code) && ctx.Resturant.Any(x => x.Code == model.Code && x.Id != model.Id))
                {
                    throw new HandledException("این کد مرکزپذیرایی قبلا ثبت شده است");
                }

            }

            var res = await base.Save(model, true);
            await SendToAdmin(model);
            return res;
        }
        public override async Task<Resturant> Save(Resturant model, bool changeDate = true)
        {
            using (var ctx = new Context())
            {
                if (!string.IsNullOrEmpty(model.Code) && ctx.Resturant.Any(x => x.Code == model.Code && x.Id != model.Id))
                {
                    throw new HandledException("این کد مرکزپذیرایی قبلا ثبت شده است");
                }
                if (model.Id == 0)
                {
                    var cartable = GetFirstStepofCartable(ctx);
                    model.CartableId = cartable;
                }
            }

            return await base.Save(model, changeDate);
        }

        public async Task<bool> SaveResturantCheckList(ResturantCheckList model)
        {
            using (var ctx = new Context())
            {
                model.CreatedDate = DateTime.Now;
                ctx.ResturantCheckList.Add(model);
                await ctx.SaveChangesAsync();
                return true;//await context.Resturant.Where(x=>x.Id==model.ResturantId).Include(x => x.ResturantCheckList).FirstOrDefaultAsync();
            }
        }

        public async Task<bool> SaveResturantMenu(int userId, string desc)
        {
            using (var ctx = new Context())
            {
                var resturant = await ctx.Resturant.FirstOrDefaultAsync(x => x.UserId == userId);
                if (resturant == null)
                {
                    throw new HandledException("لطفا ابتدا اطلاعات مرکزپذیرایی خود را تکمیل کنید");
                }
                else
                {
                    ctx.ResturantMenu.Add(new ResturantMenu()
                    {
                        CreatedDate = DateTime.Now,
                        Description = desc,
                        ResturantId = resturant.Id
                    });
                    resturant.AddorEditnote = ResturantAddorEditnote.AddMenu;
                    await ctx.SaveChangesAsync();
                    await SendToAdmin(resturant);
                    return true;
                }
            }
        }
        public async Task<bool> AcceptResturantMenu(int Id, bool Accepted, bool Active, string desc)
        {
            using (var ctx = new Context())
            {
                var foundEntity = await ctx.ResturantMenu.FirstOrDefaultAsync(x => x.Id == Id);
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
                foundEntity.Accepted = Accepted;
                foundEntity.Active = Accepted ? Active : false;
                foundEntity.UpdateDate = DateTime.Now;
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> SetFinalPriceResturantMenu(int Id, int finalPrice)
        {
            using (var ctx = new Context())
            {
                var foundEntity = await ctx.ResturantDetailMenu.FirstOrDefaultAsync(x => x.Id == Id);
                if (foundEntity == null)
                {
                    return false;
                }
                foundEntity.FinalPrice = finalPrice;
                foundEntity.UpdateDate = DateTime.Now;
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SetNameResturantMenu(int Id, string name)
        {
            using (var ctx = new Context())
            {
                var foundEntity = await ctx.ResturantDetailMenu.FirstOrDefaultAsync(x => x.Id == Id);
                if (foundEntity == null)
                {
                    return false;
                }
                foundEntity.Name = name;
                foundEntity.UpdateDate = DateTime.Now;
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> SetDescriptionResturantMenu(int Id, string desc)
        {
            using (var ctx = new Context())
            {
                var foundEntity = await ctx.ResturantDetailMenu.FirstOrDefaultAsync(x => x.Id == Id);
                if (foundEntity == null)
                {
                    return false;
                }
                foundEntity.Description = desc;
                foundEntity.UpdateDate = DateTime.Now;
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SaveResturantMenuDetail(ResturantDetailMenu model)
        {
            using (var ctx = new Context())
            {
                if (model.Id <= 0)
                {
                    model.CreatedDate = DateTime.Now;
                    model.FinalPrice = model.NewPrice;
                    ctx.ResturantDetailMenu.Add(model);
                }
                else
                {
                    model.UpdateDate = DateTime.Now;
                    var foundEntity = await ctx.ResturantDetailMenu.Include(x => x.ResturantMenu).FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (foundEntity == null)
                    {
                        return false;
                    }
                    if (foundEntity.ResturantMenu.Accepted/*|| res.ResturantMenu.Active*/)
                    {
                        throw new HandledException(" بدلیل تایید توسط مدیر سایت، امکان ویرایش اطلاعات وجود ندارد");
                    }
                    model.CreatedDate = foundEntity.CreatedDate;
                    model.FinalPrice = foundEntity.NewPrice;

                    ctx.Entry(foundEntity).CurrentValues.SetValues(model);
                }

                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SaveResturantPersonel(ResturantPersonel model, bool isAdmin = false)
        {
            using (var ctx = new Context())
            {
                if (model.Id <= 0)
                {
                    model.CreatedDate = DateTime.Now;
                    ctx.ResturantPersonel.Add(model);
                }
                else
                {
                    var foundEntity = await ctx.ResturantPersonel.FirstOrDefaultAsync(x => x.Id == model.Id);
                    if (foundEntity == null)
                    {
                        return false;
                    }
                    //context.Entry(model).State = EntityState.Modified;
                    ctx.Entry(foundEntity).CurrentValues.SetValues(model);
                    //var cartable = GetFirstStepofCartable(ctx);
                    //if (model.CartableId != cartable)
                    //{
                    //    throw new HandledException(" در حال حاضر امکان ویرایش اطلاعات وجود ندارد");
                    //}
                }

                await ctx.SaveChangesAsync();
                return true;//await context.Resturant.Where(x => x.Id == model.ResturantId).Include(x=>x.ResturantPersonel).FirstOrDefaultAsync();
            }
        }

        public async Task<int> RemoveResturantCheckList(int id, bool isAdmin = false)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantCheckList.Include(x => x.Resturant).FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return -1;
                }
                if (!isAdmin)
                {
                    var cartable = GetFirstStepofCartable(ctx);
                    if (res.Resturant.CartableId != cartable)
                    {
                        throw new HandledException(" در حال حاضر امکان حذف اطلاعات وجود ندارد");
                    }
                }
                int resturantId = res.ResturantId;
                ctx.ResturantCheckList.Remove(res);
                await ctx.SaveChangesAsync();
                return resturantId;
            }
        }

        public async Task<int> RemoveResturantPersonel(int id, bool isAdmin = false)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonel.Include(x => x.Resturant).FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return 0;
                }
                if (!isAdmin)
                {
                    var cartable = GetFirstStepofCartable(ctx);
                    if (res.Resturant.CartableId != cartable)
                    {
                        throw new HandledException(" در حال حاضر امکان حذف اطلاعات وجود ندارد");
                    }
                }
                int resturantId = res.ResturantId;
                ctx.ResturantPersonel.Remove(res);
                await ctx.SaveChangesAsync();
                return resturantId;
            }
        }
        public async Task<int> RemoveResturantMenu(int id, bool isAdmin = false)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantMenu.Include(x => x.Resturant).Include(x => x.ResturantDetailMenus).FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return 0;
                }
                if (!isAdmin)
                {
                    var cartable = GetFirstStepofCartable(ctx);
                    if (res.Resturant.CartableId != cartable || res.Accepted)
                    {
                        throw new HandledException(" در حال حاضر امکان حذف اطلاعات وجود ندارد");
                    }
                }
                var resturantId = res.Resturant.Id;
                res.ResturantDetailMenus.Clear();
                ctx.ResturantMenu.Remove(res);
                await ctx.SaveChangesAsync();
                return resturantId;
            }
        }

        public async Task<bool> RemoveResturantDetailMenu(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantDetailMenu.Include(x => x.ResturantMenu).FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                if (res.ResturantMenu.Accepted /*|| res.ResturantMenu.Active*/)
                {
                    throw new HandledException(" بدلیل تایید توسط مدیر سایت، امکان حذف اطلاعات وجود ندارد");
                }

                ctx.ResturantDetailMenu.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RemoveResturantDetailMenuByAdmin(int id, bool isAdmin = false)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantDetailMenu.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                ctx.ResturantDetailMenu.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ResturantPersonel> FindResturantPersonel(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonel.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<bool> ResturantPersonelValidation(int resturantId)
        {
            using (var ctx = new Context())
            {
                return (await ctx.ResturantPersonel.CountAsync(x => x.ResturantId == resturantId)) >= 2;
            }
        }
        public async Task<bool> ResturantCheckListValidation(int resturantId)
        {
            using (var ctx = new Context())
            {
                List<int> checkListIsReq = await ctx.CheckListType.Where(x => x.IsReq).Select(x => x.Id).ToListAsync();
                var uploded = await ctx.ResturantCheckList.Where(x => x.ResturantId == resturantId && checkListIsReq.Contains(x.CheckListId)).Select(x => x.CheckListId).Distinct().ToListAsync();
                foreach (var item in checkListIsReq)
                {
                    if (!uploded.Any(x => x == item))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<ResturantDetailMenu> FindResturantDetailMenu(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantDetailMenu.FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task<ResturantMenu> FindResturantMenuIncludeDetail(int id, int userId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantMenu.Include(x => x.ResturantDetailMenus).FirstOrDefaultAsync(x => x.Resturant.UserId == userId && x.Id == id);
            }
        }
        public async Task<ResturantMenu> FindResturantMenuIncludeDetail(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantMenu.Include(x => x.ResturantDetailMenus).FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<ResturantCheckList> FindResturantCheckList(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantCheckList.FirstOrDefaultAsync(x => x.Id == id);
            }
        }
        public async Task<List<ResturantCheckList>> FindResturantCheckListByResturantIdandType(int id, int CheckListId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantCheckList.Where(x => x.ResturantId == id && x.CheckListId == CheckListId).ToListAsync();
            }
        }

        public async Task<List<ResturantPersonel>> FindPersonelsByResturantId(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonel.Where(x => x.ResturantId == id).ToListAsync();
            }
        }

        public async Task<List<ResturantMenu>> FindMenuByUserId(int id)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantMenu.Include(x => x.ResturantDetailMenus).Where(x => x.Resturant.UserId == id).ToListAsync();
            }
        }
        public async Task<int> GetMenuByResturantId(int id, bool accepted)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantMenu.CountAsync(x => x.ResturantId == id && x.Accepted == accepted);
            }
        }

        public async Task<List<ResturantPersonelEducation>> ResturantPersonelEducations(int personelId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonelEducation.Where(x => x.ResturantPersonelId == personelId).Include(x => x.Education).ToListAsync();
            }
        }
        public async Task<bool> SaveResturantPersonelEducation(ResturantPersonelEducation model)
        {
            using (var ctx = new Context())
            {
                ctx.ResturantPersonelEducation.Add(model);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RemoveResturantPersonelEducation(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonelEducation.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                ctx.ResturantPersonelEducation.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<ResturantPersonelJob>> ResturantPersonelJobs(int personelId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonelJob.Where(x => x.ResturantPersonelId == personelId).ToListAsync();
            }
        }
        public async Task<bool> SaveResturantPersonelJob(ResturantPersonelJob model)
        {
            using (var ctx = new Context())
            {
                ctx.ResturantPersonelJob.Add(model);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RemoveResturantPersonelJob(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonelJob.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                ctx.ResturantPersonelJob.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<ResturantPersonelLanguage>> ResturantPersonelLanguages(int personelId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonelLanguage.Where(x => x.ResturantPersonelId == personelId).ToListAsync();
            }
        }
        public async Task<bool> SaveResturantPersonelLanguage(ResturantPersonelLanguage model)
        {
            using (var ctx = new Context())
            {
                ctx.ResturantPersonelLanguage.Add(model);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RemoveResturantPersonelLanguage(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonelLanguage.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                ctx.ResturantPersonelLanguage.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<List<ResturantPersonelCourse>> ResturantPersonelCourses(int personelId)
        {
            using (var ctx = new Context())
            {
                return await ctx.ResturantPersonelCourse.Where(x => x.ResturantPersonelId == personelId).ToListAsync();
            }
        }
        public async Task<bool> SaveResturantPersonelCourse(ResturantPersonelCourse model)
        {
            using (var ctx = new Context())
            {
                ctx.ResturantPersonelCourse.Add(model);
                await ctx.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RemoveResturantPersonelCourse(int id)
        {
            using (var ctx = new Context())
            {
                var res = await ctx.ResturantPersonelCourse.FirstOrDefaultAsync(x => x.Id == id);
                if (res == null)
                {
                    return false;
                }
                ctx.ResturantPersonelCourse.Remove(res);
                await ctx.SaveChangesAsync();
                return true;
            }
        }

    }
}