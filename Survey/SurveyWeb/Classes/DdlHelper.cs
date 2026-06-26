using SurveyWeb.Biz;
using SurveyWeb.Biz.CheckList;
using SurveyWeb.Models;
using SurveyWeb.Models.Resturan;
using SurveyWeb.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SurveyWeb
{
    public static class DdlHelper
    {
        public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {

            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            return MvcHtmlString.Create($"<a title='{description}'><i class='fa fa-question'> </i></a>");
        }

        public static MvcHtmlString ClassicEditorFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            StringBuilder builder = new StringBuilder();
            var ddl = self.TextAreaFor(expression, htmlAttributes);
            builder.Append(ddl);
            string propName = ((MemberExpression)expression.Body).Member.Name;
            builder.AppendLine(" <script src='/lib/ckeditor/ckeditor.js'></script>");
            builder.AppendLine(" <script src='/lib/ckeditor/adapters/jquery.js'></script>");
            builder.AppendLine(" <script>$('#" + propName + "').ckeditor();</script>");
            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString ClassicEditor(this HtmlHelper helper, string propName)
        {
            StringBuilder builder = new StringBuilder();
            var ddl = $"<textarea id='{propName}' class='form-control'></textarea>";
            builder.Append(ddl);
            builder.AppendLine(" <script src='/lib/ckeditor/ckeditor.js'></script>");
            builder.AppendLine(" <script src='/lib/ckeditor/adapters/jquery.js'></script>");
            builder.AppendLine(" <script>$('#" + propName + "').ckeditor();</script>");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ShamsiDateFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            StringBuilder builder = new StringBuilder();
            //@if(Model == null)
            //    {
            //            < input type = "text" class="form-control MyDate  " data-placement="left" id="BirthDayShamsi" name="BirthDayShamsi" placeholder="تاریخ" data-mddatetimepicker="true" data-placement="right" data-englishnumber="true" />
            //        }
            //        else
            //        {
            //            <input type = "text" class="form-control MyDate " data-placement="left" id="BirthDayShamsi" name="BirthDayShamsi" placeholder="تاریخ" data-mddatetimepicker="true" data-placement="right" data-englishnumber="true" value="@Model.BirthDayShamsi" />
            //        }
            //        <div class="input-group-addon" data-mddatetimepicker="true" data-targetselector="#BirthDayShamsi" data-trigger="click" data-enabletimepicker="true">
            //            <span class="glyphicon glyphicon-calendar"></span>
            //        </div>
            //    </div>
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString FileChosenFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, SurveyWeb.Controllers.BaseController.pathFile type, object htmlAttributes)
        {
            StringBuilder builder = new StringBuilder();
            string propName = ((MemberExpression)expression.Body).Member.Name;
            //builder.AppendLine("<label class="btn btn-default"> انتخاب فایل   <input @*accept=".pdf, .doc , .docx"  accept="video/*"*@ accept="image/*" class="fileChosen" type="file" hidden id="File" name="File"></label><span class="fileChosenText">
            //                @if (Model != null && !string.IsNullOrEmpty(Model.ImageUrl))
            //                {
            //                    <a href="~/images/news/@Model.ImageUrl"><img src="~/images/news/@Model.ImageUrl" title="دانلود" height="50" /></a>
            //                }
            //            </span>
            //            @Html.HiddenFor(x => x.ImageUrl)");
            //builder.AppendLine(" <script src='/lib/ckeditor5//translations/fa.js'></script>");
            //builder.AppendLine("<script>ClassicEditor.create(document.querySelector('#" + propName + "'), {language: 'fa', toolbar: [ 'heading', '|', 'bulletedList', 'numberedList', 'alignment:left', 'alignment:right', 'alignment:center', 'alignment:justify', 'undo', 'redo' ]}).then(editor => {  window.editor = editor;}).catch (err => {console.error(err.stack);});</script> ");
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString ForDropDown<TModel, TItemType>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TItemType>> expression,
               object htmlAttributes, string idDdl, string nameDdl, IEnumerable<object> listDdl, bool IsMultiSelect = false, bool isRequired = true, bool isChosen = true, string placeHolder = "انتخاب کنید")
        {
            var htmlAttributesDic = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) ?? new Dictionary<string, object>();
            string propName = ((MemberExpression)expression.Body).Member.Name;
            StringBuilder builder = new StringBuilder();
            if (!IsMultiSelect)
            {
                if (isChosen)
                {
                    if (htmlAttributesDic.Keys.Contains("class"))
                        htmlAttributesDic["class"] = htmlAttributesDic["class"] + " chosen-select chosen-rtl";
                    else
                        htmlAttributesDic.Add("class", " chosen-select chosen-rtl");
                }
                if (isRequired)
                {
                    //builder.AppendLine("<span style=\"color:red\">*</span>");
                    var ddl = helper.DropDownListFor(expression, new SelectList(listDdl, idDdl, nameDdl), htmlAttributesDic);
                    builder.Append(ddl);
                }
                else
                {
                    var ddl = helper.DropDownListFor(expression, new SelectList(listDdl, idDdl, nameDdl), placeHolder, htmlAttributesDic);
                    builder.Append(ddl);
                    builder.AppendLine(@"<script>$(function () {$('#" + propName + "').attr('data-placeholder','" + placeHolder + "');$('.chosen-select').chosen({allow_single_deselect: true, search_contains: true});});</script>");
                }
            }
            else
            {
                var ddl = helper.ListBoxFor(expression, new SelectList(listDdl, idDdl, nameDdl), htmlAttributesDic);
                builder.Append(ddl);
                builder.AppendLine("<script src='/lib/Jquery/multi-select/jquery.multiselect-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<script src='/lib/Jquery/multi-select/jquery.multiselect.filter-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<link href='/lib/Jquery/multi-select/jquery.multiselect-fa.css' rel='stylesheet' type='text/css' />");
                builder.AppendLine(@"<script>$(function () {$('#" + propName + "').multiselect({ isOpen: true,keepOpen: true}).multiselectfilter();});</script>");
            }
            builder.AppendLine();
            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString DropDown(this HtmlHelper helper, string propName, object selectedValue,
            object htmlAttributes, string idDdl, string nameDdl, IEnumerable<object> listDdl, bool IsMultiSelect = false, bool isRequired = true, bool isChosen = true, string placeHolder = "انتخاب کنید")
        {
            var htmlAttributesDic = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes) ?? new Dictionary<string, object>();
            StringBuilder builder = new StringBuilder();
            if (!IsMultiSelect)
            {
                if (isRequired)
                {
                    //builder.AppendLine("<span style=\"color:red\">*</span>");
                    var ddl = helper.DropDownList(propName, new SelectList(listDdl, idDdl, nameDdl, selectedValue), htmlAttributesDic);
                    builder.Append(ddl);
                }
                else
                {
                    //htmlAttributesDic["class"] = htmlAttributesDic["class"] + " chosen-single chosen-single-with-deselect chosen-default";
                    var ddl = helper.DropDownList(propName, new SelectList(listDdl, idDdl, nameDdl, selectedValue), placeHolder, htmlAttributesDic);
                    builder.Append(ddl);
                    //builder.AppendLine("<script src='/Scripts/jquery-3.2.1.min.js'></script>");
                    //builder.AppendLine("<link href='/Scripts/chosen.min.css' rel='stylesheet' />");
                    //builder.AppendLine("<script src='/Scripts/chosen.jquery.min.js'></script>");
                    //builder.AppendLine(@"<script>jQuery(function () {$('#" + propName + "').attr('data-placeholder','" + placeHolder + "');jQuery('.chosen-select').chosen({allow_single_deselect: true, search_contains: true, no_results_text: 'موردی یافت نشد!' ,rtl: true});});</script>");
                }
                if (isChosen)
                {
                    builder.AppendLine("<script>$(function () {$('#" + propName + "').select2({language:'fa'})});</script>");
                }
            }
            else
            {
                var ddl = helper.ListBox(propName, new SelectList(listDdl, idDdl, nameDdl, selectedValue), htmlAttributesDic);
                builder.Append(ddl);
                builder.AppendLine("<script src='/lib/Jquery/multi-select/jquery.multiselect-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<script src='/lib/Jquery/multi-select/jquery.multiselect.filter-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<link href='/lib/Jquery/multi-select/jquery.multiselect-fa.css' rel='stylesheet' type='text/css' />");
                builder.AppendLine(@"<script>$(function () {$('#" + propName + "').multiselect({ isOpen: true,keepOpen: true}).multiselectfilter();});</script>");
            }
            builder.AppendLine();
            return new MvcHtmlString(builder.ToString());
        }
        public static MvcHtmlString BooleanDropDownFor<TModel, TItemType>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TItemType>> codeExpression, string trueValue, string falseValue, bool isChosen, string placeHolder = "انتخاب کنید", object htmlAttributes = null)
        {
            var transportTypeList = new[]
            {
                new {ID =  true, Name = trueValue},
                new {ID =  false, Name = falseValue}
            };
            return ForDropDown(helper, codeExpression, htmlAttributes, "ID", "Name", transportTypeList, false, true, isChosen, placeHolder);
        }
        public static MvcHtmlString BooleanDropDown(this HtmlHelper helper, string codeExpression, string trueValue, string falseValue, bool isChosen, string placeHolder = "انتخاب کنید", object htmlAttributes = null)
        {
            var monthCodeList = new[]
                {
                    new {ID =  true, Name = trueValue},
                new {ID =  false, Name = falseValue}
                };

            return DropDown(helper, codeExpression, null, htmlAttributes, "ID", "Name", monthCodeList, false, true, isChosen, placeHolder);
        }
        //public static MvcHtmlString UserForTeacher_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        //{
        //    using (SWEntities db = new SWEntities())
        //    {
        //        var list = db.learn_user.Where(x => x.status && x.id_Role == 4).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.user_name }).ToList();
        //        return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
        //    }
        //}
        //public static MvcHtmlString OnlineClassTypeDropDownFor<TModel, TItemType>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TItemType>> codeExpression, bool isChosen, string placeHolder = "انتخاب کنید", object htmlAttributes = null)
        //{
        //    var transportTypeList = new[]
        //   {
        //        new {ID =  0, Name = OnlineClass.ClassType0},
        //        new {ID =  1, Name = OnlineClass.ClassType1},
        //        new {ID =  2, Name = OnlineClass.ClassType2},
        //        new {ID =  3, Name = OnlineClass.ClassType3}
        //    };
        //    return ForDropDown(helper, codeExpression, htmlAttributes, "ID", "Name", transportTypeList, false, true, isChosen, placeHolder);
        //}
        public static MvcHtmlString ResturantDegreeDropDown(this HtmlHelper helper, string codeExpression, object selected, object htmlAttributes = null, bool IsMultiSelect = false, bool isRequired = true)
        {
            var monthCodeList = new[]
                {
                     new  { Name = "یک ستاره", ID = "1" },
                    new  { Name = "دو ستاره", ID = "2" },
                    new  { Name = "سه ستاره", ID = "3" },
                    new  { Name = "چهار ستاره", ID = "4" },
                    new  { Name = "پنج ستاره", ID = "5" },
                };

            return DropDown(helper, codeExpression, selected, htmlAttributes, "ID", "Name", monthCodeList, IsMultiSelect, isRequired, false);
        }

        public static MvcHtmlString DaysDropDown(this HtmlHelper helper, string codeExpression, object htmlAttributes = null, bool IsMultiSelect = true)
        {
            var monthCodeList = new[]
                {
                    new {ID = "شنبه", Name = "شنبه"},
                    new {ID = "یکشنبه", Name = "یکشنبه"},
                    new {ID = "دوشنبه", Name = "دوشنبه"},
                    new {ID = "سه شنبه", Name = "سه شنبه"},
                    new {ID = "چهارشنبه", Name = "چهارشنبه"},
                    new {ID = "پنج شنبه", Name = "پنج شنبه"},
                    new {ID = "جمعه", Name = "جمعه"}
                };

            return DropDown(helper, codeExpression, null, htmlAttributes, "ID", "Name", monthCodeList, IsMultiSelect, false);
        }
        public static MvcHtmlString GroupSurveyDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.GroupSurveyBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString SurveyGroupQuestionDropDown(this HtmlHelper helper, string codeExpression, int SurveyEntityId, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.SurveyGroupQuestionBiz.Instance.DropDown(SurveyEntityId);
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString ProvinceDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<Province>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString QuestionTypeDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<Models.QuestionEnum>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString CartableDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.CartableBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString MenuDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.MenuBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
     
        public static MvcHtmlString UserDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool isAdmin, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.UserBiz.Instance.DropDown(isAdmin);
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString PermisstionDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<Permisstion>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString RoleSendSmsDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<Roles>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString RoleDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.RoleBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString SurveyPrivateGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.SurveyPrivateGroupBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString NewsGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.NewsGroupBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString AuthorGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.AuthorBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString CheckListDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.ResturantBiz.Instance.CheckListType();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString CheckListCartableDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.ResturantBiz.Instance.CheckListType();
            list.AddRange(EnumExtention.GetEnumsProperty<CartableCheckListType>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList());
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString CheckListGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = CheckListGroupBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString CheckListBizDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = CheckListBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString ResturantTypeDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.ResturantBiz.Instance.ResturantType();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, false, placeHolder);
        }

        public static MvcHtmlString ResturantMenuTypeDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<AdvertisingMenuType>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString ResturantDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب مرکزپذیرایی")
        {
            var list = Biz.ResturantBiz.Instance.ResturantSelectList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString EducationDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            using (var ctx = new Context())
            {
                var list = ctx.Educations.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString CompanyTypeDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, bool isChosen=true, string placeHolder = "انتخاب کنید")
        {
            using (var ctx = new Context())
            {
                var list = ctx.CompanyTypes.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.DropDownTitle }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, isChosen, placeHolder);
            }
        }

        public static MvcHtmlString CityDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, bool isChosen = true, string placeHolder = "انتخاب کنید")
        {
            using (var ctx = new Context())
            {
                var list = ctx.Citys.ToList().Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.ProvinceName + "/" + i.DropDownTitle }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, isChosen, placeHolder);
            }
        }

        public static MvcHtmlString PaymentTypeEnumDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<PaymentTypeEnum>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
    }

}