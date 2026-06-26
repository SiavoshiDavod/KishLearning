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
    public class JobPositionBiz : RepositoryBase<Models.JobBoard.JobPosition>
    {
        public static readonly JobPositionBiz Instance = new JobPositionBiz();

        public virtual JobPosition Add(JobPosition model)
        {
            using (var ctx = new Models.Context())
            {
                //var item = ctx.jobPositions.FirstOrDefault(x => x.Title == model.Title);
                //if (item != null)
                //    throw new Exception("عنوان دسته بندی تکراری است.");
                ctx.JobPositions.Add(model);
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual JobPosition Find(int id)
        {
            using (var ctx = new Models.Context())
                return ctx.JobPositions.FirstOrDefault(x => x.Id == id);
        }

        public virtual List<JobPositionWrapper> FindAll(bool isPublic = false)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.JobCategories
                            join b in ctx.JobPositions on a.Id equals b.JobCategoryID
                            join c in ctx.User on b.UserID equals c.Id
                            where (!isPublic || b.IsPublic == isPublic) && b.IsVerified == true
                            select new JobPositionWrapper
                            {
                                ID = b.Id,
                                CooperationType = (JobPositionWrapper.EnumCooperationType)b.CooperationType,
                                Description = b.Description,
                                Gender = (JobPositionWrapper.EnumGenderType)b.Gender,
                                JobCategoryID = b.JobCategoryID,
                                JobCategoryName = a.Title,
                                Location = b.Location,
                                MilitaryServiceStatus = (JobPositionWrapper.EnumMilitaryServiceType)b.MilitaryServiceStatus,
                                RequiredSkills = b.RequiredSkills,
                                SalaryFrom = b.SalaryFrom,
                                SalaryTo = b.SalaryTo,
                                Title = b.Title,
                                UserID = b.UserID,
                                UserName = c.UserName,
                                CompanyName = b.Companyname,
                                WorkExperience = (JobPositionWrapper.EnumWorkExperienceType)b.WorkExperience,
                            }).ToList();

                foreach (var item in list)
                {
                    item.CooperationTypeName = item.CooperationType.GetAttribute<DisplayAttribute>().Name;
                    item.GenderName = item.Gender.GetAttribute<DisplayAttribute>().Name;
                    item.MilitaryServiceStatusName = item.MilitaryServiceStatus.GetAttribute<DisplayAttribute>().Name;
                    item.WorkExperienceName = item.WorkExperience.GetAttribute<DisplayAttribute>().Name;
                }

                return list;
            }
        }

        public virtual List<JobPositionWrapper> FindAllByUserID(int id)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.JobCategories
                            join b in ctx.JobPositions on a.Id equals b.JobCategoryID
                            join c in ctx.User on b.UserID equals c.Id
                            where (id == 0 || b.UserID == id)
                            select new JobPositionWrapper
                            {
                                ID = b.Id,
                                CooperationType = (JobPositionWrapper.EnumCooperationType)b.CooperationType,
                                Description = b.Description,
                                Gender = (JobPositionWrapper.EnumGenderType)b.Gender,
                                JobCategoryID = b.JobCategoryID,
                                JobCategoryName = a.Title,
                                Location = b.Location,
                                MilitaryServiceStatus = (JobPositionWrapper.EnumMilitaryServiceType)b.MilitaryServiceStatus,
                                RequiredSkills = b.RequiredSkills,
                                SalaryFrom = b.SalaryFrom,
                                SalaryTo = b.SalaryTo,
                                Title = b.Title,
                                UserID = b.UserID,
                                UserName = c.UserName,
                                CompanyName = b.Companyname,
                                WorkExperience = (JobPositionWrapper.EnumWorkExperienceType)b.WorkExperience,
                            }).ToList();

                foreach (var item in list)
                {
                    item.CooperationTypeName = item.CooperationType.GetAttribute<DisplayAttribute>().Name;
                    item.GenderName = item.Gender.GetAttribute<DisplayAttribute>().Name;
                    item.MilitaryServiceStatusName = item.MilitaryServiceStatus.GetAttribute<DisplayAttribute>().Name;
                    item.WorkExperienceName = item.WorkExperience.GetAttribute<DisplayAttribute>().Name;
                }

                return list;
            }
        }

        public virtual void Remove(int id)
        {
            using (var ctx = new Models.Context())
            {
                ctx.JobPositions.Remove(ctx.JobPositions.FirstOrDefault(x => x.Id == id));
                ctx.SaveChanges();
            }
        }

        public virtual JobPosition Update(JobPosition model)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.JobPositions.FirstOrDefault(x => x.Id == model.Id);
                item.Title = model.Title;
                item.Companyname = model.Companyname;
                item.CooperationType = model.CooperationType;
                item.Description = model.Description;
                item.Gender = model.Gender;
                item.JobCategoryID = model.JobCategoryID;
                item.Location = model.Location;
                item.MilitaryServiceStatus = model.MilitaryServiceStatus;
                item.RequiredSkills = model.RequiredSkills;
                item.SalaryFrom = model.SalaryFrom;
                item.SalaryTo = model.SalaryTo;
                item.WorkExperience = model.WorkExperience;
                item.IsPublic = model.IsPublic;
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual JqGrid.PagedList<JobPosition> GetAllPagedList(GridSettings grid)
        {
            using (var ctx = new Models.Context())
                return ctx.JobPositions.FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public virtual JqGrid.PagedList<JobPosition> GetAllNotVerifiedPagedList(GridSettings grid)
        {
            using (var ctx = new Models.Context())
                return ctx.JobPositions.Where(x => x.IsVerified == false).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }

        public virtual JobPositionWrapper JobPositionDetails(int id,bool isPublic = false)
        {
            using (var ctx = new Models.Context())
            {
                var item = (from a in ctx.JobCategories
                            join b in ctx.JobPositions on a.Id equals b.JobCategoryID
                            join c in ctx.User on b.UserID equals c.Id
                            where b.Id == id && (!isPublic || b.IsPublic == isPublic)  
                            select new JobPositionWrapper
                            {
                                ID = b.Id,
                                CooperationType = (JobPositionWrapper.EnumCooperationType)b.CooperationType,
                                Description = b.Description,
                                Gender = (JobPositionWrapper.EnumGenderType)b.Gender,
                                JobCategoryID = b.JobCategoryID,
                                JobCategoryName = a.Title,
                                Location = b.Location,
                                MilitaryServiceStatus = (JobPositionWrapper.EnumMilitaryServiceType)b.MilitaryServiceStatus,
                                RequiredSkills = b.RequiredSkills,
                                SalaryFrom = b.SalaryFrom,
                                SalaryTo = b.SalaryTo,
                                Title = b.Title,
                                UserID = b.UserID,
                                UserName = c.UserName,
                                CompanyName = b.Companyname,
                                WorkExperience = (JobPositionWrapper.EnumWorkExperienceType)b.WorkExperience,
                                IsPublic = b.IsPublic
                            }).FirstOrDefault();

                item.CooperationTypeName = item.CooperationType.GetAttribute<DisplayAttribute>().Name;
                item.GenderName = item.Gender.GetAttribute<DisplayAttribute>().Name;
                item.MilitaryServiceStatusName = item.MilitaryServiceStatus.GetAttribute<DisplayAttribute>().Name;
                item.WorkExperienceName = item.WorkExperience.GetAttribute<DisplayAttribute>().Name;

                return item;
            }
        }

        public void AdminVerification(int id)
        {
            using (var ctx = new Models.Context())
            {
                ctx.JobPositions.FirstOrDefault(x => x.Id == id).IsVerified = true;
                ctx.SaveChanges();
            }
        }
    }
}