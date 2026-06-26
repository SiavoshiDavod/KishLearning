using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class VideoFile 
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid VideoId { get; set; }
        [GenericRequired]
        [Display(Name = "عنوان فایل")]
        [GenericMaxLength(50)]
        public string titel { get; set; }
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string doc { get; set; }
        [Display(Name = "فایل")]
        public string myFile { get; set; }
        [Display(Name = "فرمت فایل")]
        public string format { get; set; }
        [Display(Name = "کاربر ثبت کننده")]
        public int createBy { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public DateTime createDate { get; set; }

        public bool WaitingForAccept { get; set; }

        [NotMapped]
        [Display(Name = "تاریخ ثبت")]
        public  string CreatedDateShamsi
        {
            get => createDate.ToPersianDate();
            set => createDate = value.ToGregorianDate();
        }
        [NotMapped]
        public string act { get; set; }// !WaitingForAccept ? "" : "<a class='ajaxLink' href='#' onclick='return Mypost(\"/videoFile/Accept?id=" + VideoId + "\",null,null, reloadJqGrid(\"JoinUsGrid\"))' title='تایید'><div class=''>تایید</div></a>";
    }
}