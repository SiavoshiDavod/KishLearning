using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SenakLearn.Models.Security;

namespace SenakLearn.Models
{
    public enum Roles
    {
        User = 2,
        Teacher = 4,
        Admin = 1,
        SuperAdmin = 3
    }
    [DisplayName("کاربری")]
    //[DisplayPluralName("کاربران")]
    public class learn_user
    {

        [Key]
        public int id { get; set; }

        [GenericRequired]
        [Display(Name = "نام کاربری")]
        [GenericMaxLength(20)]
        public string user_name { get; set; }
        [GenericRequired]
        [Display(Name = "کلمه عبور")]
        [JsonIgnore]
        public string password { get; set; }
        [Display(Name = "تاریخ ثبت")]
        [DisplayFormat(DataFormatString = "{0: yyyy/MM/dd}")]
        public DateTime date_register { get; set; } = DateTime.Now;
        [Display(Name = "وضعیت")]
        public bool status { get; set; } = true;
        //[Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "نقش کاربر")]
        public Roles RoleId { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "ایمیل معتبر نمی باشد")]
        [Display(Name = "ایمیل"), DataType(DataType.EmailAddress), GenericRequired]
        public string Email { get; set; }
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "همراه معتبر نمی باشد")]
        [Display(Name = "همراه")]
        public string Mobile { get; set; }
        [Display(Name = "نام")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string Family { get; set; }
        [Display(Name = "آدرس"), GenericStringLength(500)]
        public string Address { get; set; }
        [Display(Name = "عکس"), GenericStringLength(200)]
        public string ImageUrl { get; set; }
        [Display(Name = "کد ملی"), GenericStringLength(10)]
        [GenericMinLength(10)]
        ////[RegularExpression(@"\d",ErrorMessage = "کد ملی را صحیح وارد کنید")]
        public string NationaCode { get; set; }
        [JsonIgnore]
        public string PassAdobe { get; set; } = "123456";

        [NotMapped]
        public string BREEZESESSION { get; set; }
        [NotMapped]
        public string NameForEmail => string.IsNullOrWhiteSpace(Name + " " + Family) ? user_name : Name + " " + Family;

        [Display(Name = "استان")]
        public Province? Province { get; set; }

        [Display(Name = "شهر"), GenericStringLength(50)]
        public string City { get; set; }

        [Display(Name = "مدرک تحصیلی"), GenericStringLength(50)]
        public string Education { get; set; }

        [Display(Name = "تخصص"), GenericStringLength(50)]
        public string Expertise { get; set; }

        [Display(Name = "نام پدر"), GenericStringLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "محل تولد"), GenericStringLength(50)]
        public string BirthLocation { get; set; }

        [Display(Name = "شماره ثابت"), GenericStringLength(15)]
        public string Tel { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDay { get; set; }

        [NotMapped]
        public virtual string BirthDayShamsi
        {
            get => BirthDay == null ? "" : BirthDay.Value.ToPersianDate();
            set => BirthDay = value.ToGregorianDate();
        }

        [NotMapped]
        public virtual string date_register_Shamsi
        {
            get => date_register.ToPersianDate();
            set => date_register = value.ToGregorianDate();
        }
        [Display(Name = "شماره شناسنامه"), GenericStringLength(15)]
        public string Shenasname { get; set; }

        [NotMapped]
        public string ProvinceName { get { return this.Province.ToString(); } set { } }
        [NotMapped]
        public string RoleName { get { return this.RoleId.ToString(); } set { } }
        [NotMapped]
        public List<Permisstion> Permisstions { get; set; }
        public int? TypeUser { get; set; }
        [NotMapped]
        [Display(Name = "نوع کاربر"), GenericStringLength(15)]
        public string TypeUserName { get; set; }

        public int? PostId { get; set; }
        [NotMapped]
        [Display(Name = "پست"), GenericStringLength(50)]
        public string PostName { get; set; }
        public int? OrgId { get; set; }
        [NotMapped]
        [Display(Name = "سازمان"), GenericStringLength(50)]
        public string OrgName { get; set; }
        [Display(Name = "کد پرسنلی"), GenericStringLength(50)]
        public string PersonCode { get; set; }
        [NotMapped]
        public int? CourseDurationSum { get; set; }
        [NotMapped]
        public int? CourseDurationYear { get; set; }
        public virtual void Validate()
        {
            List<ValidationResult> resultList = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), resultList, true);
            if (resultList.Count > 0)
            {//throw new BusinessException(resultList.First().ErrorMessage);
                var current1 = "";
                current1 = resultList.Aggregate(current1,
                                    (current, ve) => current + (ve.ErrorMessage));
                throw new Exception(current1);
            }
        }
        public virtual List<ValidationResult> Validate(bool flag)
        {
            List<ValidationResult> resultList = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), resultList, true);
            if (resultList.Count > 0)
            {//throw new BusinessException(resultList.First().ErrorMessage);
                return resultList;
            }
            else
                return new List<ValidationResult>();
        }
    }
    public enum Province
    {
        [Description("تهران")]
        Tehran = 21,
        [Description("البرز")]
        Alborz = 26,
        [Description("قم")]
        Qum = 25,
        [Description("مرکزی")]
        Markazi = 86,
        [Description("زنجان")]
        Zanjan = 24,
        [Description("سمنان")]
        Semnam = 23,
        [Description("همدان")]
        Hamadan = 81,
        [Description("قزوین")]
        Qazvin = 28,
        [Description("اصفهان")]
        Isfahan = 31,
        [Description("آذربایجان غربی")]
        AzerbaijanGharbi = 44,
        [Description("مازندران")]
        Mazandaran = 11,
        [Description("کهگیلویه و بویراحمد")]
        KohgiluyehBoyerAhmad = 74,
        [Description("کرمانشاه")]
        Kermanshah = 83,
        [Description("خراسان رضوی")]
        KhorasanRazavi = 51,
        [Description("اردبیل")]
        Ardabil = 45,
        [Description("گلستان")]
        Golestan = 17,
        [Description("آذربایجان شرقی")]
        AzerbaijanSharghi = 41,
        [Description("سیستان و بلوچستان")]
        SistanBaluchestan = 54,
        [Description("کردستان")]
        Kordestan = 87,
        [Description("فارس")]
        Fars = 71,
        [Description("لرستان")]
        Lorestan = 66,
        [Description("کرمان")]
        Kerman = 34,
        [Description("خراسان جنوبی")]
        KhorasanJonobi = 56,
        [Description("گیلان")]
        Gilan = 13,
        [Description("بوشهر")]
        Bousher = 77,
        [Description("هرمزگان")]
        Hormozgan = 76,
        [Description("خوزستان")]
        Khozestan = 61,
        [Description("چهار محال و بختیاری")]
        ChaharMahaalBakhtiari = 38,
        [Description("خراسان شمالی")]
        KhorasanShomali = 58,
        [Description("یزد")]
        Yazd = 35,
        [Description("ایلام")]
        Ilam = 84,
    }
}
