using System.Linq;
using SenakLearn.Models;
using System.Web.Mvc;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using System;

namespace SenakLearn.Controllers.Admin
{
    public class OfflineVideoController : BaseAdminController
    {
        public ActionResult IndexAll(int learn_coursId)
        {
            ViewBag.Url = "/OfflineVideo/GetTreeList?id=" + learn_coursId;
            ViewBag.learn_coursId = learn_coursId;
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public static GridColumnModelList<OfflineVideo> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<OfflineVideo> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<OfflineVideo>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true).SetCellType(GridCellType.INT);
                Columns.Add(x => x.ParentId).SetCaption("پدر").SetWidth("100").SetCellType(GridCellType.INT);
                Columns.Add(x => x.Description).SetCaption("توضیحات").SetWidth("300");
                Columns.Add(x => x.IsFree).SetCaption("رایگان").SetWidth("100");
                Columns.Add(x => x.VideoId).SetCaption("VideoId").SetWidth("100");
                Columns.Add(x => x.UpdateDateShamsi).SetCaption("تاریخ ایجاد").SetWidth("100");
                Columns.Add(x => x.CreatedDateShamsi).SetCaption( "ویرایش ").SetWidth("100");

            }
            return Columns;
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.OfflineVideoBiz.Instance.GetAllPagedList(grid);
            //var count = Biz.zarinpalBiz.Instance.Count;
            return Json(new
            {
                Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
                Page = grid.PageIndex,
                Records = list.TotalCount,
                Rows = list.ToArray(),
                UserData = "Null"
            },
          JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetTreeList(int id)
        {
            var OfflineVideo = Biz.OfflineVideoBiz.Instance.GetAll(x => x.learn_coursId == id);// db.OnlineClassAccorations.Find(id);
            if (OfflineVideo == null)
            {
                return HttpNotFound();
            }
            var treeList = GetRecursiveJsTreeList<OfflineVideo>.Instance.GetTreeList(OfflineVideo.ToList());
            return Json(treeList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {

            OfflineVideo OfflineVideo = Biz.OfflineVideoBiz.Instance.Get(id ?? 0);// db.OfflineVideos.Find(id);
            if (OfflineVideo == null)
            {
                return HttpNotFound();
            }
            return PartialView();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(OfflineVideo OfflineVideo)
        {
          //  if (ModelState.IsValid)
            {
                Biz.OfflineVideoBiz.Instance.Save(OfflineVideo);
                return RedirectToAction("Create");
            }

          //  return PartialView(OfflineVideo);
        }

        // GET: OfflineVideos/Edit/5
        public ActionResult Edit(int? id)
        {

            OfflineVideo OfflineVideo = Biz.OfflineVideoBiz.Instance.Get(id ?? 0);// db.OfflineVideos.Find(id);
            if (OfflineVideo == null)
            {
                return HttpNotFound();
            }
            return PartialView("Create", OfflineVideo);
        }


        [HttpPost]
        public ActionResult Edit(OfflineVideo OfflineVideo)
        {
            //if (ModelState.IsValid)
            {
                Biz.OfflineVideoBiz.Instance.Save(OfflineVideo);
                //db.Entry(OfflineVideo).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Create", new { id = OfflineVideo.learn_coursId });
            }
           // return PartialView("Create", OfflineVideo);
        }

        public ActionResult Delete(int id)
        {
            Biz.OfflineVideoBiz.Instance.Remove(id);
            return null;// RedirectToAction("Index", new { id = OfflineVideo.learn_coursId });
        }
    }
}