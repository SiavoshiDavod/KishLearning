using MVC.Controls;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Biz
{
    public class PaperBiz : RepositoryBase<SenakLearn.Models.Paper>
    {
        public static readonly PaperBiz Instance = new PaperBiz();

        public Tuple<List<Paper>, int> GetPapers(string title, string titleE, string titleF, int groupId, int qualityId, int universityId, int journalId, int trendId, int fieldId, int publisherId, int skip = 0, int take = 10)
        {
            using (var context = new SWEntities())
            {
                var query = context.Paper.Where(x => (titleE == null || titleE == x.Title) && (titleF == null || titleF == x.TitleF)
            // &&(string.IsNullOrWhiteSpace(title)||x.Keyword.Contains(title))
            && (qualityId == 0 || qualityId == x.TranslateQualityId)
            && (universityId == 0 || universityId == x.UniversityId)
            && (journalId == 0 || journalId == x.JournalId)
            && (trendId == 0 || ("," + x.TrendIds + ",").Contains("," + trendId + ","))
            && (fieldId == 0 || fieldId == x.FieldId)
            && (publisherId == 0 || publisherId == x.PublisherId)
            && (groupId == 0 || groupId == x.GroupId)
            );
                var list = query.OrderByDescending(x => x.Id).Take(take).Skip(skip).ToList();
                var count = query.Count();
                return new Tuple<List<Paper>, int>(list, count);
            }
        }

        public override int Remove(int id)
        {
            using (var context = new SWEntities())
            {
                Paper result = context.Set<Paper>().Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    return 0;
                }
                var groupId = result.GroupId;
                context.Set<Paper>().Remove(result);
                context.SaveChanges();

                return groupId;
            }
        }
    }
    public class PaperTranslateQualityBiz : RepositoryBase<SenakLearn.Models.PaperTranslateQuality>
    {
        public static readonly PaperTranslateQualityBiz Instance = new PaperTranslateQualityBiz();
        public override bool Save(PaperTranslateQuality model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperTranslateQuality.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperTranslateQuality>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class PaperUniversityBiz : RepositoryBase<SenakLearn.Models.PaperUniversity>
    {
        public static readonly PaperUniversityBiz Instance = new PaperUniversityBiz();
        public override bool Save(PaperUniversity model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperUniversity.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperUniversity>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class PaperJournalBiz : RepositoryBase<SenakLearn.Models.PaperJournal>
    {
        public static readonly PaperJournalBiz Instance = new PaperJournalBiz();
        public override bool Save(PaperJournal model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperJournal.Any(x => (model.Id == 0 || x.Id != model.Id) && (x.DropDownTitle == model.DropDownTitle || x.DropDownTitleE == model.DropDownTitleE)))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperJournal>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class PaperPublisherBiz : RepositoryBase<SenakLearn.Models.PaperPublisher>
    {
        public static readonly PaperPublisherBiz Instance = new PaperPublisherBiz();
        public override bool Save(PaperPublisher model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperPublisher.Any(x => (model.Id == 0 || x.Id != model.Id) && (x.DropDownTitle == model.DropDownTitle || x.DropDownTitleE == model.DropDownTitleE)))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperPublisher>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class PaperTrendBiz : RepositoryBase<SenakLearn.Models.PaperTrend>
    {
        public static readonly PaperTrendBiz Instance = new PaperTrendBiz();
        public override bool Save(PaperTrend model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperTrend.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperTrend>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    public class PaperFieldBiz : RepositoryBase<SenakLearn.Models.PaperField>
    {
        public static readonly PaperFieldBiz Instance = new PaperFieldBiz();
        public override bool Save(PaperField model, bool changeDate = true)
        {
            using (var context = new SWEntities())
            {
                if (context.PaperField.Any(x => (model.Id == 0 || x.Id != model.Id) && x.DropDownTitle == model.DropDownTitle))
                {
                    throw new Exception("عنوان وارد شده تکراریست");
                }
                return base.Save(model, changeDate);
            }
        }
        public List<SelectListItem> DropDown()
        {
            using (var context = new SWEntities())
                return context.Set<PaperField>().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
        }
    }
    //public class PaperBiz : RepositoryBase<SenakLearn.Models.Paper>
    //{
    //    public static readonly PaperBiz Instance = new PaperBiz();
    //}
}