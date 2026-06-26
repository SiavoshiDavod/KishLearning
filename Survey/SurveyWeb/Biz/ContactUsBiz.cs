using System;
using SurveyWeb.JqGrid;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SurveyWeb.Biz
{
    public class ContactUsBiz : RepositoryBase<Models.ContactUs>
    {
        public static readonly ContactUsBiz Instance = new ContactUsBiz();

        public  JqGrid.PagedList<ContactUs> GetAllPagedListByCartable(GridSettings grid, int cartable)
        {
            using (var ctx = new Context())
                return ctx.ContactUs.Where(x => cartable == 0 || x.CartableId == cartable).Include(x => x.Cartable).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public override async Task<ContactUs> Save(ContactUs model, bool changeDate = true)
        {
            int? cartable = null;
            if (model.Id == 0)
            {
                using (var ctx = new Context())
                    cartable = ctx.Cartable.Where(x => x.IsFirstState && x.CartableType == CartableType.ContactUs).Select(x => x.Id).FirstOrDefault();
                if (cartable == null)
                {
                    throw new HandledException("مرحله اول کارتابل تعریف نشده است");
                }
                model.CartableId = cartable.Value;
            }

            return await base.Save(model, changeDate);
        }
    }
}