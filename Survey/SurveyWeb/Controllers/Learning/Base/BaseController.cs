using ClosedXML.Excel;
using MVC.Controls.Grid;
using Newtonsoft.Json;
using SenakLearn.Biz;
using SenakLearn.JqGrid;
using SenakLearn.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers
{
    public class BaseController : Controller
    {
        public static readonly Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
        internal void CheckGoogleRecapcha(string gRecaptchaResponse)
        {
            if (HttpContext.IsDebuggingEnabled)
            {
                return;
            }
            if (string.IsNullOrEmpty(gRecaptchaResponse))
            {
                throw new Exception("خطا در اعتبار سنجی ، لطفا مجددا تلاش کنید");
            }
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";

            var postData = "secret=" + SiteSetting.GetSetting.Instance.Get().GoogleSecretKey + "&response=" + gRecaptchaResponse;

            // send post data
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            // receive the response now
            string result = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            // validate the response from Google reCaptcha
            var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
            if (!captChaesponse.Success)
            {
                throw new Exception("خطا در اعتبار سنجی گوگل، لطفا مجددا تلاش کنید");
                //throw new Exception( "Google recaptcha error: "+string.Join(" , ", captChaesponse.ErrorCodes));
            }
        }
        public static GridColumnModelList<learn_user> learnUserColumns { get; private set; } = GetlearnUserColumns();
        public static GridColumnModelList<learn_user> GetlearnUserColumns()
        {
            if (learnUserColumns == null)
            {
                learnUserColumns = new GridColumnModelList<learn_user>();
                learnUserColumns.Add(x => x.BREEZESESSION).SetCaption("ردیف").SetWidth("50");
                learnUserColumns.Add(x => x.Name).SetCaption("نام").SetWidth("50");
                learnUserColumns.Add(x => x.Family).SetCaption("نام خانوادگی").SetWidth("300");
                learnUserColumns.Add(x => x.Email).SetCaption("ایمیل").SetWidth("300");
                learnUserColumns.Add(x => x.NationaCode).SetCaption("کدملی").SetWidth("300");
                learnUserColumns.Add(x => x.Mobile).SetCaption("موبایل").SetWidth("300");
                learnUserColumns.Add(x => x.user_name).SetCaption("نام کاربری").SetWidth("100");
                
            }
            return learnUserColumns;
        }
        protected void SetSessionUser(learn_user user)
        {
            Current_learn_userId = user.id;
            user.Permisstions = UserBiz.Instance.GetPermisstionsByUserId(user);
            Current_learn_user = user;
            Session["user_object"] = JsonConvert.SerializeObject(user);
            //Session["user_name"] = user.user_name;
            //Session["Email"] = user.Email;
            //Session["PassAdobe"] = user.PassAdobe;
            //Session["id"] = user.id;
            //Session["Mobile"] = user.Mobile;
            //Session["Name"] = user.Name;
            //Session["Family"] = user.Family;
            //Session["NationaCode"] = user.NationaCode;
            //Session["Address"] = user.Address;
            //Session["ImageUrl"] = user.ImageUrl;
            //Session["RoleId"] = user.RoleId;
            //Session["Province"] = user.Province;
            //Session["City"] = user.City;
            //Session["Education"] = user.Education;
            //Session["Expertise"] = user.Expertise;
            //Session["BirthLocation"] = user.BirthLocation;
            //Session["BirthDay"] = user.BirthDay;
            //Session["Tel"] = user.Tel;
            //Session["Shenasname"] = user.Shenasname;
            //Session["FatherName"] = user.FatherName;
        }
        public learn_user Current_learn_user { get; private set; }
        public int Current_learn_userId { get; private set; }
        protected learn_user GetSessionUser()
        {
            try
            {
                var user = JsonConvert.DeserializeObject<learn_user>(Session["user_object"].ToString());
                Current_learn_userId = user.id;
                Current_learn_user = user;
                return user;
            }
            catch (Exception)
            {
                return new learn_user();
            }
            //var user = new learn_user();
            //user.user_name = (string)Session["user_name"];
            //user.Email = (string)Session["Email"];
            //user.PassAdobe = (string)Session["PassAdobe"];
            //user.id = ((int?)Session["id"]) ?? 0;
            //user.RoleId = ((Roles?)Session["RoleId"]) ?? Roles.User;
            //user.Mobile = (string)Session["Mobile"];
            //user.Name = (string)Session["Name"];
            //user.Family = (string)Session["Family"];
            //user.NationaCode = (string)Session["NationaCode"];
            //user.Address = (string)Session["Address"];
            //user.ImageUrl = (string)Session["ImageUrl"];
            //user.BREEZESESSION = (string)Session["BREEZESESSION"];
            //Current_learn_userId = user.id;
            //user.Province = (Province?)Session["Province"];
            //user.City = (string)Session["City"];
            //user.Education = (string)Session["Education"];
            //user.Expertise = (string)Session["Expertise"];
            //user.BirthLocation = (string)Session["BirthLocation"];
            //user.BirthDay = (DateTime?)Session["BirthDay"];
            //user.Tel = (string)Session["Tel"];
            //user.Shenasname = (string)Session["Shenasname"];
            //user.FatherName = (string)Session["FatherName"];
            //return user;
        }
        internal void SetViewBagSuccessMessage(string SuccessMessage)
        {
            if (!string.IsNullOrEmpty(SuccessMessage))
                ViewData["SuccessMessage"] = SuccessMessage;
        }
        internal void SetViewBagErrorMessage(string ErrorMessage)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ViewData["ErrorMessage"] = ErrorMessage;
        }
        //internal ActionResult ToJsonPagedList(JqGrid.PagedList<object> list, GridSettings grid)
        //{
        //    return Json(new
        //    {
        //        Total = (int)Math.Ceiling((double)list.TotalCount / grid.PageSize),
        //        Page = grid.PageIndex,
        //        Records = list.TotalCount,
        //        Rows = list.ToArray(),
        //        UserData = "Null"
        //    },
        // JsonRequestBehavior.AllowGet);
        //}
        public enum pathFile
        {
            DynamicForm,
            JoinUs,
            Interview,
            teacher,
            Users,
            VideoFile,
            cours,
            PaperPublisher,
            Paper,
            Group,
            Book,
            Slider,
            News,
            Author,
            Logo,
            CKEDITOR
        }
        internal string SaveFile(HttpPostedFileBase ImageFile, pathFile path)
        {
            string img = ImageFile?.FileName;
            if (ImageFile != null)
            {

                if (System.IO.File.Exists("/images/" + path + "/" + ImageFile.FileName))
                    img = Guid.NewGuid().ToString().Replace("-", "") +
                                        System.IO.Path.GetExtension(ImageFile.FileName);

                ImageFile.SaveAs(Server.MapPath("/images/" + path + "/" + img));
            }
            return img;
        }
        public string EditFile(HttpPostedFileBase ImageFile, pathFile path, string oldFileName)
        {
            if (ImageFile == null && !string.IsNullOrEmpty(oldFileName))
            {
                return oldFileName;
            }
            string img = ImageFile?.FileName;
            if (ImageFile != null)
            {
                if (!string.IsNullOrEmpty(oldFileName) && System.IO.File.Exists("/images/" + path + "/" + oldFileName))
                    System.IO.File.Delete(Server.MapPath("/images/" + path + "/" + oldFileName));

                if (System.IO.File.Exists("/images/" + path + "/" + ImageFile.FileName))
                    img = Guid.NewGuid().ToString().Replace("-", "") +
                                        System.IO.Path.GetExtension(ImageFile.FileName);

                ImageFile.SaveAs(Server.MapPath("/images/" + path + "/" + img));
            }
            return img;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                Current_learn_user = GetSessionUser();
                if (Current_learn_userId == 0)
                {
                    var user = UserBiz.Instance.FindByUserName(User.Identity.Name);
                    if (user == null)
                        Session.Clear();
                    else
                        SetSessionUser(user);
                }
                ViewBag.User = Current_learn_user;
            }
            else
            {
                //Session.Abandon();
                Session.Clear();
                ViewBag.User = new learn_user();
            }
            if (TempData["SuccessMessage"] != null)
            {
                SetViewBagSuccessMessage(TempData["SuccessMessage"].ToString());
                TempData["SuccessMessage"] = null;
            }
            else if (TempData["ErrorMessage"] != null)
            {
                SetViewBagErrorMessage(TempData["ErrorMessage"].ToString());
                TempData["ErrorMessage"] = null;
            }
            ViewBag.SiteSetting = SiteSetting.GetSetting.Instance.Get();
            base.OnActionExecuting(filterContext);
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //       // db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        protected override void OnException(ExceptionContext filterContext)
        {
            var innerEx = filterContext.Exception;
            //var ex1 = filterContext.Exception;

            if (innerEx is Exception)
            {
                //var innerEx = ex;
                while (innerEx.InnerException != null && innerEx.InnerException.Message != null)
                {
                    innerEx = innerEx.InnerException;
                }
                if (innerEx != null && innerEx.Message != null && innerEx.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    //var d = reverseStringFormat("The DELETE statement conflicted with the REFERENCE constraint \"{0}\". The conflict occurred in database \"{1}\", table \"{2}\", column '{3}'.\r\nThe statement has been terminated.", innerEx.Message);


                    //var iSqlTableFieldTitleBiz = DependencyResolver.Current.GetService<ISqlTableFieldTitleBiz>();
                    //اگر نوع خطا حذف به دلیل وابستگی باشد ، نام جدول و نام فیلد لود و سپس از عنوان های آنها از جدولی در دیتابیس آورده می شود تا پیغام مناسب داده شود
                    var exMessageSplitedQuoute = innerEx.Message.Split('"');
                    var exMessageSplitedOneQuote = innerEx.Message.Split('\'');
                    //if (exMessageSplitedQuoute.Length > 5 && exMessageSplitedOneQuote.Length > 2)
                    //{
                    //    var sqlTableField = iSqlTableFieldTitleBiz.FindSqlTableFieldTitle(exMessageSplitedQuoute[5], exMessageSplitedOneQuote[1]);
                    //    if (sqlTableField != null)
                    //        innerEx = new BusinessException("به علت وابستگی به جدول " + sqlTableField.TableTitle + " فیلد " + sqlTableField.FieldTitle + " حذف امکان پذیر نمی باشد.");
                    //    else
                    //        innerEx = new BusinessException("به علت وابستگی به جدول " + exMessageSplitedQuoute[5] + " فیلد " + exMessageSplitedOneQuote[1] + " حذف امکان پذیر نمی باشد.");
                    //}
                    //else
                    //    innerEx = new BusinessException("خطای وابستگی ");


                }
            }

            if (Request.IsAjaxRequest())
            {

                var isJson = Request.AcceptTypes != null && Request.AcceptTypes.Any(a => a.Contains("json"));
                if (isJson)
                {
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                    //var ErrorObject = (innerEx is BusinessException) ? ((BusinessException)innerEx).ErrorObject : null;

                    //context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    //await context.Response.WriteAsync(exception.Message);
                    filterContext.Result = new JsonResult
                    {
                        Data = new { success = false, State = "NOK", ErrorMessage = innerEx.Message, InnerExceptionMessage = innerEx.InnerException?.Message },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.ExceptionHandled = true;

                    var res = new JavaScriptResult();
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    //if (innerEx is BusinessException)
                    //{
                    res.Script = "SwalErrorMessage('" + innerEx.Message + "');";
                    //}
                    //else
                    //{
                    //    res.Script = "SwalErrorMessage('<div>خطای سمت سرور رخ داد. لطفا با پشتیبانی تماس بگیرید.</div><div style=\"display:none;\">" + HttpUtility.JavaScriptStringEncode(innerEx.Message.Replace("'", "\"")) + "</div>');";
                    //}
                    filterContext.Result = res;
                }

            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                //if (innerEx is BusinessException)
                //filterContext.Result = new RedirectResult("/Error/Error?ErrorMessage=" + innerEx.Message);
                //else
                //{
                //    TempData["_UnknownErrorMessage"] = innerEx.Message;
                //    TempData["_UnknownErrorMessageStack"] = innerEx.StackTrace;
                //    TempData["_UnknownInnerErrorMessage"] = innerEx.InnerException != null ? innerEx.InnerException.Message : "";
                TempData["ErrorMessage"] = innerEx.Message;
                //filterContext.Controller.ViewData.Model = viewModel;
                //filterContext.Result = new ViewResult { ViewName = "Login", ViewData = new ViewDataDictionary(viewModel) };
                //filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult("/");
                //}
            }

            //if (!(ex1 is BusinessException))
            //{

            //    //Get a StackTrace object for the exception
            //    StackTrace st = new StackTrace(ex1, true);

            //    var frame = st.GetFrames() == null ? null : st.GetFrames().Where(a => a.GetMethod() != null && a.GetMethod().Module != null && a.GetMethod().Module.Name != null && a.GetMethod().Module.Name.Contains("Business")).FirstOrDefault();
            //    if (frame != null)
            //    {

            //        //Get the file name
            //        string fileName = frame.GetFileName();

            //        //Get the method name
            //        string methodName = frame.GetMethod().Name;

            //        //Get the line number from the stack frame
            //        int line = frame.GetFileLineNumber();

            //        //Get the column number
            //        int col = frame.GetFileColumnNumber();
            //        if (!string.IsNullOrEmpty(ex.Message))
            //            Business.Log.ExceptionLog.Instance.Add(new BaseSystemModel.Model.Log.ExceptionLog { CreatedDate = DateTime.Now, FileName = fileName, MethodName = methodName, LineNumber = line, ColNo = col, Body = ex.Message });
            //    }
            //}
            if (!regex.IsMatch(innerEx.Message))
            {

                try
                {
                    // string fileName = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "LogEXCEPTION.log");
                    //string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, "Log" + DateUtil.CurrentJalaliDate().Replace("/", "") + ".log"); 
                    string fileName = Server.MapPath("/LogException.log");
                    //if (!File.Exists(fileName))
                    using (var streamWriter = new StreamWriter(fileName, true, System.Text.Encoding.Unicode))
                    {
                        streamWriter.WriteLine(System.DateTime.Now.ToString() + " " + innerEx.Message + " " + innerEx.InnerException?.Message);
                        // streamWriter.WriteLine("---------------------------------------------------");
                    }

                }
                catch (Exception)
                {//
                }
            }

            base.OnException(filterContext);
        }

        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, bool rtl = true)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, null, null, null, null, null, null, null, rtl);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, HttpResponseBase response)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, null, null, response);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, string gridHeader)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, gridHeader, null, null);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, string gridHeader, int? decimalplaces)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, gridHeader, decimalplaces, null);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, int? decimalplaces)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, null, decimalplaces, null);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, int? decimalplaces, HttpResponseBase response)
        {
            return PrintListToExcel(list, gridColumnModelList, worksheetCaption, null, decimalplaces, response);
        }
        [NonAction]
        public static ActionResult PrintListToExcel<T>(List<T> list, List<GridColumnModel> gridColumnModelList, string worksheetCaption, string gridHeader, int? decimalplaces, HttpResponseBase response, bool? showSumRow = null, bool? showRowNumber = null, bool? hasFilter = null, bool? setBlueOneLastBackgroundColorRow = null, bool rtl = true)
        {
            //Error: worksheet names cannot be more than 31 characters
            if (worksheetCaption.Length > 31)
                worksheetCaption = worksheetCaption.Substring(0, 31);
            if (!list.Any())
                throw new Exception("رکوردی یافت نشد");
            var wb = new XLWorkbook { RightToLeft = rtl };
            var ws = wb.Worksheets.Add(worksheetCaption);

            //add logo
            var setting = SiteSetting.GetSetting.Instance.Get();
            ws.AddPicture(System.AppDomain.CurrentDomain.BaseDirectory + setting.LogoUrl, setting.Name);

            //title
            ws.Cell("D1").Value = "بنام خدا ";
            ws.Cell("D1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell("D1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            ws.Cell("D1").Style.Font.Bold = true;
            ws.Cell("D1").Style.Font.SetFontName("B Nazanin");
            ws.Cell("D1").Style.Font.FontSize = 20;

            //date
            ws.Cell("G1").Value = DateTimeExtensions.DateTimeNowPersian();

            var letter = showRowNumber != null && showRowNumber.Value ? 'A' : '@';
            var prefixLetter = ' ';
            var columnLetter = showRowNumber != null && showRowNumber.Value ? "B" : "A";
            var numberCols = new List<char>();

            if (showRowNumber != null && showRowNumber.Value)
                ws.Cell("A3").Value = "ردیف";

            for (var i = 0; i < gridColumnModelList.Count; i++)
            {
                if (gridColumnModelList[i].IsHidden || gridColumnModelList[i].Name == "act")
                    continue;

                var colRenderer = gridColumnModelList[i].GetColumnRenderer();

                if (letter == 'Z')
                {
                    letter = 'A';
                    if (prefixLetter == ' ')
                        prefixLetter = 'A';
                    else
                        prefixLetter = (char)(prefixLetter + 1);
                }
                else
                    letter = (char)(letter + 1);

                if (!numberCols.Contains(letter))
                {
                    if (colRenderer is NumberColumnRenderer)
                    {
                        numberCols.Add(letter);
                    }
                }
                columnLetter = prefixLetter.ToString().Trim() + letter + 3;//3
                ws.Cell(columnLetter).Value = gridColumnModelList[i].GetColumnCaption();
            }

            var dataRow = 4;//4
            list = list ?? new List<T>();

            var EntityType = list[0].GetType();
            var Properties = EntityType.GetProperties();

            foreach (T Entity in list)
            {
                letter = showRowNumber != null && showRowNumber.Value ? 'A' : '@';
                prefixLetter = ' ';

                if (showRowNumber != null && showRowNumber.Value)
                    ws.Cell("B" + dataRow).Value = dataRow - 3;//3

                for (int i = 0; i < gridColumnModelList.Count; i++)
                {
                    if (gridColumnModelList[i].IsHidden || gridColumnModelList[i].Name == "act") continue;

                    var gridColumnModel = gridColumnModelList[i].GetColumnRenderer();

                    if (letter == 'Z')
                    {
                        letter = 'A';
                        if (prefixLetter == ' ')
                            prefixLetter = 'A';
                        else
                            prefixLetter = (char)(prefixLetter + 1);
                    }
                    else
                        letter = (char)(letter + 1);

                    columnLetter = prefixLetter.ToString().Trim() + letter + dataRow;

                    var propertyV = Properties.FirstOrDefault(x => x.Name == gridColumnModelList[i].Name);
                    if (propertyV.PropertyType.Name == "String" || propertyV.PropertyType.Name == "string")
                    {
                        ws.Cell(columnLetter).SetValue<string>(propertyV?.GetValue(Entity, null)?.ToString());
                    }
                    else
                    {
                        ws.Cell(columnLetter).Value = propertyV?.GetValue(Entity, null);
                    }

                    if (gridColumnModelList[i].CellType == GridCellType.DECIMAL)
                    {
                        //ws.Cell(columnLetter).DataType = XLCellValues.Number;
                        var decimalPlaces = (gridColumnModel as NumberColumnRenderer)?._decimalPlaces ?? 0;
                        ws.Cell(columnLetter).Style.NumberFormat.Format = decimalPlaces == 0 ? "#,##0" : "#,##0." + "".PadLeft(decimalPlaces, '0');
                    }


                }
                dataRow++;
            }
            //--------------------------------- رديف جمع ------------------------------------
            if (showSumRow != null && showSumRow.Value)
            {
                letter = showRowNumber != null && showRowNumber.Value ? 'A' : '@';
                prefixLetter = ' ';

                if (showRowNumber != null && showRowNumber.Value)
                    ws.Cell("C" + dataRow).Value = "رديف جمع";
                else
                    ws.Cell("B" + dataRow).Value = "رديف جمع";

                foreach (var gridColumn in gridColumnModelList)
                {
                    if (gridColumn.IsHidden || gridColumn.Name == "act") continue;

                    if (letter == 'Z')
                    {
                        letter = 'A';
                        if (prefixLetter == ' ')
                            prefixLetter = 'A';
                        else
                            prefixLetter = (char)(prefixLetter + 1);
                    }
                    else
                        letter = (char)(letter + 1);

                    columnLetter = prefixLetter.ToString().Trim() + letter + dataRow;

                    if (!gridColumn.SumRecords || (gridColumn.CellType != GridCellType.DECIMAL && !(gridColumn.GetColumnRenderer() is NumberColumnRenderer))) continue;

                    var sumValue = (from Entity in list
                                    let propertyV = Properties.FirstOrDefault(x => x.Name == gridColumn.Name)
                                    select propertyV?.GetValue(Entity, null) into value
                                    where value != null
                                    select Convert.ToDecimal(value)).Sum();

                    //decimal sumValue = 0;
                    //if (gridColumn.CellType != GridCellType.DECIMAL) continue;

                    //foreach (T Entity in list)
                    //{
                    //    var propertyV = Properties.FirstOrDefault(x => x.Name == gridColumn.Name);
                    //    var value = propertyV?.GetValue(Entity, null);
                    //    if (value != null)
                    //        sumValue += Convert.ToDecimal(value);
                    //}

                    var gridColumnModel = gridColumn.GetColumnRenderer();
                    var decimalPlaces = (gridColumnModel as NumberColumnRenderer)?._decimalPlaces ?? 0;

                    ws.Cell(columnLetter).Value = sumValue;
                    //ws.Cell(columnLetter).DataType = XLCellValues.Number;
                    ws.Cell(columnLetter).Style.NumberFormat.Format = decimalPlaces == 0 ? "#,##0" : "#,##0." + "".PadLeft(decimalPlaces, '0');
                }
            }
            //---------------------------------- شماره رديف ---------------------------------
            if (showRowNumber != null && showRowNumber.Value)
            {
                var rngRowNumber = ws.Range("A1", "B" + dataRow);
                rngRowNumber.Style.Alignment.WrapText = true;
                rngRowNumber.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //rngRowNumber.Style.Font.Bold = true;
                rngRowNumber.Style.Font.SetFontName("B Nazanin");
                rngRowNumber.Style.Font.FontColor = XLColor.White;
                rngRowNumber.Style.Font.FontSize = 10;
                rngRowNumber.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
                //rngRowNumber.DataType = XLCellValues.Text;
            }
            //---------------------------------- رديف جمع ----------------------------------
            if ((showSumRow != null && showSumRow.Value))
            {
                var rngRowNumber = ws.Range("B" + dataRow, prefixLetter.ToString().Trim() + letter + dataRow);
                rngRowNumber.Style.Alignment.WrapText = true;
                //rngRowNumber.Style.Font.Bold = true;
                rngRowNumber.Style.Font.SetFontName("B Nazanin");
                rngRowNumber.Style.Font.FontColor = XLColor.White;
                rngRowNumber.Style.Font.FontSize = 10;
                rngRowNumber.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            }
            if ((setBlueOneLastBackgroundColorRow != null && setBlueOneLastBackgroundColorRow.Value))
            {
                var dataRowLast = dataRow - 1;
                var rngRowNumber = ws.Range("B" + dataRowLast, prefixLetter.ToString().Trim() + letter + dataRowLast);
                rngRowNumber.Style.Alignment.WrapText = true;
                //rngRowNumber.Style.Font.Bold = true;
                rngRowNumber.Style.Font.SetFontName("B Nazanin");
                rngRowNumber.Style.Font.FontColor = XLColor.White;
                rngRowNumber.Style.Font.FontSize = 10;
                rngRowNumber.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            }
            //--------------------------------------------------------------------------------

            var rngTable = ws.Range("A3", prefixLetter.ToString().Trim() + letter + "3");//3
            rngTable.Style.Alignment.WrapText = true;
            rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTable.Style.Font.Bold = true;
            rngTable.Style.Font.SetFontName("B Nazanin");
            rngTable.Style.Font.FontColor = XLColor.White;
            rngTable.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            //rngTable.DataType = XLCellValues.Text;

            //last row
            ws.Cell(letter.ToString() + (dataRow + 2)).Value = "محل امضاء مدیریت ";


            if (!string.IsNullOrEmpty(gridHeader))
            {
                var gridHeaderHeight = gridHeader.Split('\n').Length * 15;
                var rngHeaders = ws.Range("A1", prefixLetter.ToString().Trim() + letter + "1").Merge();
                rngHeaders.Style.Alignment.WrapText = true;
                rngHeaders.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                rngHeaders.Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                rngHeaders.Style.Font.Bold = true;
                rngHeaders.Style.Font.SetFontName("B Nazanin");
                rngHeaders.Style.Font.FontColor = XLColor.DarkBlue;
                rngHeaders.Style.Fill.BackgroundColor = XLColor.Aqua;
                ws.Row(1).Height = gridHeaderHeight;
                rngHeaders.Value = gridHeader;
            }

            if (hasFilter != null && hasFilter.Value)
            {
                var rngData = ws.Range("A1:" + columnLetter);
                var excelTable = rngData.CreateTable();
                excelTable.ShowTotalsRow = true;
                //ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            }

            ws.Columns().AdjustToContents(2);

            //for logo
            ws.FirstColumn().Width = 20;
            ws.FirstRow().Height = 85;

            var ms = new MemoryStream();
            wb.SaveAs(ms, false);
            var bytes = ms.ToArray();

            response?.AddHeader("content-disposition", "attachment; filename=Result.xlsx");
            var d = new FileContentResult(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return d;
        }
    }
}