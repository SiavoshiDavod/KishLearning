using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class JoinUs:BaseEntity
    {
        [GenericRequired, GenericStringLength(50)]
        [Display(Name ="نام")]
        public string Name { get; set; }
        [GenericRequired, GenericStringLength(50)]
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        [GenericRequired,NationalCode,GenericStringLength(10)]
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; }
        [Display(Name = "ایمیل")]
        [GenericRequired, GenericStringLength(50)]
        public string Email { get; set; }
        [Display(Name = "موبایل")]
        [GenericRequired, GenericStringLength(12)]
        public string Mobile { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [GenericStringLength(100)]
        [Display(Name = "فایل رزومه")]
        public string ResumeFile { get; set; }
        [GenericRequired, GenericStringLength(100)]
        [Display(Name = "زمینه های همکاری")]
        public string GroupIds { get; set; }

        [Display(Name = "تایید شده؟")]
        public bool IsAccept { get; set; }

        [Display(Name = "قرارداد؟")]
        public bool IsAcceptContract { get; set; }

        [Display(Name = "آپلود ویدیو؟")]
        public bool IsUploadVideo { get; set; }

        [Display(Name = "تاریخ تایید")]
        public System.DateTime? AcceptedDate { get; set; }

        public int? UserId { get; set; }
        public int? TeacherId { get; set; }

        [NotMapped]
        [Display(Name = "زمینه های همکاری")]
        public int[] GroupIdArray { get; set; }
        [Display(Name = "زمینه های همکاری")]
        [NotMapped]
        public string GroupNames { get; set; }
        [NotMapped]
        public override string act =>("<a class='ajaxLink' target-update='TadGoodsDetailsDiv' href='/JoinUs/Edit?id=" + Id + "' title='ویرایش'><div class='tableEditLink'>ویرایش</div></a>") + (UserId == null ? "" : "<a href='/UsersAdmin/edit?Id=" + UserId + "'><div>اطلاعات کاربری</div></a>") + (TeacherId == null ? "" : "<a href='/Teacher/edit?Id=" + TeacherId + "'><div>اطلاعات تکمیلی</div></a>") + (UserId == null || TeacherId==null ? "" : "<a href='/VideoFile/index?userId=" + UserId + "'><div>ویدیوها</div></a>") + (string.IsNullOrEmpty(ResumeFile)?"": "<a href='/images/joinUs/" + ResumeFile + "'><div>دانلود</div></a>") +(IsAccept?"": "<a class='ajaxLink' href='#' onclick='return Mypost(\"/JoinUs/Accept?id=" + Id+ "\",null,null, reloadJqGrid(\"JoinUsGrid\"))' title='تایید'><div class=''>تایید</div></a>");
    }
}