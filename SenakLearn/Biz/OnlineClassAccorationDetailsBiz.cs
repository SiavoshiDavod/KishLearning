using System.Linq;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using SenakLearn.JqGrid.Common;

namespace SenakLearn.Biz
{
    public class OnlineClassAccorationDetailsBiz : RepositoryBaseParentChild<SenakLearn.Models.OnlineClassAccorationDetails>
    {
        public static readonly OnlineClassAccorationDetailsBiz Instance = new OnlineClassAccorationDetailsBiz();
        public  PagedList<OnlineClassAccorationDetailsViewModel> GetAllPagedListToViewModel(GridSettings grid)
        {
            using (SWEntities context =new SWEntities())
            {
               return context.OnlineClassAccorationDetails.Select(x=>new OnlineClassAccorationDetailsViewModel {Id=x.Id,Description=x.Description,Order=x.Order,Parent=x.Parent.Description,OnlineClassAccoration=x.OnlineClassAccoration.Name } ).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
            
        }
    }
}