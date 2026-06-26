using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SenakLearn.Models
{
    [Table("ObjCount", Schema = "dbo")]
    public class ObjCount : BaseEntity
    {
        [Display(Name = "عنوان شمارش")]
        public string ObjName { get; set; }
        [Display(Name = "نوع شمارش"), GenericRequired]
        public string ObjType { get; set; }
        [Display(Name = "شناسه شمارش"), GenericRequired]
        public string ObjId { get; set; }
        [NotMapped]
        public string ObjTitle { get; set; }
        [NotMapped]
        public string ObjDescript { get; set; }
        [Display(Name = "تعداد"), GenericRequired]
        public int Count { get; set; }

        /// <summary>
        /// دوره آموزشی
        /// </summary>
        public static string objType_Course= "Course";
        /// <summary>
        /// ویدیو آموزشی
        /// </summary>
        public static string objType_Video= "Video";
        /// <summary>
        /// کتاب
        /// </summary>
        public static string objType_Book = "Book";
        /// <summary>
        /// دانلود کتاب
        /// </summary>
        public static string objType_BookDownload = "BookDownload";
        /// <summary>
        /// مقاله
        /// </summary>
        public static string objType_Paper = "Paper";
        /// <summary>
        /// دانلود مقاله
        /// </summary>
        public static string objType_PaperDownload = "PaperDownload";
        /// <summary>
        /// دانلود پادکست
        /// </summary>
        public static string objType_Podcast = "Podcast";
    }
}