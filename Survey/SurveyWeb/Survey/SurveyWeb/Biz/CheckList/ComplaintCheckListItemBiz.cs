using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.CheckList;
using SurveyWeb.Models.wrapper;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;
using System.Threading.Tasks;

namespace SurveyWeb.Biz.CheckList
{
    public class ComplaintCheckListItemBiz : RepositoryBase<ComplaintCheckListItem>
    {
        public static readonly ComplaintCheckListItemBiz Instance = new ComplaintCheckListItemBiz();
        public PagedList<ComplaintCheckListItemWrapper> GetItemsGrid(GridSettings grid, int complaintCheckListId)
        {
            using (var Context = new Context())
            {
                var comCheckList = ComplaintCheckListBiz.Instance.FindFull(complaintCheckListId);
                int checkListId = comCheckList.CheckListId;
                var list = (from i in Context.CheckListItems
                            join g in Context.CheckListGroups on i.CheckListGroupId equals g.Id
                            join ci in Context.ComplaintCheckListItems on i.Id equals ci.CheckListItemId into comListItem
                            from ci in comListItem.DefaultIfEmpty()
                            where (ci == null || ci.ComplaintCheckListId == complaintCheckListId) && i.CheckListId == checkListId
                            select
                 new ComplaintCheckListItemWrapper()
                 {
                     Id = i.Id.ToString(),
                     ComplaintCheckListItemId = ci != null ? ci.Id : 0,
                     ComplaintCheckListId=complaintCheckListId,
                     CheckListItemName = i.Name,
                     CheckListItemGroupName = g.Name,
                     CheckListItemId = i.Id,
                     IsGoodMidBad = i.CheckListItemType == CheckListItemTypeEnum.GoodMediumBad ? 1 : 0,
                     IsHasItDontHave = i.CheckListItemType == CheckListItemTypeEnum.HasItDontHave ? 1 : 0,
                     IsYesNo = i.CheckListItemType == CheckListItemTypeEnum.YesNo ? true : false,
                     ValueItem = ci != null ? ci.ValueItem : null
                 }).FilterAndSortJqGrid(grid).ToPagedList(grid);
                list.ForEach(row =>
                {
                    row.Id = Guid.NewGuid().ToString().Replace("-", "");
                });
                return list;
            }
        }
        public async Task<bool> UpdateComCheckListItem(IEnumerable<ComplaintCheckListItemWrapper> list)
        {
            using (var Context = new Context())
            {
                foreach (var row in list)
                {
                    if (row.ComplaintCheckListItemId != 0)
                    {
                        var complaintCheckListItem = Context.ComplaintCheckListItems.SingleOrDefault(a => a.Id == row.ComplaintCheckListItemId);
                        if (complaintCheckListItem == null)
                            return false;

                        complaintCheckListItem.IsGoodMidBad = row.IsGoodMidBad;
                        complaintCheckListItem.IsHasItDontHave = row.IsHasItDontHave;
                        complaintCheckListItem.IsYesNo = row.IsYesNo;
                        complaintCheckListItem.ValueItem = row.ValueItem;
                        complaintCheckListItem.UpdateDate = DateTime.Now;
                        Context.SaveChanges();
                    }
                    else
                    {
                        var complaintCheckListItem = new ComplaintCheckListItem()
                        {
                            ComplaintCheckListId = row.ComplaintCheckListId,
                            CheckListItemId = row.CheckListItemId,
                            IsGoodMidBad = row.IsGoodMidBad,
                            IsHasItDontHave = row.IsHasItDontHave,
                            IsYesNo = row.IsYesNo,
                            ValueItem = row.ValueItem,
                            CreatedDate = DateTime.Now,
                        };
                        Context.ComplaintCheckListItems.Add(complaintCheckListItem);
                        Context.SaveChanges();
                    }
                }
                return true;
            }
        }
    }
}