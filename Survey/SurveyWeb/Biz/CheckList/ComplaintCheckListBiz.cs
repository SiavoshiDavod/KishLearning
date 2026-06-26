using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SurveyWeb.JqGrid;
using SurveyWeb.Models.CheckList;
using SurveyWeb.Models.wrapper;
using SurveyWeb.Models;
using SurveyWeb.JqGrid.Common;

namespace SurveyWeb.Biz.CheckList
{
    public class ComplaintCheckListBiz : RepositoryBase<ComplaintCheckList>
    {
        public static readonly ComplaintCheckListBiz Instance = new ComplaintCheckListBiz();
        public PagedList<ComplaintCheckListWrapper> GetItemsGrid(GridSettings grid)
        {
            using (var Context = new Context())
            {
                var list = Context.ComplaintCheckLists.Include("Resturant").Include("CheckList").Include("User").
                Select(a => new ComplaintCheckListWrapper()
                {
                    Id = a.Id,
                    Descript = a.Descript,
                    CheckListName = a.CheckList.Name,
                    CheckListId = a.CheckListId,
                    ResturantId = a.ResturantId,
                    ResturantName= a.Resturant.Name,
                    ModirName= a.Resturant.Manager,
                    ComplaintDatePersian= a.ComplaintDatePersian,
                    ComplaintTimePersian= a.ComplaintTimePersian,
                    UserComplaintName = a.UserComplaint.UserName,
                    UserComplaintId=a.UserComplaintId
                }).FilterAndSortJqGrid(grid).ToPagedList(grid);

                return list;
            }
        }
        public ComplaintCheckListWrapper FindFull(int id)
        {
            using (var Context = new Context())
            {
                var item = Context.ComplaintCheckLists.Include("Resturant").Include("CheckList").Include("User").Where(a => a.Id == id).
                Select(a => new ComplaintCheckListWrapper()
                {
                    Id = a.Id,
                    Descript = a.Descript,
                    CheckListName = a.CheckList.Name,
                    CheckListId = a.CheckListId,
                    ResturantId = a.ResturantId,
                    ResturantName = a.Resturant.Name,
                    ModirName = a.Resturant.Manager,
                    ComplaintDatePersian = a.ComplaintDatePersian,
                    UserComplaintName = a.UserComplaint.UserName,
                    UserComplaintId = a.UserComplaintId
                }).FirstOrDefault();

                return item;
            }
        }
    }
}