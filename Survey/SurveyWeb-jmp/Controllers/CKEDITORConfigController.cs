using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SenakLearn.Controllers.Admin
{
    public class CKEDITORConfigController : BaseController
    {
        const string basePath = @"D:\CKFinder\ckfinder\userfiles\";
        const string baseUrl = @"/ckfinder/userfiles/";

        const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";

        public ActionResult Index()
        {
            var funcNum = 0;
            int.TryParse(Request["CKEditorFuncNum"], out funcNum);

            if (Request.Files == null || Request.Files.Count < 1)
                return BuildReturnScript(funcNum, null, "No file has been sent");

            if (!System.IO.Directory.Exists(basePath))
                return BuildReturnScript(funcNum, null, "basePath folder doesn't exist");

            var receivedFile = Request.Files[0];

            var fileName = receivedFile.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return BuildReturnScript(funcNum, null, "File name is empty");
            }

            var sFileName = System.IO.Path.GetFileName(fileName);

            var nameWithFullPath = System.IO.Path.Combine(basePath, sFileName);
            //Note: you may want to consider using your own naming convention for files, as this is vulnerable to overwrites
            //e.g. at the moment if two users uploaded a file called image1.jpg, one would clash with the other.
            //In the past, I've used Guid.NewGuid() combined with the file extension to ensure uniqueness.
            receivedFile.SaveAs(nameWithFullPath);

            var url = baseUrl + sFileName;

            return BuildReturnScript(funcNum, url, null);
        }
        const string filesavepath = "~/Content/Uploads/Ckeditor";
        //const string baseUrl = @"/Content/Uploads/Ckeditor/";

        //const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";

        public ActionResult Index2()
        {
            var funcNum = 0;
            int.TryParse(Request["CKEditorFuncNum"], out funcNum);

            if (Request.Files == null || Request.Files.Count < 1)
                return BuildReturnScript(funcNum, null, "No file has been sent");

            string fileName = string.Empty;
            SaveAttatchedFile(filesavepath, Request, ref fileName);
            var url = baseUrl + fileName;

            return BuildReturnScript2(funcNum, url, null);
        }

        private ContentResult BuildReturnScript2(int functionNumber, string url, string errorMessage)
        {
            return Content(
                string.Format(scriptTag, functionNumber, HttpUtility.JavaScriptStringEncode(url ?? ""), HttpUtility.JavaScriptStringEncode(errorMessage ?? "")),
                "text/html"
                );
        }

        private void SaveAttatchedFile(string filepath, HttpRequestBase Request, ref string fileName)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    string targetPath = Server.MapPath(filepath);
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    fileName = Guid.NewGuid() + fileName;
                    string fileSavePath = Path.Combine(targetPath, fileName);
                    file.SaveAs(fileSavePath);
                }
            }
        }


        private ContentResult BuildReturnScript(int functionNumber, string url, string errorMessage)
        {
            return Content(
                string.Format(scriptTag, functionNumber, HttpUtility.JavaScriptStringEncode(url ?? ""), HttpUtility.JavaScriptStringEncode(errorMessage ?? "")),
                "text/html"
                );
        }
        public ActionResult filebrowserBrowseUrl()
        {
            return null;
        }
        public ActionResult filebrowserImageBrowseUrl()
        {
            return null;
        }
        public ActionResult filebrowserUploadUrl()
        {
            return null;
        }
        public ActionResult filebrowserImageUploadUrl()
        {
            return null;
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor,
          string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
               // string ex = Path.GetExtension(upload.FileName).ToLower();
                if (upload.ContentType.ToLower().Contains("image"))
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") +
                                        Path.GetExtension(upload.FileName).ToLower();
                        var vFolderPath = Server.MapPath("/images/CKEDITOR/");
                        if (!Directory.Exists(vFolderPath))
                        {
                            Directory.CreateDirectory(vFolderPath);
                        }
                        vFilePath = Path.Combine(vFolderPath, vFileName);
                        upload.SaveAs(vFilePath);
                        vImagePath = Url.Content("/images/CKEDITOR/" + vFileName);
                        vMessage = "Image was saved correctly";
                    }
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }
    }
}