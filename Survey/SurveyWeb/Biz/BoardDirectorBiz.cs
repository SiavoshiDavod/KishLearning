using SurveyWeb.Models;
using SurveyWeb.JqGrid;
using SurveyWeb.JqGrid.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SurveyWeb.Biz
{
    public class BoardDirectorBiz
    {
        public static readonly BoardDirectorBiz Instance = new BoardDirectorBiz();
        public JqGrid.PagedList<BoardDirector> GetAllPagedList(GridSettings grid)
        {
            using (Models.Context db = new Models.Context())
            {
                return db.BoardDirector.FilterAndSortJqGrid(grid).ToPagedList(grid);
            }
        }

        public List<BoardDirector> FindAll(int take = 20)
        {
            using (Models.Context db = new Models.Context())
            {
                return db.BoardDirector.Where(x => x.status).Take(take).ToList();
            }
        }

        public BoardDirector Create(BoardDirector teacher)
        {
            using (Models.Context db = new Models.Context())
            {
                db.BoardDirector.Add(teacher);
                db.SaveChanges();
                return teacher;
            }
        }
        public BoardDirector Update(BoardDirector teacher)
        {
            using (Models.Context db = new Models.Context())
            {
                teacher.UpdateDate = DateTime.Now;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return teacher;
            }
        }

        public List<BoardDirector> FindAll(int take, int skip)
        {
            using (Models.Context db = new Models.Context())
            {
                return db.BoardDirector.Where(x => x.status).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();
            }
        }
   
        public BoardDirector FindById(int id)
        {
            using (Models.Context db = new Models.Context())
            {
                return db.BoardDirector.FirstOrDefault(x => x.Id == id);
            }
        }
        public void Remove(int id)
        {
            using (Models.Context db = new Models.Context())
            {
                var item= db.BoardDirector.FirstOrDefault(x => x.Id == id);
                if (item!=null)
                {
                    db.BoardDirector.Remove(item);
                    db.SaveChanges();
                }
            }
        }
    }

}