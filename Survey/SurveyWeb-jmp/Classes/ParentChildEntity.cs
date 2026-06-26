using MVC.Controls.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SurveyWeb
{
    public interface IParentChild : IBaseEntity
    {
        int? ParentId { get; set; }
        string Description { get; set; }
        int Order { get; set; }
    }
    public class ParentChildEntity : BaseEntity, IParentChild
    {
        [DisplayName(@"پدر")]
        public int? ParentId { get; set; }
        [DisplayName(@"توضیحات")]
        [GenericRequired]
        [GenericStringLength(100)]
        public string Description { get; set; }
        [Display(Name = "ترتیب")]
        public virtual int Order { get; set; }
        [NotMapped, DisplayName(@"نام پدر")]
        public string ParentName { get; set; }
        public override void Validate()
        {
            base.Validate();
            if (string.IsNullOrEmpty(Description))
                throw new Exception("لطفا توضیحات را وارد نمایید.");
        }
        //[NotMapped]
        //public override DateTime CreatedDate { get => base.CreatedDate; set => base.CreatedDate = value; }
        //[NotMapped]
        //public override DateTime? UpdateDate { get => base.UpdateDate; set => base.UpdateDate = value; }
    }
    public class GetRecursiveJsTreeList<T> : IDisposable
       where T : ParentChildEntity
    {
        public static readonly GetRecursiveJsTreeList<T> Instance = new GetRecursiveJsTreeList<T>();
        public List<TreeJsonModel> GetTreeList(ICollection<T> list)
        {
            var treeList = new List<TreeJsonModel>();
            if (list == null) return null;

            foreach (var l in list.Where(i => i.ParentId == null).OrderBy(x=>x.Order))
            {
                List<TreeJsonModel> childTreeList = CreateRecursiveChilds(list, l.Id);

                var parentTreeModel = new TreeJsonModel
                {
                    children = childTreeList,
                    data = new TreeJsonModelData { title = l.Description }
                };

                var treeParameterJsonModel = new TreeParameterJsonModel { Code = l.Id.ToString() };

                parentTreeModel.attr = treeParameterJsonModel;
                treeList.Add(parentTreeModel);
            }
            return treeList;
        }

        private List<TreeJsonModel> CreateRecursiveChilds(ICollection<T> list, long parentId)
        {
            if (list == null) return null;
            var treeList = new List<TreeJsonModel>();
            List<T> parentList = list.Where(w => w.ParentId == parentId).ToList();

            foreach (var t in parentList.OrderBy(x => x.Order))
            {
                var treeJsonModel = new TreeJsonModel { data = new TreeJsonModelData { title = t.Description } };

                var treeParameterJsonModel = new TreeParameterJsonModel { Code = t.Id.ToString() };

                treeJsonModel.attr = treeParameterJsonModel;
                treeJsonModel.children = CreateRecursiveChilds(list, t.Id);
                treeList.Add(treeJsonModel);
            }
            return treeList;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}