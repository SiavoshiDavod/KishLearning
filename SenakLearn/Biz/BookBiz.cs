using DocumentFormat.OpenXml.Presentation;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Razor.Tokenizer.Symbols;

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
        public List<Book> ListBooks(string title, int? groupId, int? publisherId, string author, string keyword, int page = 1, bool filter = false)
        {
            using (var context = new SWEntities())
            {

                var query= context.Set<Book>().AsQueryable();
                if (!string.IsNullOrEmpty(title))
                    query = query.Where(w => w.Title.Contains(title) || w.TitleF.Contains(title));
                if (!string.IsNullOrEmpty(author))
                    query = query.Where(w => w.Author.Contains(author));
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(w => w.Keyword.Contains(keyword));
                if (groupId != null && groupId != 0)
                    query = query.Where(w => w.GroupId == groupId);
                if (publisherId != null && publisherId != 0) query = query.Where(w => w.PublisherId == publisherId);


                return query.OrderByDescending(x => x.Id).ToList();
            }
        }
        public List<BookSlideModel> GetBooksInSlider()
        {
            using (var context = new SWEntities())
            {

                return context.Set<BookSlideModel>().Where(w => w.IsActive == true).OrderByDescending(x => x.Id).ToList();
            }
        }
        public Dictionary<Book, byte[]> GetBookFile(int bookId, string serverBookPath)
        {
            var result = new Dictionary<Book, byte[]>();
            byte[] bytes = null;
            using (var context = new SWEntities())
            {
                Book book = Get(bookId);
                if (book == null)
                    return null;
                var videoDir = serverBookPath + "/" + book.FileId;
                bytes = System.IO.File.ReadAllBytes(videoDir);
                result.Add(book, bytes);
            }
            return result;
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