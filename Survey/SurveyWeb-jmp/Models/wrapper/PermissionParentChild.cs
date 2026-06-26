using MVC.Controls.Tree;
using SurveyWeb.Models.Security;
using System.Collections.Generic;
using System.Linq;

namespace SurveyWeb.Models.wrapper
{
    public class PermissionParentChild
    {
        public List<PermissionParentChild> Childs { get; set; }
        public Permisstion Permisstion { get; set; }
        public int Id { get { return (int)Permisstion; } set { } }
        public string Url { get { return "/" + Permisstion + "/Index"; } set { } }
        public string Description { get; set; }


    }
    /// <summary>
    ///  var result= GetTreeJsonModel.Instance.GetTreeList(GetTreeJsonModel.PermissionParentChildStaticList);
    /// </summary>
    public class GetTreeJsonModel
    {
        public static readonly List<PermissionParentChild> PermissionParentChildStaticList = new List<PermissionParentChild>()
        {
            new PermissionParentChild()
            {
                Description="مراکزپذیرایی",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Resturant)],
                         Permisstion=Permisstion.Resturants
                    },new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.ResturantType)],
                         Permisstion=Permisstion.ResturantTypes
                    },new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.CheckListType)],
                         Permisstion=Permisstion.CheckListTypes
                    }
                }
            },
            new PermissionParentChild()
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
                Description="اخبار",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.NewsGroup)],
                         Permisstion=Permisstion.NewsGroups
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Author)],
                         Permisstion=Permisstion.Authors
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.News)],
                         Permisstion=Permisstion.News
                    },
                     new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.UserComment)],
                         Permisstion=Permisstion.UserComment
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
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Menu)],
                         Permisstion=Permisstion.Menus
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.MenuSub)],
                         Permisstion=Permisstion.MenuSubs
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Faq)],
                         Permisstion=Permisstion.Faqs
                    },
                     new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SiteSetting.SiteSetting)],
                         Permisstion=Permisstion.SiteSetting
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.ContactUs)],
                         Permisstion=Permisstion.ContactUs
                    },
                      new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.NewsSubscription)],
                         Permisstion=Permisstion.NewsSubscriptions
                    },
                        new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.CompanyLogoAndLink)],
                         Permisstion=Permisstion.CompanyLogoAndLinks
                    },
                        new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.OrgIntro)],
                         Permisstion=Permisstion.OrgIntroes
                    },
                    //    new PermissionParentChild()
                    //{
                    //     Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Tender)],
                    //     Permisstion=Permisstion.Tender
                    //},
                }
            },
            new PermissionParentChild()
            {
                Description="تنظیمات کارتابل",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Cartable)],
                         Permisstion=Permisstion.Cartables
                         ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(CartableLog)], Permisstion = Permisstion.CartableLogs } }
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.CartableRelation)],
                         Permisstion=Permisstion.CartableRelations
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.CartableUserAccess)],
                         Permisstion=Permisstion.CartableUserAccesses
                    },
                }
            },
            new PermissionParentChild()
            {
                Description="خدمات عمومی",
                Childs=new List<PermissionParentChild>()
                {
                   new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Suggestion)],
                         Permisstion=Permisstion.Suggestions
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Idea)],
                         Permisstion=Permisstion.Ideas
                    },
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Regulation)],
                         Permisstion=Permisstion.Regulations
                    },


                }
            },
            new PermissionParentChild()
            {
                Description="بازرسی",
                Childs=new List<PermissionParentChild>()
                {
                    new PermissionParentChild()
                    {
                         Description=Dictionary.ModelDictionary[typeof(SurveyWeb.Models.Complaint)],
                         Permisstion=Permisstion.Complaints
                    }
                }
            },
            new PermissionParentChild()
            {
                Description="امنیت سیستم",
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
                         Description=Dictionary.ModelDictionary[typeof(User)],
                         Permisstion=Permisstion.Users
                         ,Childs=new List<PermissionParentChild>(){ new PermissionParentChild() { Description=Dictionary.ModelDictionary[typeof(RoleUser)], Permisstion = Permisstion.RoleUsers } }
                    },
                }
            },
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