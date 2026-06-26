using MVC.Controls.Tree;
using SenakLearn.Models.Person;
using SenakLearn.Models.Security;
using System.Collections.Generic;
using System.Linq;

namespace SenakLearn.Models.wrapper
{
    public class PermissionParentChild
    {
        public List<PermissionParentChild> Childs { get; set; }
        public Permisstion Permisstion { get; set; }
        public int Id { get { return (int)Permisstion; } set { } }
        public string Url { get { return "/" + Permisstion + "/Index"; } set { } }
        public string Description { get; set; }
        // public string DashboardClass { get; set; }


    }
    /// <summary>
    ///  var result= GetTreeJsonModel.Instance.GetTreeList(GetTreeJsonModel.PermissionParentChildStaticList);
    /// </summary>
    public class GetTreeJsonModel
    {
        public static readonly List<PermissionParentChild> PermissionParentChildStaticList = new List<PermissionParentChild>()
        {   new PermissionParentChild()
            {
                Description="نظرسنجی",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                        Description=Dictionary.ModelDictionary[typeof(GroupSurvey)],
                        Permisstion=Permisstion.GroupSurveys,
                    },

                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyUserAnswer)],
                         Permisstion=Permisstion.SurveyUserAnswers
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyEntity)],
                         Permisstion=Permisstion.SurveyEntitys
                        ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(SurveyAnswer)], Permisstion=Permisstion.SurveyAnswers },new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(SurveyGroupQuestion)], Permisstion = Permisstion.SurveyGroupQuestions } , new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(SurveyQuestion)], Permisstion = Permisstion.SurveyQuestions,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description = Dictionary.ModelDictionary[typeof(SurveyQuestionOption)], Permisstion = Permisstion.SurveyQuestionOptions } }  } } },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyPrivateGroup)],
                         Permisstion=Permisstion.SurveyPrivateGroups
                        ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(SurveyPrivateGroupUser)], Permisstion = Permisstion.SurveyPrivateGroupUsers } }
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="آزمون",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                        Description=Dictionary.ModelDictionary[typeof(GroupAzmoon)],
                        Permisstion=Permisstion.GroupAzmoons,
                    },

                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(AzmoonUserAnswer)],
                         Permisstion=Permisstion.AzmoonUserAnswers
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(AzmoonEntity)],
                         Permisstion=Permisstion.AzmoonEntitys
                        ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(AzmoonAnswer)], Permisstion=Permisstion.AzmoonAnswers },new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(AzmoonGroupQuestion)], Permisstion = Permisstion.AzmoonGroupQuestions } , new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(AzmoonQuestion)], Permisstion = Permisstion.AzmoonQuestions,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description = Dictionary.ModelDictionary[typeof(AzmoonQuestionOption)], Permisstion = Permisstion.AzmoonQuestionOptions } }  } } },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(AzmoonPrivateGroup)],
                         Permisstion=Permisstion.AzmoonPrivateGroups
                        ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(AzmoonPrivateGroupUser)], Permisstion = Permisstion.AzmoonPrivateGroupUsers } }
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="اساتید",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.learn_teacher)],
                         Permisstion=Permisstion.Teacher
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.JoinUs)],
                         Permisstion=Permisstion.JoinUs
                    }
                }
            },
            new PermissionParentChild()
            {
                Description="کتابخانه و مقالات",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.Book)],
                         Permisstion=Permisstion.Book
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.Paper)],
                         Permisstion=Permisstion.Papers
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperTranslateQuality)],
                         Permisstion=Permisstion.PaperTranslateQuality
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperUniversity)],
                         Permisstion=Permisstion.PaperUniversity
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperJournal)],
                         Permisstion=Permisstion.PaperJournal
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperPublisher)],
                         Permisstion=Permisstion.PaperPublisher
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperTrend)],
                         Permisstion=Permisstion.PaperTrend
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.PaperField)],
                         Permisstion=Permisstion.PaperField
                    }
                }
            },
            new PermissionParentChild()
            {
                Description="ویدیو",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.VideoFile)],
                         Permisstion=Permisstion.VideoFile
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.StudentSupport)],
                         Permisstion=Permisstion.StudentSupport
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.TeacherSupport)],
                         Permisstion=Permisstion.TeacherSupport
                    },
                     new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.OfflineVideo)],
                         Permisstion=Permisstion.OfflineVideo
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="مدیریت سیستم",
                Childs=new List<PermissionParentChild>()
                {

                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.Menu)],
                         Permisstion=Permisstion.Menus
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.DynamicForm)],
                         Permisstion=Permisstion.DynamicForms
                    },

                        new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SiteSetting.SiteSetting)],
                         Permisstion=Permisstion.SiteSetting
                    },
                                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.CompanyLogoAndLink)],
                         Permisstion=Permisstion.CompanyLogoAndLinks
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.UserCommnet)],
                         Permisstion=Permisstion.UserComments
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.ObjCount)],
                         Permisstion=Permisstion.ObjCount
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="دوره ها",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.learn_cours_group)],
                         Permisstion=Permisstion.Group
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.learn_cours)],
                         Permisstion=Permisstion.Cours
                    },
                                        new PermissionParentChild()
                    {
                         Description="پادکست",
                         Permisstion=Permisstion.Podcast
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.OnlineClass)],
                         Permisstion=Permisstion.OnlineClasses
                    },
                     new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.OnlineClassAccoration)],
                         Permisstion=Permisstion.OnlineClassAccorations
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.OnlineClassAccorationDetails)],
                         Permisstion=Permisstion.OnlineClassAccorationDetails
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="امور مالی",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(ZarinpalPayment)],
                         Permisstion=Permisstion.ZarinpalPayments
                    }

                }
            },
            new PermissionParentChild()
            {
                Description="اخبار",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.NewsGroup)],
                         Permisstion=Permisstion.NewsGroups
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.Author)],
                         Permisstion=Permisstion.Authors
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.News)],
                         Permisstion=Permisstion.News
                    },

                }
            },
            new PermissionParentChild()
            {
                Description="ارتباط با مشتری",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.EmailSms)],
                         Permisstion=Permisstion.SendSms
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.Group)],
                         Permisstion=Permisstion.GroupUser
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.GroupDetail)],
                         Permisstion=Permisstion.GroupDetail
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Models.OnlineClassRequest)],
                         Permisstion=Permisstion.OnlineClassRequests
                    }
                }
            },
            new PermissionParentChild()
            {
                Description="سیستم",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Role)],
                         Permisstion=Permisstion.Roles
                         ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(RolePermission)], Permisstion = Permisstion.RolePermissions } }
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(learn_user)],
                         Permisstion=Permisstion.UsersAdmin
                         ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(RoleUser)], Permisstion = Permisstion.RoleUsers } }
                    },
                    new PermissionParentChild()
                        {
                            Description=Dictionary.ModelDictionary[typeof(Organization)],
                            Permisstion=Permisstion.Org
                        },
                      new PermissionParentChild()
                        {
                            Description=Dictionary.ModelDictionary[typeof(Post)],
                            Permisstion=Permisstion.Post
                        },


                }
            },
            new PermissionParentChild()
            {
                Description="پرسنل",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Person_Course)],
                         Permisstion=Permisstion.PersonCourse
                    },
                     new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(Person_Teacher)],
                         Permisstion=Permisstion.PersonTeacher
                    },
                }
                        }
        };

    
    public static readonly GetTreeJsonModel Instance = new GetTreeJsonModel();
    public List<TreeJsonModel> GetTreeList(ICollection<PermissionParentChild> list)
    {
        var treeList = new List<TreeJsonModel>();
        if (list == null) return null;
        int code = 10000;
        foreach (var l in list)
        {
            code++;
            List<TreeJsonModel> childTreeList = CreateRecursiveChilds(l.Childs);

            var parentTreeModel = new TreeJsonModel
            {
                children = childTreeList,
                data = new TreeJsonModelData { title = l.Description }
            };

            var treeParameterJsonModel = new TreeParameterJsonModel { Code = code.ToString() };

            parentTreeModel.attr = treeParameterJsonModel;
            treeList.Add(parentTreeModel);
        }
        return treeList;
    }

    private List<TreeJsonModel> CreateRecursiveChilds(ICollection<PermissionParentChild> parentList)
    {
        if (parentList == null) return null;
        var treeList = new List<TreeJsonModel>();

        foreach (var t in parentList)
        {
            var treeJsonModel = new TreeJsonModel { data = new TreeJsonModelData { title = t.Description } };

            var treeParameterJsonModel = new TreeParameterJsonModel { Code = t.Id.ToString() };

            treeJsonModel.attr = treeParameterJsonModel;
            treeJsonModel.children = CreateRecursiveChilds(t.Childs);
            treeList.Add(treeJsonModel);
        }
        return treeList;
    }
}
}