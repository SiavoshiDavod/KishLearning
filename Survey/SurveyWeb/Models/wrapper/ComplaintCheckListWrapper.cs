using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.wrapper
{
    public class ComplaintCheckListWrapper
    {
        public int Id { get; set; }
        public string act { get; set; }
        public int CheckListId { get; set; }
        public string CheckListName { get; set; }
        public CheckList.CheckList CheckList { get; set; }
        public int ResturantId { get; set; }
        public Resturant Resturant { get; set; }
        public string ResturantName { get; set; }
        public string ModirName { get; set; }
        public string ComplaintDatePersian { get; set; }
        public string ComplaintTimePersian { get; set; }
        public DateTime ComplaintDate { get; set; }
        public int? UserComplaintId { get; set; }
        public User UserComplaint { get; set; }
        public string UserComplaintName { get; set; }
        public string Descript { get; set; }
        public int? DayNumResolve { get; set; }
    }
}