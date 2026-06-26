using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    public abstract class UserBaseInformation: BaseEntity
    {

        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام ", Description = "نام شناسنامه خود را وارد کنید")]
        public string Name { get; set; }

        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام خانوادگی ")]
        public string Family { get; set; }

        [GenericStringLength(100)]
        [Display(Name = "مدرک و رشته تحصیلی")]
        public string Education { get; set; }

        [Display(Name = "ایمیل", Description = "لطفا ایمیل خود را وارد کنید")]
        [GenericRequired, GenericStringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "تلفن همراه")]
        [GenericStringLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "شماره تماس")]
        [GenericStringLength(20)]
        public string Tel { get; set; }

        [Display(Name = "واحد سازمانی/شغل")]
        [GenericStringLength(100)]
        public string UnitOrJob { get; set; }

        [Display(Name = "آدرس")]
        [GenericStringLength(500), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "کدرهگیری"), NotMapped]
        public long TrackingCode => Id + 10000;

        [Display(Name = "وضعیت کارتابل")]
        [ForeignKey("CartableId")]
        public Cartable Cartable { get; set; }
        [Display(Name = "وضعیت کارتابل")]
        public int CartableId { get; set; }

        [Display(Name = "آی پی")]
        [GenericStringLength(20)]
        public string Ip { get; set; }
    }
}