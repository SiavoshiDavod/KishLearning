using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using SurveyWeb.Models;
using SurveyWeb.Models.JobBoard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class JobRequestBiz : RepositoryBase<Models.JobBoard.JobCategory>
    {
        public static readonly JobRequestBiz Instance = new JobRequestBiz();

        public virtual JobRequest Add(JobRequest model)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.JobRequests.FirstOrDefault(x => x.UserID == model.UserID && x.JobPositionId == model.JobPositionId);
                if (item != null)
                    throw new Exception("شما قبلا برای این موقعیت شغلی رزومه ارسال کرده اید.");
                ctx.JobRequests.Add(model);
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual JobRequest Find(int id)
        {
            using (var ctx = new Models.Context())
                return ctx.JobRequests.FirstOrDefault(x => x.Id == id);
        }

        public virtual void ChangeStatus(int id,JobRequest.StatusType status)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.JobRequests.FirstOrDefault(x => x.Id == id);
                item.Status = status;
                ctx.SaveChanges();
            }
        }

        public virtual JobRequestWrapper FindWrapper(int id)
        {
            using (var ctx = new Models.Context())
            {
                var item = (from a in ctx.JobRequests
                            join b in ctx.JobPositions on a.JobPositionId equals b.Id
                            join c in ctx.User on a.UserID equals c.Id
                            where a.Id == id
                            select new JobRequestWrapper
                            {
                                FirstName = c.Name,
                                JobPositionID = a.JobPositionId,
                                JobPositionTitle = b.Title,
                                LastName = c.Family,
                                Company = b.Companyname,
                                UserID = c.Id,
                                UserName = c.UserName,
                                Id = a.Id,
                                Phone = c.Mobile,
                                Status =(JobRequestWrapper.StatusType) a.Status
                            }).FirstOrDefault();
                item.StatusName = item.Status.GetAttribute<DisplayAttribute>().Name;

                return item;
            }

        }

        public virtual JobRequest AppliedBefore(int jobPositionId, int userId)
        {
            using (var ctx = new Models.Context())
                return ctx.JobRequests.FirstOrDefault(x => x.JobPositionId == jobPositionId && x.UserID == userId);
        }


        public virtual List<JobRequest> FindAll()
        {
            using (var ctx = new Models.Context())
                return ctx.JobRequests.ToList();
        }

        public virtual List<JobPositionWrapper> FindByUserID(int id)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.JobRequests
                            join b in ctx.JobPositions on a.JobPositionId equals b.Id
                            join c in ctx.JobCategories on b.JobCategoryID equals c.Id
                            join d in ctx.User on a.UserID equals d.Id
                            where a.UserID == id
                            select new JobPositionWrapper
                            {
                                ID = b.Id,
                                CooperationType = (JobPositionWrapper.EnumCooperationType)b.CooperationType,
                                Description = b.Description,
                                Gender = (JobPositionWrapper.EnumGenderType)b.Gender,
                                JobCategoryID = b.JobCategoryID,
                                JobCategoryName = c.Title,
                                Location = b.Location,
                                MilitaryServiceStatus = (JobPositionWrapper.EnumMilitaryServiceType)b.MilitaryServiceStatus,
                                RequiredSkills = b.RequiredSkills,
                                SalaryFrom = b.SalaryFrom,
                                SalaryTo = b.SalaryTo,
                                Title = b.Title,
                                UserID = b.UserID,
                                UserName = d.UserName,
                                CompanyName = b.Companyname,
                                WorkExperience = (JobPositionWrapper.EnumWorkExperienceType)b.WorkExperience,
                                RequestStatus = a.Status
                            }).ToList();
                foreach (var item in list)
                {
                    item.RequestStatusName = item.RequestStatus.GetAttribute<DisplayAttribute>().Name;
                }
                return list;
            }
        }

        public virtual List<JobPositionWrapper> FindByJobPositionID(int id)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.JobRequests
                            join b in ctx.JobPositions on a.JobPositionId equals b.Id
                            join c in ctx.JobCategories on b.JobCategoryID equals c.Id
                            join d in ctx.JobRequests on b.Id equals d.JobPositionId
                            join e in ctx.User on a.UserID equals e.Id
                            where a.UserID == id
                            select new JobPositionWrapper
                            {
                                ID = b.Id,
                                CooperationType = (JobPositionWrapper.EnumCooperationType)b.CooperationType,
                                Description = b.Description,
                                Gender = (JobPositionWrapper.EnumGenderType)b.Gender,
                                JobCategoryID = b.JobCategoryID,
                                JobCategoryName = c.Title,
                                Location = b.Location,
                                MilitaryServiceStatus = (JobPositionWrapper.EnumMilitaryServiceType)b.MilitaryServiceStatus,
                                RequiredSkills = b.RequiredSkills,
                                SalaryFrom = b.SalaryFrom,
                                SalaryTo = b.SalaryTo,
                                Title = b.Title,
                                UserID = b.UserID,
                                UserName = e.UserName,
                                CompanyName = b.Companyname,
                                WorkExperience = (JobPositionWrapper.EnumWorkExperienceType)b.WorkExperience,
                            }).ToList();
                return list;
            }
        }

        public virtual void Remove(int id)
        {
            using (var ctx = new Models.Context())
            {
                ctx.JobRequests.Remove(ctx.JobRequests.FirstOrDefault(x => x.Id == id));
                ctx.SaveChanges();
            }
        }

        public virtual JqGrid.PagedList<JobRequestWrapper> LoadPagedList(GridSettings grid, int currentUserID)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.JobRequests
                            join b in ctx.JobPositions on a.JobPositionId equals b.Id
                            join c in ctx.User on a.UserID equals c.Id
                            where b.UserID == currentUserID
                            select new JobRequestWrapper
                            {
                                FirstName = c.Name,
                                JobPositionID = a.JobPositionId,
                                JobPositionTitle = b.Title,
                                LastName = c.Family,
                                Company = b.Companyname,
                                UserID = c.Id,
                                UserName = c.UserName,
                                Id = a.Id,
                                Phone = c.Mobile,
                                Status = (JobRequestWrapper.StatusType)a.Status,
                            });
               
                return list.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
    }
}