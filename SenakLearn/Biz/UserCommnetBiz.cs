using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using SenakLearn.Models;

namespace SenakLearn.Biz
{
    public class UserCommnetBiz: RepositoryBaseParentChild<SenakLearn.Models.UserCommnet>
    {
        public static readonly UserCommnetBiz Instance = new UserCommnetBiz();
        public bool Accept(int id)
        {
            using (var context = new SWEntities())
            {
                var obj = context.UserCommnet.Find(id);
                if (obj == null)
                {
                    return false;
                }
                obj.Status = !obj.Status;
                context.SaveChanges();
                return true;
            }
        }
        public override IEnumerable<UserCommnet> GetAll(System.Linq.Expressions.Expression<Func<UserCommnet, bool>> Expr)
        {
            using (var context = new SWEntities())
                return context.UserCommnet.Include(x=>x.learn_user).Where(Expr).ToList();
        }
    }
}