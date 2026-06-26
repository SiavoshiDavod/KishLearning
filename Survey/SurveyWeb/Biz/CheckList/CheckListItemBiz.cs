using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using SurveyWeb.Models.CheckList;
using SurveyWeb.Models.wrapper;

namespace SurveyWeb.Biz.CheckList
{
    public class CheckListItemBiz : RepositoryBase<CheckListItem>
    {
        public static readonly CheckListItemBiz Instance = new CheckListItemBiz();
        public PagedList<CheckListItemWrapper> GetItemsGrid(GridSettings grid,int checkListId) {
            using (var Context = new Context())
            {
                var list = Context.CheckListItems.Include("CheckListGroup").Include("CheckList").Where(a => a.CheckListId == checkListId).
                Select(a => new CheckListItemWrapper()
                {
                    Id = a.Id,
                    CheckListGroupId = a != null ? a.CheckListGroupId : 0,
                    CheckListGroupName = a != null ? a.CheckListGroup.Name : string.Empty,
                    Name = a.Name ,
                    CheckListName = a != null ? a.CheckList.Name : string.Empty,
                    CheckListId=a.CheckListId,
                    CheckListItemTypeName = a == null ? "" : (int)a.CheckListItemType == 1 ? "بله یا خیر" : (int)a.CheckListItemType == 2 ? "خوب متوسط بد" : (int)a.CheckListItemType == 3 ? "دارد ندارد" : ""
                }).FilterAndSortJqGrid(grid).ToPagedList(grid);

                return list;
            }
        }
        public CheckListItemWrapper FindFull(int checkListId)
        {
            using (var Context = new Context())
            {
                var item = Context.CheckListItems.Include("CheckListGroup").Where(a => a.CheckListId == checkListId).
                Select(a => new CheckListItemWrapper()
                {
                    Id = a.Id,
                    CheckListGroupId = a != null ? a.CheckListGroupId : 0,
                    CheckListGroupName = a != null ? a.CheckListGroup.Name : string.Empty,
                    Name = a.Name,
                    CheckListItemTypeName = a == null ? "" : (int)a.CheckListItemType == 1 ? "بله یا خیر" : (int)a.CheckListItemType == 2 ? "خوب متوسط بد" : (int)a.CheckListItemType == 3 ? "دارد ندارد" : ""
                }).FirstOrDefault();

                return item;
            }
        }
    }
    
}