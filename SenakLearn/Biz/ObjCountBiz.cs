using SenakLearn.JqGrid.Common;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SenakLearn.Models.wrapper;

namespace SenakLearn.Biz
{
    public class ObjCountBiz : RepositoryBase<SenakLearn.Models.ObjCount>
    {
        public static readonly ObjCountBiz Instance = new ObjCountBiz();

        public string GetObjName(string objType)
        {
            switch (objType)
            {
                case "Course":
                    return "دوره آموزشی";
                case "Video":
                    return "ویدیو آموزشی";
                case "Book":
                    return "مشاهده کتاب";
                case "BookDownload":
                    return "دانلود کتاب";
                case "Paper":
                    return "مشاهده مقاله";
                case "PaperDownload":
                    return "دانلود مقاله";
                case "Podcast":
                    return "پادکست";
                default:
                    return "";
            }
        }
        public  JqGrid.PagedList<ObjCountWrapper> GetAllPagedListWrapper(GridSettings grid)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                var query = "select o.Id,o.ObjName,o.ObjType,o.CreatedDate,o.UpdateDate,o.[Count],o.[ObjId],\r\n" +
                    "  case when c.Id is not null then c.[name]  when v.Id is not null then '' when b.Id is not null then b.TitleF when p.Id is not null then p.TitleF else '' end ObjTitle,\r\n" +
                    "  case when c.Id is not null then c.doc  when v.Id is not null then v.[Description] when b.Id is not null then b.Keyword when p.Id is not null then p.Keyword else '' end ObjDescript\r\n" +
                    "  from ObjCount o\r\n" +
                    "  left join learn_cours c on o.ObjType='Course' and cast(o.[ObjId] as int)=c.id\r\n" +
                    "  left join OfflineVideos v on o.ObjType='Video' and v.Id = cast(o.[ObjId] as int)\r\n" +
                    "  left join Book b on (o.ObjType='Book' or o.ObjType='BookDownload') and b.Id = cast(o.[ObjId] as int)\r\n" +
                    "  left join Papers p on (o.ObjType='Paper' or o.ObjType='PaperDownload') and p.Id = cast(o.[ObjId] as int)";

                var list = context.Database.SqlQuery<ObjCountWrapper>(query).ToList().AsQueryable() ;

                return list.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
        public override async Task<bool> SaveAsync(ObjCount model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                model.Validate();
                var item = await context.ObjCounts.FirstOrDefaultAsync(a => a.ObjType == model.ObjType && a.ObjId == model.ObjId);
                if (item == null)
                {
                    model.Count = 1;
                    model.CreatedDate = DateTime.Now;
                    model.ObjName = GetObjName(model.ObjType);
                    context.Set<ObjCount>().Add(model);
                }
                else
                {
                    item.UpdateDate = DateTime.Now;
                    item.Count++;
                    context.Entry(item).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
                return true;
            }
        }
    }
}