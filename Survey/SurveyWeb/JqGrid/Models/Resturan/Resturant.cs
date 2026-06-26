using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    public class Resturant: BaseEntity
    {
        public Resturant()
        {
            ResturantPersonel = new HashSet<ResturantPersonel>();
            ResturantCheckList = new HashSet<ResturantCheckList>();
            Advertising = new HashSet<Advertising>();
            ResturantMenu = new HashSet<ResturantMenu>();
        }
        public virtual ICollection<ResturantCheckList> ResturantCheckList { get; set; }
        public virtual ICollection<ResturantPersonel> ResturantPersonel { get; set; }
        [JsonIgnore]
        public virtual ICollection<Advertising> Advertising { get; set; }
        [JsonIgnore]
        public virtual ICollection<ResturantMenu> ResturantMenu { get; set; }

        [Display(Name = "محبوب است؟")]
        public bool IsFavorite { get; set; }

        [Display(Name = "موزیکال است؟")]
        public bool IsMusical { get; set; }

        [Display(Name = "کد مرکز")]
        [GenericStringLength(10)]
        public string Code { get; set; }

        [GenericStringLength(100)]
        [Display(Name = "نام مرکز پذیرایی")]
        [GenericRequired]
        public string Name { get; set; }

        [Display(Name = "سال ساخت")]
        public int BuildYear { get; set; }

        [Display(Name = " درجه رستوران")]
        public byte Degree { get; set; }

        [Display(Name = " متراژ زمین")]
        public int MeterGround { get; set; }

        [Display(Name = " متراژ آشپزخانه")]
        public int MeterKitchen { get; set; }

        [Display(Name = " متراژ سالن")]
        public int MeterSaloon { get; set; }

        [Display(Name = "شماره تماس")]
        [GenericStringLength(20)]
        public string Tel { get; set; }

        [Display(Name = "آدرس")]
        [GenericStringLength(500), DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [NotMapped]
        public string LastDateExtendedLicenseShamsi
        {
            get { return LastDateExtendedLicense?.ToPersianDate(); }
            set { LastDateExtendedLicense = value.ToGregorianDate(); }
        }

        [Display(Name = "آخرین تاریخ تمدید مجوز")]
        public DateTime? LastDateExtendedLicense { get; set; }

        [Display(Name = "ایمیل")]
        [ GenericStringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "وب سایت")]
        [GenericStringLength(50), DataType(DataType.Url, ErrorMessage = "لطفا وب سایت معتبر وارد کنید")]
        public string WebSite { get; set; }

        ///////////////////////////////////////////////////////////

        [GenericRequired]
        [Display(Name = "نام بهره بردار")]
        [GenericStringLength(100)]
        public string Beneficiary { get; set; }

        [GenericStringLength(100)]
        [Display(Name =" عکس")]
        public string BeneficiaryImageUrl { get; set; }

        [Display(Name = "نام پدر ")]
        [GenericStringLength(100)]
        public string BeneficiaryFatherName { get; set; }

        [NotMapped]
        public string BeneficiaryBirthdayShamsi
        {
            get { return BeneficiaryBirthday?.ToPersianDate(); }
            set { BeneficiaryBirthday = value.ToGregorianDate(); }
        }

        [Display(Name = " تاریخ تولد")]
        public DateTime? BeneficiaryBirthday { get; set; }

        [Display(Name = "شماره شناسنامه ")]
        public int BeneficiaryCodeNumber { get; set; }

        [GenericStringLength(10)]
        [Display(Name = "شماره ملی ")]
        public string BeneficiaryNationalCode { get; set; }

        [Display(Name = "میزان تحصیلات")]
        [GenericStringLength(100)]
        public string BeneficiaryEducation { get; set; }

        [Display(Name = "سابقه قبلی")]
        [GenericStringLength(100)]
        public string BeneficiaryLastHistory { get; set; }

        [Display(Name = "تلفن همراه و ثابت")]
        [GenericRequired]
        [GenericStringLength(100)]
        public string BeneficiaryTel { get; set; }

        ///////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////

        [Display(Name = "نام مدیر")]
        [GenericStringLength(100)]
        [GenericRequired]
        public string Manager { get; set; }

        [GenericStringLength(100)]
        [Display(Name = " عکس")]
        public string ManagerImageUrl { get; set; }

        [Display(Name = "میزان تحصیلات")]
        [GenericStringLength(100)]
        public string ManagerEducation { get; set; }

        [NotMapped]
        public string ManagerBirthdayShamsi
        {
            get { return ManagerBirthday?.ToPersianDate(); }
            set { ManagerBirthday = value.ToGregorianDate(); }
        }

        [Display(Name = " تاریخ تولد")]
        public DateTime? ManagerBirthday { get; set; }

        [Display(Name = "سابقه کار در رستوران")]
        [GenericStringLength(100)]
        public string ManagerLastHistory { get; set; }


        [Display(Name = " دوره های آموزشی ")]
        public string ManagerLearningCourse { get; set; }

        [GenericRequired]
        [Display(Name = "تلفن همراه ")]
        [GenericStringLength(100)]
        public string ManagerTel { get; set; }

        [GenericStringLength(50)]
        [Display(Name = " معرف ")]
        public string ManagerReagent { get; set; }


        [Display(Name = "توضیحات")]
        [GenericStringLength(500), DataType(DataType.MultilineText)]
        public string ManagerDesc { get; set; }


        ///////////////////////////مشخصات نیروی انسانی////////////////////////////////

        [Display(Name = "تعداد کل پرسنل ")]
        public byte PersonelCountAll { get; set; }
        [Display(Name = "تعداد پرسنل آوزش دیده")]
        public byte PersonelCountLearned { get; set; }
        [Display(Name = "تعداد پرسنل مسلط به زبان انگلیسی")]
        public byte PersonelCountEnglishTalking { get; set; }
        [Display(Name = "تعداد پرسنل با تجربه بیش از 2 سال")]
        public byte PersonelCountTwoYear { get; set; }
        [Display(Name = "تعداد پرسنل دارای پرونده و تاییدشده از اماکن")]
        public byte PersonelCountAccepted { get; set; }
        ///////////////////////////مشخصات  منو////////////////////////////////

        [Display(Name = "منوی دوزبانه")]
        public bool MenuTwoLanguage { get; set; }

        [Display(Name = "منوی کودک")]
        public bool MenuBaby { get; set; }

        [Display(Name = "منوی غذای گیاهی رژیمی")]
        public bool MenuRejim { get; set; }

        [Display(Name = "صندلی کودک")]
        public bool babyseat { get; set; }

        [Display(Name = "خدمات صبحانه")]
        public bool HasBreakfast { get; set; }

        [Display(Name = "خدمات نهار")]
        public bool HasLunch { get; set; }

        [Display(Name = "خدمات شام")]
        public bool HasDinner { get; set; }

        [Display(Name = "خدمات سرویس بهداشتی")]
        public bool HasWC { get; set; }

        [Display(Name = "ظرفیت به تعداد صندلی")]
        public int CapacitySeatCount { get; set; }

        [Display(Name = "نوع خدمات و امکانات(موزیک زنده،بوفه،آلاکارد و ...)")]
        [GenericStringLength(200), DataType(DataType.MultilineText)]
        public string ResturantServiceType { get; set; }
        ///////////////////////////مشخصات  آشپزخانه////////////////////////////////

        [Display(Name = " انبار موادغذایی")]
        public bool HasFoodStorage { get; set; }

        [Display(Name = "سردخانه زیرصفر")]
        public bool HasFreezerUnderZero { get; set; }

        [Display(Name = "سردخانه بالای صفر")]
        public bool HasFreezerMoreThanZero { get; set; }

        [Display(Name = "اتاق جداگانه سرآشپز")]
        public bool HasMasterChefRoom { get; set; }

        [Display(Name = "قصابی جداگانه")]
        public bool HasButcher { get; set; }

        [Display(Name = "ماشین ظرف شویی مکانیکی")]
        public bool HasMechanicalDishwasher { get; set; }

        ///////////////////////////////////////////////////////////
        [Display(Name = "نوع خدمات")]
        public int ResturantTypeId { get; set; }

        [ForeignKey("ResturantTypeId")]
        public ResturantType ResturantType { get; set; }

        [Display(Name = "مدیر سالن")]
        [GenericStringLength(100)]
        public string SalonManager { get; set; }

        [Display(Name = "سرآشپز")]
        [GenericStringLength(100)]
        public string MasterChef { get; set; }

        [Display(Name = "مالک")]
        [GenericStringLength(100)]
        public string Owner { get; set; }

        [Display(Name = "شماره ثبت")]
        [GenericStringLength(100)]
        public string RegistrationNumber { get; set; }


        [Display(Name = "تاریخ قرارداد")]
        public DateTime? ContractDate { get; set; }

        [NotMapped]
        public string ContractDateShamsi
        {
            get { return ContractDate?.ToPersianDate(); }
            set { ContractDate = value.ToGregorianDate(); }
        }

        [NotMapped]
        public string ContractExpireDateShamsi
        {
            get { return ContractExpireDate?.ToPersianDate(); }
            set { ContractExpireDate = value.ToGregorianDate(); }
        }

        [Display(Name = "تاریخ انقضا قرارداد")]
        public DateTime? ContractExpireDate { get; set; }

        [Display(Name = "نوع قرارداد")]
        [GenericStringLength(100)]
        public string ContractType { get; set; }

        [Display(Name = "فعالیت اقتصادی")]
        [GenericStringLength(100)]
        public string EconomicActivity { get; set; }

        [Display(Name = " بهره برداری")]
        [GenericStringLength(100)]
        public string Use { get; set; }

        
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

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User user { get; set; }

    }

}