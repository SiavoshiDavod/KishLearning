using SenakLearn.JqGrid;
using SenakLearn.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using SenakLearn.JqGrid.Common;
using System.Threading.Tasks;
using System;

namespace SenakLearn.Biz
{
    public class TeacherBiz
    {
        public static readonly TeacherBiz Instance = new TeacherBiz();
        public JqGrid.PagedList<learn_teacher> GetAllPagedList(GridSettings grid)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_teacher.Include(c => c.learnUser).FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }

        public List<learn_teacher> FindAll(int take = 6)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_teacher.Where(x => x.status && x.IsFavorite).Take(take).ToList();
            }
        }

        public learn_teacher Create(learn_teacher teacher)
        {
            using (SWEntities db = new SWEntities())
            {
                db.learn_teacher.Add(teacher);
                db.SaveChanges();
                return teacher;
            }
        }

        public List<learn_teacher> FindAll(int take, int skip)
        {
            using (SWEntities db = new SWEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                return db.learn_teacher.Where(x => x.status).OrderByDescending(x => x.id).Skip(skip).Take(take).ToList();
            }
        }
        public learn_teacher FindByUserId(int userId)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_teacher.FirstOrDefault(x => x.UserId == userId);
            }
        }
        public learn_teacher FindById(int id)
        {
            using (SWEntities db = new SWEntities())
            {
                return db.learn_teacher.FirstOrDefault(x => x.id == id);
            }
        }

        public async Task UpdateCourseCount( int teacherId, bool add = true)
        {
            string operate = add ? "+" : "-";
            using (var ctx = new SWEntities())
            {
                try
                {
                    await ctx.Database.ExecuteSqlCommandAsync($"update [dbo].[learn_teacher] set CourseCount= CourseCount{operate}1  where id={teacherId}");
                }
                catch (Exception ex)
                {
                    throw new Exception("error update count teacher !");
                }

            }
        }
    }
}