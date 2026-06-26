using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.Web.Mvc;
using System.Web.Mvc;

namespace MVC.Controls
{
    public enum FormSubmitButtonActionType
    {
        ADD,
        EDIT
    }
    public enum FormSubmitButtonType
    {
        ADD,
        EDIT,
        NEW,
        DELETE,
        PRINT,
        PDF,
        EXCEL,
        TempAdd
    }
    public class SubmitButtonControl
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string ButtonText { get; set; }
        public bool ValidateForm { get; set; }
        public FormSubmitButtonType ButtonType { get; set; }
        public IDictionary<string, object> htmlAttributes { get; set; }

        public SubmitButtonControl(string Url, string Name, string ButtonText, FormSubmitButtonType ButtonType, object htmlAttributes = null)
            : this(Url, Name, ButtonText, ButtonType, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
        {
        }
        public SubmitButtonControl(string Url, string Name, string ButtonText, FormSubmitButtonType ButtonType, IDictionary<string, object> htmlAttributes = null)
        {
            this.Url = Url;
            this.Name = Name;
            this.ButtonText = ButtonText;
            this.htmlAttributes = htmlAttributes;
            this.ButtonType = ButtonType;
        }

    }
    public class SubmitButtonControlList
    {
        public List<SubmitButtonControl> ButtonList = new List<SubmitButtonControl>();

        public SubmitButtonControlList SetButtonAdd(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.ADD, htmlAttributes);
            button.ValidateForm = true;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonAdd(string Url, string Name, object htmlAttributes = null)
        {
            SetButtonAdd(Url, Name, null, htmlAttributes);
            return this;
        }
        public SubmitButtonControlList SetButtonEdit(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.EDIT, htmlAttributes);
            button.ValidateForm = true;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonEdit(string Url, string Name, object htmlAttributes = null)
        {
            SetButtonEdit(Url, Name, null, htmlAttributes);
            return this;
        }
        public SubmitButtonControlList SetButtonDelete(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.DELETE, htmlAttributes);
            button.ValidateForm = false;
            ButtonList.Add(button);
            return this;

        }
        public SubmitButtonControlList SetButtonDelete(string Url, string Name, object htmlAttributes = null)
        {
            SetButtonDelete(Url, Name, null, htmlAttributes);
            return this;
        }
        public SubmitButtonControlList SetButtonNew(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.NEW, htmlAttributes);
            button.ValidateForm = false;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonNew(string Url, string Name, object htmlAttributes = null)
        {
            SetButtonNew(Url, Name, null, htmlAttributes);
            return this;
        }
        public SubmitButtonControlList SetButtonPrint(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.PRINT, htmlAttributes);
            button.ValidateForm = false;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonPdf(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.PDF, htmlAttributes);
            button.ValidateForm = false;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonExcel(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.EXCEL, htmlAttributes);
            button.ValidateForm = false;
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonTempAdd(string Url, string Name, string ButtonText, object htmlAttributes = null)
        {
            SubmitButtonControl button = new SubmitButtonControl(Url, Name, ButtonText, FormSubmitButtonType.TempAdd, htmlAttributes);
            
            ButtonList.Add(button);
            return this;
        }
        public SubmitButtonControlList SetButtonPrint(string Url, string Name, object htmlAttributes = null)
        {
            SetButtonPrint(Url, Name, null, htmlAttributes);
            return this;
        }
    }
    /// <summary>
    /// Multi submit button action
    /// </summary>
    public static class FromActionButtonExtension
    {
        public static MvcHtmlString FromActionButton(this HtmlHelper helper, SubmitButtonControlList submitButtonList, FormSubmitButtonActionType formSubmitButtonActionType)
        {
            return FromActionButton(helper, submitButtonList.ButtonList, formSubmitButtonActionType);
        }

        /// <summary>
        /// form action button
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="submitButtonList"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString FromActionButton(this HtmlHelper helper, List<SubmitButtonControl> submitButtonList, FormSubmitButtonActionType formSubmitButtonActionType)
        {
            //htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

            if (submitButtonList != null)
            {
                string htmlString = "";
                var mainSpanSubmit = new TagBuilder("span");
                mainSpanSubmit.Attributes.Add("class", "spanFormActionButton");
                foreach (var submitButton in submitButtonList)
                {
                    htmlString += loadButtonHtmlString(helper, submitButton.Url, submitButton.Name, submitButton.ButtonText, submitButton.ButtonType,
                        submitButton.htmlAttributes, formSubmitButtonActionType, submitButton.ValidateForm);
                    htmlString += " ";
                }
                var hiddenSubmit = new TagBuilder("input");
                hiddenSubmit.Attributes.Add("type", "submit");
                hiddenSubmit.Attributes.Add("class", "formActionButtonSubmit");
                hiddenSubmit.Attributes.Add("style", "display:none;");

                htmlString += " " + hiddenSubmit.ToString();
                return new MvcHtmlString(mainSpanSubmit.ToString(TagRenderMode.StartTag) + htmlString + mainSpanSubmit.ToString(TagRenderMode.EndTag));
            }

            return new MvcHtmlString("");
        }
        /// <summary>
        /// load button html as string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="buttonText"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static string loadButtonHtmlString(this HtmlHelper helper, string url, string name, string buttonText,
            FormSubmitButtonType ButtonType,
            IDictionary<string, object> htmlAttributes, FormSubmitButtonActionType formSubmitButtonActionType, bool ValidateForm)
        {
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            bool isDisabled = false;
            string baseUrl = ((HttpRuntime.AppDomainAppVirtualPath != null && HttpRuntime.AppDomainAppVirtualPath.Length > 1) ? (HttpRuntime.AppDomainAppVirtualPath + "/") : "/");
            
            bool disabled = false;
            bool isVisible = true;

            if (htmlAttributes.ContainsKey("editable"))
            {
                if (htmlAttributes["editable"] != null)
                {
                    var editable = true;
                    try
                    {
                        editable = Convert.ToBoolean(htmlAttributes["editable"]);
                    }
                    catch
                    {
                        // ignored
                    }
                    if (!editable)
                    {
                        disabled = true;
                    }
                }
            }
            if (htmlAttributes.ContainsKey("visible"))
            {
                if (htmlAttributes["visible"] != null)
                {
                    try
                    {
                        isVisible = Convert.ToBoolean(htmlAttributes["visible"]);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            if (!isVisible)
                return "";
            if (!disabled)
            {
                if (formSubmitButtonActionType == FormSubmitButtonActionType.ADD)
                {
                    if (ButtonType == FormSubmitButtonType.EDIT || ButtonType == FormSubmitButtonType.DELETE)
                    {
                        disabled = true;
                    }
                }
                else
                {
                    if (ButtonType == FormSubmitButtonType.ADD)
                    {
                        disabled = true;
                    }
                }
            }


            if (disabled)
            {
                if (htmlAttributes.ContainsKey("disabled"))
                {
                    htmlAttributes["disabled"] = "disabled";
                }
                else
                {
                    htmlAttributes.Add("disabled", "disabled");
                }
            }
            else
            {
                if (htmlAttributes.ContainsKey("disabled"))
                {
                    htmlAttributes["disabled"] = "false";
                }
            }


            string onClick = "";
            if (htmlAttributes.ContainsKey("onClick"))
            {
                htmlAttributes["onClick"] = //onClick + ";" + 
                    htmlAttributes["onClick"] + ";";
            }
            else
            {
                htmlAttributes.Add("onClick", onClick);
            }


            //var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var buttonBuilder = new TagBuilder("button");

            buttonBuilder.Attributes.Add("formUrl", url);
            buttonBuilder.Attributes.Add("targetUpdate", (htmlAttributes.ContainsKey("targetUpdate")) ? ("#" + htmlAttributes["targetUpdate"]) : "");
            buttonBuilder.Attributes.Add("validateForm", ValidateForm?"1":"0");

            var spanBuilderImg = new TagBuilder("span");
            spanBuilderImg.Attributes.Add("class", "spanImage");
            var spanBuilderTitle = new TagBuilder("span");
            spanBuilderTitle.Attributes.Add("class", "spanTitle");

            buttonBuilder.Attributes.Add("type", "submit");

            if (ButtonType == FormSubmitButtonType.ADD)
            {
                buttonBuilder.Attributes.Add("class", "_loading sanatFormButton sanatFormButtonAdd" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/add1.png)");
                //spanBuilderTitle.Attributes.Add("style", "text-indent: -1e+7px;padding:0.4em;display:block;");
                buttonBuilder.Attributes.Add("title", "اضافه");
            }
            else if (ButtonType == FormSubmitButtonType.EDIT)
            {
                buttonBuilder.Attributes.Add("class", "_loading sanatFormButton sanatFormButtonEdit" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/edit1.png)");
                buttonBuilder.Attributes.Add("title", "به روز رسانی");
            }
            else if (ButtonType == FormSubmitButtonType.DELETE)
            {
                buttonBuilder.Attributes.Add("class", "_loading sanatFormButton sanatFormButtonDelete cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/trash1.png)");
                buttonBuilder.Attributes.Add("title", "حذف");
            }
            else if (ButtonType == FormSubmitButtonType.NEW)
            {
                buttonBuilder.Attributes.Add("class", "_loading sanatFormButton sanatFormButtonNew cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/newRecord1.png)");
                buttonBuilder.Attributes.Add("title", "جدید");
            }
            else if (ButtonType == FormSubmitButtonType.PRINT)
            {
                buttonBuilder.Attributes.Add("class", "sanatFormButton sanatFormButtonPrint cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/print1.png)");
                buttonBuilder.Attributes.Add("title", "چاپ");
            }
            else if (ButtonType == FormSubmitButtonType.PDF)
            {
                buttonBuilder.Attributes.Add("class", "sanatFormButton sanatFormButtonPdf cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/print1.png)");
                buttonBuilder.Attributes.Add("title", "پی دی اف");
            }
            else if (ButtonType == FormSubmitButtonType.EXCEL)
            {
                buttonBuilder.Attributes.Add("class", "sanatFormButton sanatFormButtonExcel cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/print1.png)");
                buttonBuilder.Attributes.Add("title", "اکسل");
            }
            else if (ButtonType == FormSubmitButtonType.TempAdd)
            {
                buttonBuilder.Attributes.Add("class", "sanatFormButton sanatFormButtonTempAdd cancel" + (disabled ? " btnDisabled" : ""));
                //spanBuilderImg.Attributes.Add("style", "left: 5px; margin-left: -8px; position: absolute; top: 6px; margin-top: -8px; width: 73px; height:35px; background-image: url(/Images/Common/48x48/print1.png)");
                buttonBuilder.Attributes.Add("title", "ثبت موقت");
            }
            else
            {
                buttonBuilder.SetInnerText(buttonText);
            }
            if (!disabled )
            {
                if (htmlAttributes["onClick"] != null)
                    buttonBuilder.Attributes.Add("onclick", htmlAttributes["onClick"].ToString());
            }
            else
            {
                //buttonBuilder.Attributes.Add("style", "cursor:default");
                buttonBuilder.Attributes.Add("onclick", "return false;");
            }

            buttonBuilder.Attributes.Add("name", name);
            buttonBuilder.Attributes.Add("id", name);
          
            var submitBtn = helper.SubmitButton(name + "BTN", buttonText, htmlAttributes).ToString();

            return buttonBuilder.ToString(TagRenderMode.StartTag) + spanBuilderImg.ToString(TagRenderMode.StartTag) + spanBuilderImg.ToString(TagRenderMode.EndTag) + spanBuilderTitle.ToString(TagRenderMode.StartTag) + spanBuilderTitle.ToString(TagRenderMode.EndTag) + buttonBuilder.ToString(TagRenderMode.EndTag);
            //return helper.SubmitButton(name, buttonText, htmlAttributes).ToString();
        }

    }
}