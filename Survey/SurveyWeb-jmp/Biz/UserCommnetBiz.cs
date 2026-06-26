using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SurveyWeb.Biz
{
    public class UserCommentBiz : RepositoryBaseParentChild<UserComment>
    {
        public static readonly UserCommentBiz Instance = new UserCommentBiz();
        public bool Accept(int id)
        {
            using (var ctx = new Context())
            {
                var obj = ctx.UserComment.Find(id);
                if (obj == null)
                {
                    return false;
                }
                obj.Status = !obj.Status;
                ctx.SaveChanges();
                return true;
            }
               
        }
        public List<UserComment> GetAllsync(System.Linq.Expressions.Expression<Func<UserComment, bool>> Expr)
        {
            using (var ctx = new Context())
                return  ctx.UserComment.Include(x => x.user).Where(Expr).ToList();
        }
    }
}