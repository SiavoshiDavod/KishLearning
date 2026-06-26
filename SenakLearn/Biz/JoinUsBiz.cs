using SenakLearn.Models;
using System.Linq;

namespace SenakLearn.Biz
{
    public class JoinUsBiz : RepositoryBase<SenakLearn.Models.JoinUs>
    {
        public static readonly JoinUsBiz Instance = new JoinUsBiz();

        public JoinUs FidnByTeacherId(int id)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.TeacherId == id);
                return obj;
            }
        }

        public JoinUs Accept(int id)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.Id == id);
                if (obj == null)
                    return null;
                if (!obj.IsAccept)
                {
                    obj.IsAccept = true;
                    obj.AcceptedDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                return obj;
            }
        }
        public JoinUs AcceptContract(int userId)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.UserId == userId);
                if (obj == null)
                    return null;
                if (!obj.IsAcceptContract)
                {
                    obj.IsAcceptContract = true;
                    obj.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                return obj;
            }
        }
        public JoinUs UploadVideo(int userId)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.UserId == userId);
                if (obj == null)
                    return null;
                if (!obj.IsUploadVideo)
                {
                    obj.IsUploadVideo = true;
                    obj.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                return obj;
            }
        }
        public bool SetUserId(int id, int UserId)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.Id == id);
                if (obj == null)
                    return false;
                if (!obj.IsAccept)
                    return false;
                if (obj.TeacherId != null)
                    return false;
                if (obj.UserId == null)
                {
                    if (db.learn_teacher.Any(x=>x.UserId==UserId))
                    {
                        return false;
                    }
                    obj.UserId = UserId;
                    obj.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                else if (obj.UserId != UserId)
                {
                    return false;
                }
                return true;
            }
        }

        public bool SetTeacherId(int id, int teacherId)
        {
            using (SWEntities db = new SWEntities())
            {
                var obj = db.JoinUs.First(x => x.Id == id);
                if (obj == null)
                    return false;
                if (!obj.IsAccept)
                    return false;
                if (obj.TeacherId == null)
                {
                    obj.TeacherId = teacherId;
                    obj.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                }
                else if (obj.TeacherId != teacherId)
                {
                    return false;
                }
                return true;
            }
        }

    }
}