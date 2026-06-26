using SenakLearn.Models;
using SenakLearn.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SenakLearn
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
            builder.AppendLine(" <script src='/ckeditor/ckeditor.js'></script>");
            builder.AppendLine(" <script src='/ckeditor/adapters/jquery.js'></script>");
            //builder.AppendLine(" <script src='/ckeditor/ckeditor5.js'></script>");
            //builder.AppendLine(" <script src='/ckeditor5-premium-features/ckeditor5-premium-features.js'></script>");
            builder.AppendLine(" <script>$('#" + propName + "').ckeditor();</script>");

            // builder.AppendLine(" <script src='/lib/ckeditor5/translations/fa.js'></script>");
            //builder.AppendLine(" <script src='/lib/ckeditor5/src/alignment.js'></script>");
            //builder.AppendLine(" <script src='/lib/ckeditor5/src/alignmentcommand.js'></script>");
            //builder.AppendLine(" <script src='/lib/ckeditor5/src/alignmentediting.js'></script>");
            //builder.AppendLine(" <script src='/lib/ckeditor5/src/alignmentui.js'></script>");
            //builder.AppendLine(" <script src='/lib/ckeditor5/src/utils.js'></script>");
            // builder.AppendLine("<script>ClassicEditor.create(document.querySelector('#"+ propName + @"'), {alignment: {
            //    options: ['left', 'right','center','justify']
            //},
            //plugins: [  Alignment ],
            //toolbar: [
            //    'heading', '|','alignment', 
            //    'bold',
            //    'italic',ad
            //    'link',
            //    'bulletedList',
            //    'numberedList',
            //    'blockQuote',
            //    'undo',
            //    'redo'
            //]
            //,language: 'fa'}).then(editor => {  window.editor = editor;}).catch (err => {console.error(err.stack);});</script> ");
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
                builder.AppendLine("<script src='/Scripts/Jquery/multi-select/jquery.multiselect-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<script src='/Scripts/Jquery/multi-select/jquery.multiselect.filter-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<link href='/Content/Jquery/multi-select/jquery.multiselect-fa.css' rel='stylesheet' type='text/css' />");
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
            }
            else
            {
                var ddl = helper.ListBox(propName, new SelectList(listDdl, idDdl, nameDdl, selectedValue), htmlAttributesDic);
                builder.Append(ddl);
                builder.AppendLine("<script src='/Scripts/Jquery/multi-select/jquery.multiselect-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<script src='/Scripts/Jquery/multi-select/jquery.multiselect.filter-fa.js' type='text/javascript'></script>");
                builder.AppendLine("<link href='/Content/Jquery/multi-select/jquery.multiselect-fa.css' rel='stylesheet' type='text/css' />");
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
        public static MvcHtmlString UserForTeacher_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.learn_user.Where(x => x.status && x.RoleId == Roles.Teacher).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.user_name }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString OnlineClassTypeDropDownFor<TModel, TItemType>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TItemType>> codeExpression, bool isChosen, string placeHolder = "انتخاب کنید", object htmlAttributes = null)
        {
            List<SelectListItem> list = EnumExtention.GetEnumsProperty<Enums.OnlineClassType>().Select(x => new SelectListItem
            {
                Text = x.Key,
                Value = x.Value.ToString()
            }).ToList();
            return ForDropDown(helper, codeExpression, htmlAttributes, "Value", "Text", list, false, true, isChosen, placeHolder);
        }
        public static MvcHtmlString Cours_Group_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null, bool? online = null, bool? offline = null, bool? paper = null, bool? book = null, bool? booklet = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.learn_cours_group.Where(x => x.status && (book == null || book == x.Book) && (booklet == null || booklet == x.Booklet) && (online == null || online == x.Online) && (offline == null || offline == x.Offline) && (paper == null || paper == x.Paper)).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.name }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString Cours_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.learn_cours.Where(x => x.status).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.name }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }

        public static MvcHtmlString OnlineClass_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.OnlineClasses.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.name }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }

        public static MvcHtmlString Book_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.Book.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.TitleF }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }

        public static MvcHtmlString Booklet_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.Paper.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.TitleF }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString Teacher_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", int? selected = null)
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.learn_teacher.Where(x => x.status).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.name + " " + i.family }).ToList();
                return DropDown(helper, codeExpression, selected, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        //public static MvcHtmlString File_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        //{
        //    using (SWEntities db = new SWEntities())
        //    {
        //        var list = db.learn_file.Where(x => x.status).Select(i => new SelectListItem() { Value = i.id.ToString(), Text = i.titel }).ToList();
        //        return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
        //    }
        //}
        public static MvcHtmlString Video_DropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.VideoFiles.Select(i => new SelectListItem() { Value = i.VideoId.ToString(), Text = i.titel }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString OnlineClassAccorationDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            using (SWEntities db = new SWEntities())
            {
                var list = db.OnlineClassAccorations.Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name }).ToList();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString OfflineVideoDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید", object selectedValue = null)
        {
            //using (SWEntities db = new SWEntities())
            {
                // var list = db.OfflineVideo.Where(x=>x.ParentId==null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Description }).ToList();
                List<SelectListItem> list;
                if (selectedValue != null)
                    list = Biz.OfflineVideoBiz.Instance.DropDown((int)selectedValue);
                else
                {
                    list = Biz.OfflineVideoBiz.Instance.DropDown();
                    list.Insert(0, new SelectListItem { Text = "", Value = null });
                }
                return DropDown(helper, codeExpression, selectedValue, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString StudentSupportDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            //using (SWEntities db = new SWEntities())
            {
                // var list = db.StudentSupport.Where(x => x.ParentId == null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Description }).ToList();
                var list = Biz.StudentSupportBiz.Instance.DropDown();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString TeacherSupportDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            //  using (SWEntities db = new SWEntities())
            {
                //   var list = db.TeacherSupport.Where(x => x.ParentId == null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Description }).ToList();

                var list = Biz.TeacherSupportBiz.Instance.DropDown();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
        }
        public static MvcHtmlString OnlineClassAccorationDetialDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            //using (SWEntities db = new SWEntities())
            {
                // var list = db.OnlineClassAccorationDetails.Where(x => x.ParentId == null).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Description }).ToList();
                var list = Biz.OnlineClassAccorationDetailsBiz.Instance.DropDown();
                return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, false, isRequerd, true, placeHolder);
            }
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
        public static MvcHtmlString PaperTranslateQualityDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperTranslateQualityBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString PaperUniversityDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperUniversityBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString PaperJournalDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperJournalBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString PaperPublisherDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperPublisherBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString PaperFieldDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperFieldBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString PaperTrendDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.PaperTrendBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }

        public static MvcHtmlString UserGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = true, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.GroupBiz.Instance.DropDown();
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

        public static MvcHtmlString RoleDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.RoleBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
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
        public static MvcHtmlString SurveyPrivateGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.SurveyPrivateGroupBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }


        public static MvcHtmlString GroupAzmoonDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.GroupAzmoonBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString AzmoonGroupQuestionDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.AzmoonGroupQuestionBiz.Instance.DropDown();
            return DropDown(helper, codeExpression, null, htmlAttributes, "Value", "Text", list, IsMultiSelect, isRequerd, true, placeHolder);
        }
        public static MvcHtmlString AzmoonPrivateGroupDropDown(this HtmlHelper helper, string codeExpression, bool isRequerd, bool IsMultiSelect = false, object htmlAttributes = null, string placeHolder = "انتخاب کنید")
        {
            var list = Biz.AzmoonPrivateGroupBiz.Instance.DropDown();
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

    }

}