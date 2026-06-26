using Newtonsoft.Json;
using SurveyWeb.Models.Security;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("کاربران")]
    public class User : BaseEntity
    {
        [Display(Name = "نقش")]
        public Roles RoleId { get; set; }
        [Display(Name = "نام")]
        [GenericRequired, GenericStringLength(20)]
        public string Name { get; set; }
        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        [Display(Name = "ایمیل")]
        [GenericRequired, GenericStringLength(50), DataType(DataType.EmailAddress,ErrorMessage ="لطفا ایمیل معتبر وارد کنید")]
        public string UserName { get; set; }
        [Display(Name = "رمزعبور")]
        [GenericRequired, GenericStringLength(20)]
        [JsonIgnore]
        public string Pass { get; set; }
        [Display(Name = "تلفن همراه")]
        [/*GenericRequired,*/ GenericStringLength(11)]
        public string Mobile { get; set; }
        [Display(Name = "سن")]
        public byte OldYear { get; set; }
        [Display(Name = "محل سکونت")]
        public Province Province { get; set; }

        [GenericStringLength(30)]
        [Display(Name = "مدرک تحصیلی")]
        public string Education { get; set; }

        [GenericStringLength(30)]
        [Display(Name = "شغل")]
        public string Job { get; set; }
        [Display(Name = "وضعیت تاهل")]
        public bool IsMarried { get; set; }

        [Display(Name = " عکس کاربر")]
        [GenericStringLength(100)]
        public string UserImageUrl { get; set; }

        [NotMapped]
        [Display(Name = "غیرفعال شدن کاربر ")]
        public bool Archive { get; set; }

        [NotMapped]
        public string ProvinceName { get { return this.Province.ToString(); } set { } }
        [NotMapped]
        public string RoleName { get { return this.RoleId.ToString(); } set { } }
        [NotMapped]
        public List<Permisstion> Permisstions { get; set; }
    }
    public enum Roles
    {
        User = 0,
        Admin = 1,
       SuperAdmin = 2
    }
}