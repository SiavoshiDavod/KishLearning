using GemBox.Spreadsheet;
using MVC.Controls.Grid;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class GroupDetailController : BaseAdminController
    {
        static string key = "FREE-LIMITED-KEY";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Selector()
        {
            return PartialView();
        }
        public ActionResult LoadList(GridSettings grid)
        {
            var list = Biz.GroupDetailBiz.Instance.GetAllPagedList(grid);
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

        public static GridColumnModelList<GroupDetail> Columns { get; private set; } = GetColumns();
        public static GridColumnModelList<GroupDetail> GetColumns()
        {
            if (Columns == null)
            {
                Columns = new GridColumnModelList<GroupDetail>();
                Columns.Add(x => x.Id).SetAsPrimaryKey().SetHidden(true);
                Columns.Add(x => x.act).SetCaption("عملیات").SetWidth("50");
                Columns.Add(x => x.Name).SetCaption("نام").SetWidth("300");
                Columns.Add(x => x.Email).SetCaption("ایمیل ").SetWidth("300");
                Columns.Add(x => x.Mobile).SetCaption(" موبایل").SetWidth("100");
            }
            return Columns;
        }
        // GET: GroupDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupDetail GroupDetail = Biz.GroupDetailBiz.Instance.Get(id.Value);
            if (GroupDetail == null)
            {
                return HttpNotFound();
            }
            return View(GroupDetail);
        }

        // GET: GroupDetail/Create
        public ActionResult Create(int id = 0)
        {
            GroupDetail GroupDetail = id == 0 ? new GroupDetail() : Biz.GroupDetailBiz.Instance.Get(id);
            return View(GroupDetail);
        }

        // POST: GroupDetail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(GroupDetail GroupDetail, HttpPostedFileBase File, HttpPostedFileBase ScreenShot, HttpPostedFileBase TranslateWord, HttpPostedFileBase TranslatePdf)
        {
            // if (ModelState.IsValid)
            {

                Biz.GroupDetailBiz.Instance.Save(GroupDetail);
                return RedirectToAction("Index");
            }

            // return View(GroupDetail);
        }


        // GET: GroupDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupDetail GroupDetail = Biz.GroupDetailBiz.Instance.Get(id.Value);
            if (GroupDetail == null)
            {
                return HttpNotFound();
            }
            return View(GroupDetail);
        }

        // POST: GroupDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Biz.GroupDetailBiz.Instance.Remove(id);
            return RedirectToAction("Index");
        }

        public FileResult Download()
        {
            SpreadsheetInfo.SetLicense(key);

            // Create a new empty Excel file.
            var workbook = new ExcelFile();

            // Create a new worksheet and set cell A1 value to 'Hello world!'.
            //workbook.Worksheets.Add("Sheet 1").Cells["A1"].Value = "Hello world!";

            var sheet1 = workbook.Worksheets.Add("Sheet 1");
            sheet1.ViewOptions.ShowColumnsFromRightToLeft = true;
            sheet1.Cells["A1"].Value = "نام و نام خانوادگی";
            sheet1.Cells["B1"].Value = "ایمیل";
            sheet1.Cells["C1"].Value = "موبایل";
            sheet1.Cells["D1"].Value = "نام شرکت یا شغل";
            sheet1.Cells["E1"].Value = "تاریخ تولد1398/02/05";

            // Save to XLSX file.
            //workbook.Save("Sample.xlsx");

            byte[] fileContents;

            var options = SaveOptions.XlsxDefault;

            // Save spreadsheet to XLSX format in byte array.
            using (var stream = new MemoryStream())
            {
                workbook.Save(stream, options);
                fileContents = stream.ToArray();
            }

            // Stream spreadsheet to browser in XLSX format.
            return File(fileContents, options.ContentType, "Sample.xlsx");

            //var path = Server.MapPath("~/Content/Template/Emdad/Sample.xlsx");
            //byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            //string fileName = "Sample.xlsx";
            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult ExcelUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExcelUpload(GroupDetail obj, System.Web.HttpPostedFileBase file)
        {
            var count = 0;
            SpreadsheetInfo.SetLicense(key);

            var workbook = ExcelFile.Load(file.InputStream);

            var list = new List<GroupDetail>();

            // Iterate through all worksheets in an Excel workbook.
            foreach (var worksheet in workbook.Worksheets)
            {
                // Iterate through all rows in an Excel worksheet.
                foreach (var row in worksheet.Rows)
                {
                    var model = new GroupDetail()
                    {
                        //Name = row.Cells["1"].Value?.ToString(),
                        //Email = row.Cells["2"].Value?.ToString(),
                        //Mobile = row.Cells["3"].Value?.ToString(),
                        //Company = row.Cells["4"].Value?.ToString(),
                        //BirthDayShamsi = row.Cells["5"].Value?.ToString(),
                        GroupId = obj.GroupId,
                        CreatedDate = DateTime.Now
                    };
                    var countCells = 0;

                    //// Iterate through all allocated cells in an Excel row.
                    foreach (var cell in row.AllocatedCells)
                    {
                        countCells++;
                        if (cell.ValueType != CellValueType.Null)
                        {
                            if (countCells == 1)
                            {
                                model.Name = cell.Value?.ToString();// cell.ValueType
                            }
                            else if (countCells == 2)
                            {
                                model.Email = cell.Value?.ToString();
                                if (string.IsNullOrEmpty(model.Email))
                                {
                                    model.Email = null;
                                }
                            }
                            else if (countCells == 3)
                            {
                                model.Mobile = cell.Value?.ToString();
                                if (!string.IsNullOrEmpty(model.Mobile)&& model.Mobile.Length==10)
                                {
                                    model.Mobile = "0" + model.Mobile;
                                }
                                if (!System.Text.RegularExpressions.Regex.IsMatch(model.Mobile, @"09\d{9}"))
                                    model.Mobile = null;
                            }
                            else if (countCells == 4)
                            {
                                model.Company = cell.Value?.ToString();
                            }
                            else if (countCells == 5)
                            {
                                model.BirthDayShamsi = cell.Value?.ToString();
                                if (string.IsNullOrEmpty(model.BirthDayShamsi))
                                {
                                    model.BirthDayShamsi = null;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    try
                    {
                        model.Validate();
                        list.Add(model);
                        count++;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            Biz.GroupDetailBiz.Instance.AddAll(list);
            SetViewBagSuccessMessage(count + " ردیف اضافه شد");
            return RedirectToAction("Index");
            //        //string fileName = file.FileName;
            //        //string fileContentType = file.ContentType;
            //        //byte[] fileBytes = new byte[file.ContentLength];
            //        //var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            //        int noOfCol;
            //        using (var package = new ExcelPackage(file.InputStream))
            //        {
            //            var workSheet = package.Workbook.Worksheets.First();
            //            noOfCol = workSheet.Dimension.End.Column - 1;
            //            var noOfRow = workSheet.Dimension.End.Row;
            //            for (var i = 2; i <= noOfRow; i++)
            //            {
            //                var x1 = int.Parse(workSheet.Cells[i, 1].Value.ToString());
            //                    var x1 = workSheet.Cells[i, 2].Value.ToString(),
            //                    var x1 = Convert.ToBoolean(workSheet.Cells[i, 3].Value),
            //                };
            //            }
            //        }
        }
    }
}
