using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Presentation;
using MVC.Controls;
using SenakLearn.JqGrid;
using SenakLearn.JqGrid.Common;
using SenakLearn.Models;
using SenakLearn.Models.Common;
using SenakLearn.Models.Person;
using SenakLearn.Models.wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ISImage = SixLabors.ImageSharp.Image;
using ISSize = SixLabors.ImageSharp.Size;

namespace SenakLearn.Biz
{
    public class AzmoonUserAnswerBiz : RepositoryBaseSurvey<Models.AzmoonUserAnswer>
    {
        public static readonly AzmoonUserAnswerBiz Instance = new AzmoonUserAnswerBiz();
        public async Task<int> GetTotalCount(int idAzmoonEntityId)
        {
            using (var ctx = new SWEntities())
            {
                return await ctx.AzmoonUserAnswer.CountAsync(x => x.AzmoonEntityId == idAzmoonEntityId);
            }
        }
        public List<SurveyUserAnswerVM> GetListVm( int? azmoonEntityId)
        {
            using (var ctx = new SWEntities())
            {
                var list = (from u in ctx.AzmoonUserAnswer
                            join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                            join lu in ctx.learn_user on u.UserId equals lu.id into uuu
                            from user in uuu.DefaultIfEmpty()
                            select new SurveyUserAnswerVM()
                            {
                                Id = u.Id,
                                Ip = u.Ip,
                                SurveyEntity = e.Name,
                                CorrectAnswerd = u.CorrectAnswerd,
                                NoAnswerd = u.NoAnswerd,
                                WrongAnswerd = u.WrongAnswerd,
                                User = user.Name + " " + user.Family,
                                UserName = user.user_name,
                                TotalScore = u.TotalScore,
                                TotalRank = u.TotalRank,
                                TotalCorrectScore = e.TotalScore,
                                maxScore = e.MaxScore,
                                minScore = e.MinScore,
                                zaribManfi = e.ZaribManfi,
                                AzmounDate = u.CreatedDate,
                                AzmoonEntityId = e.Id,
                            });
                if (azmoonEntityId != null)
                    list = list.Where(w => w.AzmoonEntityId == azmoonEntityId);
                List<SurveyUserAnswerVM> result = new List<SurveyUserAnswerVM>();
                result = list.ToList();
                return result;
            }
        }
        public SenakLearn.JqGrid.PagedList<SurveyUserAnswerVM> GetAllPagedListVm(GridSettings grid, int? azmoonEntityId)
        {

                var list = GetListVm(azmoonEntityId).AsQueryable();
                var result = list.FilterAndSortJqGrid<SurveyUserAnswerVM>(grid).ToPagedList<SurveyUserAnswerVM>(grid);
                return result;
            
        }
        public MemoryStream GetAllReportExcel(int? entityId)
        {
            var list = GetListVm(entityId);

            var excelService = new ExcelService();
            var memoryStream = excelService.GenerateExcelFile(list);
            return memoryStream;

        }

        public SenakLearn.JqGrid.PagedList<SurveyUserAnswerVM> GetAllAzmoonByUserId(GridSettings grid, int current_learn_userId)
        {
            SenakLearn.JqGrid.PagedList<SurveyUserAnswerVM> list;
            using (var ctx = new SWEntities())
                list = (from u in ctx.AzmoonUserAnswer.Where(x => x.UserId == current_learn_userId)
                        join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                        select new SurveyUserAnswerVM()
                        {
                            Id = u.Id,
                            Ip = u.Ip,
                            SurveyEntity = e.Name,
                            CorrectAnswerd = u.CorrectAnswerd,
                            NoAnswerd = u.NoAnswerd,
                            WrongAnswerd = u.WrongAnswerd,
                            TotalScore = u.TotalScore,
                            TotalRank = u.TotalRank,
                            TotalCorrectScore = e.TotalScore,
                            maxScore = e.MaxScore,
                            minScore = e.MinScore,
                            zaribManfi = e.ZaribManfi,
                            Accepted = u.AcceptedDate != null ? true : false,
                            AzmounDate = u.CreatedDate,
                            AcceptedDate = u.AcceptedDate,
                            TimeDuration = e.TimeDuration,
                        }).FilterAndSortJqGrid<SurveyUserAnswerVM>(grid).ToPagedList<SurveyUserAnswerVM>(grid);
            list.ForEach(row =>
            {
                if (row.AcceptedDate != null)
                    row.AcceptedDatePersian = row.AcceptedDate.Value.GeogianToPersianStringDateOnly();
                if (row.AzmounDate.HasValue)
                    row.AzmounDatePersian = row.AzmounDate.Value.GeogianToPersianStringDateOnly();
            });
            return list;
        }

        public IEnumerable<SurveyUserAnswerVM> GetAllAzmoonAcceptedByUserId(int current_learn_userId)
        {
            List<SurveyUserAnswerVM> list;
            using (var ctx = new SWEntities())
                list = (from u in ctx.AzmoonUserAnswer.Where(x => x.UserId == current_learn_userId)
                        join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                        where u.AcceptedDate != null
                        select new SurveyUserAnswerVM()
                        {
                            Id = u.Id,
                            Ip = u.Ip,
                            SurveyEntity = e.Name,
                            CorrectAnswerd = u.CorrectAnswerd,
                            NoAnswerd = u.NoAnswerd,
                            WrongAnswerd = u.WrongAnswerd,
                            TotalScore = u.TotalScore,
                            TotalRank = u.TotalRank,
                            TotalCorrectScore = e.TotalScore,
                            maxScore = e.MaxScore,
                            minScore = e.MinScore,
                            zaribManfi = e.ZaribManfi,
                            Accepted = u.AcceptedDate != null ? true : false,
                            AzmounDate = u.CreatedDate,
                            AcceptedDate = u.AcceptedDate,
                            TimeDuration = e.CoursTimeDuration,
                            FromDate = e.FromDate,
                            ToDate = e.ToDate,

                        }).ToList();
            list.ForEach(row =>
            {
                if (row.AcceptedDate != null)
                    row.AcceptedDatePersian = row.AcceptedDate.Value.GeogianToPersianStringDateOnly();
                if (row.FromDate != null)
                    row.FromDateCourse = row.FromDate.Value.GeogianToPersianStringDateOnly();
                if (row.ToDate != null)
                    row.ToDateCourse = row.ToDate.Value.GeogianToPersianStringDateOnly();
                if (row.AzmounDate.HasValue)
                    row.AzmounDatePersian = row.AzmounDate.Value.GeogianToPersianStringDateOnly();
            });
            return list;
        }
        public SurveyUserAnswerVM GetAzmoonById(int AzmoonUserAnswerId)
        {
            SurveyUserAnswerVM item;
            using (var ctx = new SWEntities())
                item = (from u in ctx.AzmoonUserAnswer.Where(x => x.Id == AzmoonUserAnswerId)
                        join e in ctx.AzmoonEntity on u.AzmoonEntityId equals e.Id
                        select new SurveyUserAnswerVM()
                        {
                            Id = u.Id,
                            Ip = u.Ip,
                            SurveyEntity = e.Name,
                            CorrectAnswerd = u.CorrectAnswerd,
                            NoAnswerd = u.NoAnswerd,
                            WrongAnswerd = u.WrongAnswerd,
                            TotalScore = u.TotalScore,
                            TotalRank = u.TotalRank,
                            TotalCorrectScore = e.TotalScore,
                            maxScore = e.MaxScore,
                            minScore = e.MinScore,
                            zaribManfi = e.ZaribManfi,
                            Accepted = u.AcceptedDate != null ? true : false,
                            AzmounDate = u.CreatedDate,
                            AcceptedDate = u.AcceptedDate,
                            TimeDuration = e.CoursTimeDuration,
                            FromDate = e.FromDate,
                            ToDate = e.ToDate,

                        }).SingleOrDefault();

            if (item.AcceptedDate != null)
                item.AcceptedDatePersian = item.AcceptedDate.Value.GeogianToPersianStringDateOnly();
            if (item.FromDate != null)
                item.FromDateCourse = item.FromDate.Value.GeogianToPersianStringDateOnly();
            if (item.ToDate != null)
                item.ToDateCourse = item.ToDate.Value.GeogianToPersianStringDateOnly();
            if (item.AzmounDate.HasValue)
                item.AzmounDatePersian = item.AzmounDate.Value.GeogianToPersianStringDateOnly();

            return item;
        }

        public static byte[] UpscaleImageBytes(byte[] imageBytes, double scaleFactor)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("آرایه بایت ورودی خالی است.");

            using (var inputStream = new MemoryStream(imageBytes))
            {
                // استفاده از نام مستعار ISImage به جای Image برای عدم تداخل با System.Drawing.Image
                using (ISImage image = ISImage.Load(inputStream, out IImageFormat format))
                {
                    int newWidth = (int)(image.Width * scaleFactor);
                    int newHeight = (int)(image.Height * scaleFactor);

                    // استفاده از نام مستعار ISSize برای مشخص کردن ابعاد جدید
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new ISSize(newWidth, newHeight),
                        Mode = ResizeMode.Max,
                        Sampler = KnownResamplers.Lanczos3
                    }));

                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, format);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public  byte[] ResizeImageLegacy(byte[] imageBytes, int targetWidth, int targetHeight)
        {
            using (var msInput = new MemoryStream(imageBytes))
            {
                using (var originalImage = System.Drawing.Image.FromStream(msInput))
                {
                    using (var bitmap = new Bitmap(targetWidth, targetHeight))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            // تنظیم کیفیت رندرینگ برای خروجی بهتر
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;

                            graphics.DrawImage(originalImage, 0, 0, targetWidth, targetHeight);
                        }

                        using (var msOutput = new MemoryStream())
                        {
                            // حفظ فرمت اصلی تصویر (مثلاً JPEG)
                            bitmap.Save(msOutput, originalImage.RawFormat);
                            return msOutput.ToArray();
                        }
                    }
                }
            }
        }

     

        internal async Task<FileStreamResponse> GetCertificate(int userAnswerId, string cerPath, string fontPath)
        {
            MemoryStream result = new MemoryStream();
            try
            {


                //var userAnswer = await context.AzmoonUserAnswer.FirstOrDefaultAsync(a => a.Id == userAnswerId);

                //var answerEntity = await context.AzmoonEntity.FirstOrDefaultAsync(a => a.Id == userAnswer.AzmoonEntityId);
                var answerUser = await Get(userAnswerId);
                if (answerUser == null)
                    throw new Exception("کاربر شرکت کننده در آزمون معتبر نمی باشد .");
                var answerEntity = await AzmoonEntityBiz.Instance.Get(answerUser.AzmoonEntityId);
                if (answerEntity == null)
                    throw new Exception("آزمون معتبر نمی باشد .");
                string groupAzmoon = $"کتابخوانی";
                if (answerEntity.GroupAzmoonId == 2)
                    groupAzmoon = $"آموزش مجازی";
                else if (answerEntity.GroupAzmoonId == 3)
                    groupAzmoon = $" آموزشی غیر حضوری کتابخوانی";
                else
                    groupAzmoon = $"آموزشی";

                var user = UserBiz.Instance.FindByUserId(answerUser.UserId.Value);
                if (string.IsNullOrEmpty(answerEntity.AzmoonCerImageUrl))
                    throw new Exception("تصویری برای گواهینامه تعریف نشده است !");
                string cerPathFull = cerPath + answerEntity.AzmoonCerImageUrl;
                if (!File.Exists(cerPathFull))
                    throw new Exception("فایل تصویر گواهینامه یافت نشد !");
                var cerFile = File.ReadAllBytes(cerPathFull);

                if (cerFile.Length > 0)
                {
                    //cerFile = ResizeImageLegacy(cerFile, 1025, 682);
                    cerFile = ResizeImageLegacy(cerFile, 1025, 682);
                    //byte[] output = ImageTextPrinterBiz.DrawTextOnImage(
                    //    imageBytes: cerFile,
                    //    text: $"گواهی می شود           ابراهیم حیدری \nنام پدر محمد        کد ملی ۳۴۲۵۷۶۹۸۰۰۹\n" +
                    //    $"دوره آموزشی غیر حضوری کتابخوانی چگونه مثل یک میلیونر فکر کنیم؟\n" +
                    //    $"را در تاریخ  ۲۶آبان ۱۴۰۴ به مدت ۴ ساعت با موفقیت به پایان رسانده است." ,
                    //    fontPath: fontPath+"BYekan.ttf", // اگر ندارید null بفرستید
                    //    fontName: "BYekan",
                    //    initialFontSize: 21,
                    //    maxWidth: 890,
                    //    maxHeight: 600,
                    //    x: 50,
                    //    y: 230,
                    //    lineSpacingPx:8,
                    //    color: Color.Black
                    //);
                    byte[] output = ImageTextPrinterColorBiz.DrawTextOnImage(
   imageBytes: cerFile,
   text: $"گواهی می شود       {user.Name + " " + user.Family} \n" +
   $"نام پدر {user.FatherName}        کد ملی {user.NationaCode}\n" +
   $" دوره " + groupAzmoon + $"<color red>{answerEntity.Name}</color>" + "\n" +
   $"را در تاریخ  {answerUser.CreatedDate.ToPersianDate()} به مدت {answerEntity.CoursTimeDuration} ساعت با موفقیت به پایان رسانده است.",
   fontPath: fontPath + "BYekan.ttf", // اگر ندارید null بفرستید
   fontName: "BYekan",
   initialFontSize: 21,
   maxWidth: 890,
   maxHeight: 600,
   x: 50,
   y: 230,
   lineSpacingPx: 8,
   System.Drawing.Color.Black
             );
                    byte[] output2 = ImageTextPrinterBiz.DrawTextOnImage(
                        imageBytes: output,
                        text: $"شماره : {answerUser.Id + "-" + user.id}\n" + $"تاریخ : {DateTime.Now.ToPersianDate()}\n",
                        fontPath: fontPath + "BYekan.ttf", // اگر ندارید null بفرستید
                        fontName: "BYekan",
                        initialFontSize: 13,
                        maxWidth: 200,
                        maxHeight: 120,
                        x: 30,
                        y: 70,
                        lineSpacingPx: 3,
                        color: System.Drawing.Color.Black
                    );
                    //File.WriteAllBytes("output.jpg", output);
                    output2 = UpscaleImageBytes(output2, 2);
                    return new FileStreamResponse
                    {
                        Stream = new MemoryStream(output2),
                        Content = output2,
                        FileName = answerEntity.AzmoonCerImageUrl,
                        PathFull = cerPathFull,
                    };
                }
                cerFile = UpscaleImageBytes(cerFile, 2);
                return new FileStreamResponse
                {
                    Stream = new MemoryStream(cerFile),
                    Content = cerFile,
                    FileName = answerEntity.AzmoonCerImageUrl,
                    PathFull = cerPathFull,
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        internal async Task<bool> Accept(int userAnswerId, int currentUserId)
        {
            using (var ctx = new SWEntities())
            {
                var item = await ctx.AzmoonUserAnswer.SingleOrDefaultAsync(a => a.Id == userAnswerId);
                item.AcceptedDate = DateTime.Now;
                item.AcceptedBy = currentUserId;
                ctx.SaveChanges();
                return true;
            }
        }
        internal async Task<bool> Reject(int userAnswerId, int currentUserId)
        {
            using (var ctx = new SWEntities())
            {
                var item = await ctx.AzmoonUserAnswer.SingleOrDefaultAsync(a => a.Id == userAnswerId);
                item.AcceptedDate = null;
                item.AcceptedBy = null;
                ctx.SaveChanges();
                return true;
            }
        }
    }
}