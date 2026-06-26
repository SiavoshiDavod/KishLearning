using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Biz
{
    public class BookBiz : RepositoryBase<SenakLearn.Models.Book>
    {
        public static readonly BookBiz Instance = new BookBiz();
        public Tuple<List<Book>, int> GetBooks(string title, string titleE, string titleF, int groupId, int publisherId, int skip = 0, int take = 10)
        {
            using (var context = new SWEntities())
            {
                var query = context.Book.Where(x => (titleE == null || titleE == x.Title) && (titleF == null || titleF == x.TitleF)
            // &&(string.IsNullOrWhiteSpace(title)||x.Keyword.Contains(title))
            && (publisherId == 0 || publisherId == x.PublisherId)
            && (groupId == 0 || groupId == x.GroupId)
            );
                var list = query.OrderByDescending(x => x.Id).Take(take).Skip(skip).ToList();
                var count = query.Count();
                return new Tuple<List<Book>, int>(list, count);
            }
        }
        public List<Book> GetAllBooks(System.Linq.Expressions.Expression<Func<Book, bool>> Expr)
        {
            using (var context = new SWEntities())
            {

                    return context.Set<Book>().Where(Expr).OrderByDescending(x => x.Id).ToList();
            }
        }
        public List<BookSlideModel> GetBooksInSlider()
        {
            using (var context = new SWEntities())
            {

                return context.Set<BookSlideModel>().Where(w=>w.IsActive==true).OrderByDescending(x => x.Id).ToList();
            }
        }
        public override int Remove(int id)
        {
            using (var context = new SWEntities())
            {
                Book result = context.Set<Book>().Where(x => x.Id == id).FirstOrDefault();
                if (null == result)
                {
                    return 0;
                }
                var groupId = result.GroupId;
                context.Set<Book>().Remove(result);
                context.SaveChanges();

                return groupId;
            }
        }
    }
}