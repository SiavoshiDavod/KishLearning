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
    public class EmployeeProfileBiz : RepositoryBase<Models.JobBoard.EmployeeProfile>
    {
        public static readonly EmployeeProfileBiz Instance = new EmployeeProfileBiz();

        public virtual EmployeeProfile Add(EmployeeProfile model)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.EmployeeProfiles.FirstOrDefault(x => x.Id == model.Id);
                if (item != null)
                {
                    item.Address = model.Address;
                    item.BirthYear = model.BirthYear;
                    item.Email = model.Email;
                    item.Languages = model.Languages;
                    item.MaritalStatus = model.MaritalStatus;
                    item.MilitaryStatus = model.MilitaryStatus;
                    item.Phone = model.Phone;
                    item.ProvinceOfResidence = model.ProvinceOfResidence;
                    item.Skills = model.Skills;
                    item.Specialty = model.Specialty;
                    item.UpdateDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(model.ProfileImageURI))
                        item.ProfileImageURI = model.ProfileImageURI;
                    if (model.ResumeFile != null)
                        item.ResumeFile = model.ResumeFile;
                }
                else
                {
                    model.CreatedDate = DateTime.Now;
                    ctx.EmployeeProfiles.Add(model);
                }
                ctx.SaveChanges();
                return model;
            }
        }

        public virtual EmployeeProfileWrapper Find(int id)
        {
            using (var ctx = new Models.Context())
            {
                var item = (from a in ctx.EmployeeProfiles.Where(x => x.Id == id)
                            join b in ctx.User on a.UserID equals b.Id
                            select new EmployeeProfileWrapper
                            {
                                Id = a.Id,
                                AboutMe = a.AboutMe,
                                Address = a.Address,
                                BirthYear = a.BirthYear,
                                Email = a.Email,
                                Gender = (EmployeeProfileWrapper.EnumEmployeeGenderType)a.Gender,
                                IsVerified = a.IsVerified,
                                Languages = a.Languages,
                                MaritalStatus = (EmployeeProfileWrapper.EnumEmployeeMaritalStatusType)a.MaritalStatus,
                                Phone = a.Phone,
                                ProfileImageURI = a.ProfileImageURI,
                                MilitaryStatus = (EmployeeProfileWrapper.EnumEmployeeMilitaryServiceType)a.MilitaryStatus,
                                ProvinceOfResidence = a.ProvinceOfResidence,
                                ResumeFile = a.ResumeFile,
                                Skills = a.Skills,
                                Specialty = a.Specialty,
                                UserID = a.UserID,
                                Username = b.UserName
                            }).FirstOrDefault();
                if (item != null)
                {
                    item.MilitaryStatusTitle = item.MilitaryStatus.GetAttribute<DisplayAttribute>().Name;
                    item.MaritalStatusTitle = item.MaritalStatus.GetAttribute<DisplayAttribute>().Name;
                    item.GenderTitle = item.Gender.GetAttribute<DisplayAttribute>().Name;

                    item.EducationalBackground = ctx.EducationalBackgrounds.Where(x => x.UserID == item.UserID).ToList();
                    item.WorkExperiences = ctx.WorkExperiences.Where(x => x.UserID == item.UserID).ToList();
                }
                return item;
            }
        }

        public virtual EmployeeProfile FindByUserId(int id)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.EmployeeProfiles.FirstOrDefault(x => x.UserID == id);
                if (item != null)
                {
                    item.MilitaryStatusTitle = item.MilitaryStatus.GetAttribute<DisplayAttribute>().Name;
                    item.MaritalStatusTitle = item.MaritalStatus.GetAttribute<DisplayAttribute>().Name;
                    item.GenderTitle = item.Gender.GetAttribute<DisplayAttribute>().Name;
                }
                return item;
            }
        }

        public virtual List<EmployeeProfile> FindAll()
        {
            using (var ctx = new Models.Context())
                return ctx.EmployeeProfiles.ToList();
        }

        public void AdminVerification(int id)
        {
            using (var ctx = new Models.Context())
            {
                ctx.EmployeeProfiles.FirstOrDefault(x => x.Id == id).IsVerified = true;
                ctx.SaveChanges();
            }
        }

        public virtual JqGrid.PagedList<EmployeeProfileWrapper> GetAllNotVerifiedPagedList(GridSettings grid)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.EmployeeProfiles.Where(x => x.IsVerified == false)
                            join b in ctx.User on a.UserID equals b.Id
                            select new EmployeeProfileWrapper
                            {
                                Id = a.Id,
                                AboutMe = a.AboutMe,
                                Address = a.Address,
                                BirthYear = a.BirthYear,
                                Email = a.Email,
                                Gender = (EmployeeProfileWrapper.EnumEmployeeGenderType)a.Gender,
                                IsVerified = a.IsVerified,
                                Languages = a.Languages,
                                MaritalStatus = (EmployeeProfileWrapper.EnumEmployeeMaritalStatusType)a.MaritalStatus,
                                Phone = a.Phone,
                                ProfileImageURI = a.ProfileImageURI,
                                MilitaryStatus = (EmployeeProfileWrapper.EnumEmployeeMilitaryServiceType)a.MilitaryStatus,
                                ProvinceOfResidence = a.ProvinceOfResidence,
                                ResumeFile = a.ResumeFile,
                                Skills = a.Skills,
                                Specialty = a.Specialty,
                                UserID = a.UserID,
                                Username = b.UserName
                            });

                var pagedList = list.FilterAndSortJqGrid(grid).ToPagedList(grid);
                if (pagedList.Count() > 0)
                {
                    foreach (var item in pagedList)
                    {
                        item.MilitaryStatusTitle = item.MilitaryStatus.GetAttribute<DisplayAttribute>().Name;
                        item.MaritalStatusTitle = item.MaritalStatus.GetAttribute<DisplayAttribute>().Name;
                        item.GenderTitle = item.Gender.GetAttribute<DisplayAttribute>().Name;
                    }
                }
                return pagedList;
            }
        }

        public virtual JqGrid.PagedList<EmployeeProfileWrapper> GetAllVerifiedPagedList(GridSettings grid)
        {
            using (var ctx = new Models.Context())
            {
                var list = (from a in ctx.EmployeeProfiles.Where(x => x.IsVerified == true)
                            join b in ctx.User on a.UserID equals b.Id
                            select new EmployeeProfileWrapper
                            {
                                Id = a.Id,
                                AboutMe = a.AboutMe,
                                Address = a.Address,
                                BirthYear = a.BirthYear,
                                Email = a.Email,
                                Gender = (EmployeeProfileWrapper.EnumEmployeeGenderType)a.Gender,
                                IsVerified = a.IsVerified,
                                Languages = a.Languages,
                                MaritalStatus = (EmployeeProfileWrapper.EnumEmployeeMaritalStatusType)a.MaritalStatus,
                                Phone = a.Phone,
                                ProfileImageURI = a.ProfileImageURI,
                                MilitaryStatus = (EmployeeProfileWrapper.EnumEmployeeMilitaryServiceType)a.MilitaryStatus,
                                ProvinceOfResidence = a.ProvinceOfResidence,
                                ResumeFile = a.ResumeFile,
                                Skills = a.Skills,
                                Specialty = a.Specialty,
                                UserID = a.UserID,
                                Username = b.UserName
                            });

                var pagedList = list.FilterAndSortJqGrid(grid).ToPagedList(grid);
                if (pagedList.Count() > 0)
                {
                    foreach (var item in pagedList)
                    {
                        item.MilitaryStatusTitle = item.MilitaryStatus.GetAttribute<DisplayAttribute>().Name;
                        item.MaritalStatusTitle = item.MaritalStatus.GetAttribute<DisplayAttribute>().Name;
                        item.GenderTitle = item.Gender.GetAttribute<DisplayAttribute>().Name;
                    }
                }
                return pagedList;
            }
        }

        public void AddWorkExperience(WorkExperience item)
        {
            using (var ctx = new Models.Context())
            {
                if (item.FromDate == null || item.ToDate == null || item.CompanyName == null || item.Position == null)
                    throw new Exception("لطفا اطلاعات خواسته شده را وارد نمایید");
                item.CreatedDate = DateTime.Now;
                ctx.WorkExperiences.Add(item);
                ctx.SaveChanges();
            }
        }

        public void RemoveWorkExperience(int id)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.WorkExperiences.FirstOrDefault(x => x.Id == id);
                ctx.WorkExperiences.Remove(item);
                ctx.SaveChanges();
            }
        }

        public virtual JqGrid.PagedList<WorkExperience> GetWorkExperiencePagedList(GridSettings grid, int userID)
        {
            using (var ctx = new Models.Context())
            {
                var items = ctx.WorkExperiences.Where(x => x.UserID == userID);
                return items.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }

        public void AddEducationalBackground(EducationalBackground item)
        {
            using (var ctx = new Models.Context())
            {
                if (item.FromDate == null || item.ToDate == null || item.InstituteName == null || item.Field == null)
                    throw new Exception("لطفا اطلاعات خواسته شده را وارد نمایید");
                item.CreatedDate = DateTime.Now;
                ctx.EducationalBackgrounds.Add(item);
                ctx.SaveChanges();
            }
        }

        public void RemoveEducationalBackground(int id)
        {
            using (var ctx = new Models.Context())
            {
                var item = ctx.EducationalBackgrounds.FirstOrDefault(x => x.Id == id);
                ctx.EducationalBackgrounds.Remove(item);
                ctx.SaveChanges();
            }
        }


        public virtual JqGrid.PagedList<EducationalBackground> GetEducationalBackgroundPagedList(GridSettings grid, int userID)
        {
            using (var ctx = new Models.Context())
            {
                var items = ctx.EducationalBackgrounds.Where(x => x.UserID == userID);
                return items.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
    }
}