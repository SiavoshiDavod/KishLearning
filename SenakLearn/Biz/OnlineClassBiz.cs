using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SenakLearn.Models;
using SenakLearn.JqGrid.Common;
using SenakLearn.JqGrid;
using AdobeConnectService;
using System;
using AdobeConnectService.AdobeConnect.Model;
using AdobeConnectSDK.Model;
using System.Linq.Expressions;

namespace SenakLearn.Biz
{
    public class OnlineClassBiz : RepositoryBase<SenakLearn.Models.OnlineClass>
    {
        public static readonly OnlineClassBiz Instance = new OnlineClassBiz();

        public bool ExistByTeacher(int id, int teacherId)
        {
            using (var context = new SWEntities())
                return context.OnlineClasses.Any(x => x.Id == id && x.learn_teacher.UserId == teacherId);
        }
        public bool ExistByUser(int id, int userId)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Any(x => x.Status == 100 && x.OnlineClassId == id && x.UserId == userId);
        }
        public List<learn_user> GetAllPaymentsForAdobe(int id)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Where(x => x.Status == 100 && x.OnlineClassId == id).Select(x => x.learn_user).ToList();//.Select(x => new OnlineClassPayment { first_name = x.learn_user.Name, last_name = x.learn_user.Family, email = x.learn_user.Email, password = x.learn_user.PassAdobe }).ToList();
        }
        public PagedList<OnlineClass> GetAllonlineClassByUserId(GridSettings grid, int userId)
        {
            using (var context = new SWEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.LazyLoadingEnabled = false;
                return context.ZarinpalPayments.Where(x => x.Status == 100 && x.UserId == userId && x.OnlineClassId != null).Select(x => x.OnlineClass).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }
        public List<OnlineClass> GetAllonlineClassByUserId(int skip, int take, int userId)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Where(x => x.Status == 100 && x.UserId == userId && x.OnlineClassId != null).Select(x => x.OnlineClass).OrderByDescending(x => x.Id).Take(take).Skip(skip).ToList();
        }
        public PagedList<OnlineClass> GetAllonlineClassByTeacherId(GridSettings grid, int teacherId)
        {
            using (var context = new SWEntities())
                return context.OnlineClasses.Where(x => x.learn_teacher.UserId == teacherId).FilterAndSortJqGrid(grid).ToPagedList(grid);
        }
        public List<learn_user> GetAllUserOfonlineClassByTeacherId(int teacherId, int onlineClassId)
        {
            using (var context = new SWEntities())
                return context.ZarinpalPayments.Where(x => x.Status == 100 && x.OnlineClassId == onlineClassId && x.OnlineClass.learn_teacher.UserId == teacherId).Select(x => x.learn_user).ToList();
        }
        public void CalculateClassTypeForJob()
        {
            using (var context = new SWEntities())
            {
                var updateList = new List<OnlineClass>();
                var list = context.OnlineClasses.Where(x => !x.IsAutoClassType).Select(x =>new { x.Id ,x.CreatedDate,x.UpdateDate,x.ClassType,x.Capacity}).ToList();
                foreach (var model in list)
                {
                   var modelClassType = Enums.OnlineClassType.Registering;
                    if (model?.UpdateDate < DateTime.Now)
                    {
                        modelClassType = Enums.OnlineClassType.End;
                    }
                    else if (model?.CreatedDate < DateTime.Now)
                    {
                        modelClassType = Enums.OnlineClassType.OnPerforming;
                    }
                    else
                    {
                        var count = context.ZarinpalPayments.Count(x => x.Status == 100 && x.OnlineClassId == model.Id);
                        if (count >= model.Capacity)
                        {
                            modelClassType = Enums.OnlineClassType.FullCapacity;
                        }
                    }
                    if (model.ClassType!= modelClassType)
                    {
                        context.Database.ExecuteSqlCommand($"update OnlineClasses set ClassType= {((int)modelClassType).ToString()} where Id={model.Id}");
                    }
                }
               
            }
        }
        public OnlineClass CalculateClassType(OnlineClass model)
        {
            if (!model.IsAutoClassType)
            {
                return model;
            }

            model.ClassType = Enums.OnlineClassType.Registering;
            if (model?.UpdateDate < DateTime.Now)
            {
                model.ClassType = Enums.OnlineClassType.End;
                return model;
            }
            if (model?.CreatedDate < DateTime.Now)
            {
                model.ClassType = Enums.OnlineClassType.OnPerforming;
                return model;
            }
            var count = 0;
            using (var context = new SWEntities())
                count = context.ZarinpalPayments.Count(x => x.Status == 100 && x.OnlineClassId == model.Id);
            if (count >= model.Capacity)
            {
                model.ClassType = Enums.OnlineClassType.FullCapacity;
                return model;
            }
            return model;
        }
        public bool Save(OnlineClass model, learn_user currentUser, bool isNew, out string message)
        {
            CalculateClassType(model);
            if (string.IsNullOrEmpty(model.AdobeMeeting) || model.AdobeScoId == null)
            {
                model.AdobeMeeting = Guid.NewGuid().ToString().Replace("-", "");
                isNew = true;
            }

            var save = base.Save(model, false);
            message = "ثبت در سامانه ادوبی با مشکل روبرو شد: ";
            if (save)
            {
                if ((!string.IsNullOrEmpty(model.AdobeMeeting) && model.AdobeScoId != null) || model.ClassType != Enums.OnlineClassType.Registering)
                {
                    message = "ویرایش با موفقیت انجام شد ";
                    return save;
                }

                return AddToAdobe(model, currentUser, isNew, out message);
            }
            return save;
        }
        private bool AddToAdobe(OnlineClass model, learn_user currentUser, bool isNew, out string message)
        {
            message = "ثبت در سامانه ادوبی با مشکل روبرو شد: ";
            try
            {
                var adobe = new ClassUsingSdk(currentUser.Email, currentUser.PassAdobe);
                if (adobe.IsLogin && adobe.Api != null)
                {
                    message = "ثبت در سامانه ادوبی با موفقیت انجام شد ";
                    var res = adobe.MeetingUpdate(
                          new AdobeConnectSDK.Model.MeetingUpdateItemViewModel()
                          {
                              Name = model.AdobeMeeting,
                              Description = model.name,
                              Email = currentUser.Email,
                              FirstName = currentUser.Name,
                              LastName = currentUser.Family,
                              MeetingItemType = AdobeConnectSDK.Model.SCOtype.Meeting,
                              DateBegin = model.CreatedDate,
                              DateEnd = model.UpdateDate ?? model.CreatedDate.AddMonths(1),
                              FolderId = AdobeConnectSDK.Common.Constants.FolderIdofSharedMeetingTemplates,
                              ScoId = model.AdobeScoId
                          }
                          , isNew);

                    model.AdobeMeeting = res.FullUrl;
                    model.AdobeScoId = long.Parse(res.ScoId);
                    var save = base.Save(model, false);

                    if (save && isNew)
                    {
                        return AddTeacherToAdobe(model, adobe, out message);
                    }
                    return save;
                }
                return false;
            }
            catch (System.Exception e)
            {
                message += e.Message;
                return false;
            }
        }
        private bool AddTeacherToAdobe(OnlineClass model, ClassUsingSdk adobe, out string message)
        {
            message = "ثبت در سامانه ادوبی با موفقیت انجام شد ";
            adobe.SpecialPermissionsUpdate(model.AdobeScoId.Value, SpecialPermissionId.Remove);

            using (SWEntities db = new SWEntities())
            {
                var teacher = db.learn_teacher.Include(z => z.learnUser).FirstOrDefault(x => x.id == model.id_learn_teacher);
                var pass = teacher.learnUser?.PassAdobe;
                var email = teacher.learnUser?.Email;
                email = string.IsNullOrEmpty(email) ? teacher.email : email;
                pass = string.IsNullOrEmpty(pass) ? "123456" : pass;
                adobe.GetCurrentUserInfoViewModel();
                if (teacher.PrincipalId != null)
                {
                    long teacherPrincipalId = teacher.PrincipalId.Value;
                    adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = teacherPrincipalId }, PermissionId.Host);
                    adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = adobe.userInfoViewModel.UserIdVm }, PermissionId.Host);
                    return true;
                }
                else
                {
                    try
                    {
                        ClassUsingSdk adobeTeacher = new ClassUsingSdk(email, pass);
                        if (adobeTeacher.IsLogin)
                        {
                            teacher.PrincipalId = long.Parse(adobeTeacher.GetCurrentUserInfoViewModel().UserId);
                            db.SaveChanges();
                            adobe.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.TrainingManagersOfGroupMembership, teacher.PrincipalId.Value); //add teacher to Training Managers group
                            adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = teacher.PrincipalId.Value }, PermissionId.Host);
                            adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = adobe.userInfoViewModel.UserIdVm }, PermissionId.Host);
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        try
                        {
                            if (adobe.IsLogin)
                            {
                                var Principal = adobe.UserCreate(new PrincipalSetupViewModel() { FirstName = teacher.name, LastName = teacher.family, Email = email, Password = pass, Description = teacher.tel + " " + teacher.meli + " " + teacher.address });
                                teacher.PrincipalId = long.Parse(Principal.PrincipalId);
                                db.SaveChanges();
                                adobe.GroupMembershipUpdate(AdobeConnectSDK.Common.Constants.TrainingManagersOfGroupMembership, teacher.PrincipalId.Value); //add teacher to Training Managers group
                                adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = adobe.userInfoViewModel.UserIdVm }, PermissionId.Host);
                                adobe.PermissionsUpdate(new PermaissionFilter() { AclId = model.AdobeScoId.Value, PrincipalId = teacher.PrincipalId.Value }, PermissionId.Host);
                                return true;
                            }
                        }
                        catch (Exception e)
                        {
                            message = "ثبت در سامانه ادوبی انجام شد ولی ثبت نام استاد با مشکل روبرو شد: " + e.Message;
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public override int Remove(int id)
        {
            using (var context = new SWEntities())
            {
                OnlineClass result = context.Set<OnlineClass>().Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    return 0;
                }
                var groupId = result.id_learn_cours_group;
                context.Set<OnlineClass>().Remove(result);
                context.SaveChanges();

                return groupId;
            }
        }
    }
}