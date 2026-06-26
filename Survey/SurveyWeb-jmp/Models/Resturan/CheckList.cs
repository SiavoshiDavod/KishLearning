using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    public class CheckListType
    {
        [Key]
        public int Id { get; set; }
        [GenericStringLength(100)]
        [GenericRequired]
        public string DropDownTitle { get; set; }
    }
    //public enum CheckList : byte
    //{
    //    [Description("معرفی نامه از جامعه مراکز پذیرایی")]
    //    Moarefiname,
    //    [Description("تاییدیه اداره ایمنی و آتش نشانی کیش")]
    //    Atashneshani,
    //    [Description("تاییدیه بهداشت مکان از مرکز توسعه سلامت کیش")]
    //    Behdasht,
    //    [Description("تاییدیه اداره اماکن")]
    //    Amaken,
    //    [Description("تاییدیه صلاحیت فردی بهره بردار  و پرسنل")]
    //    salahiat,
    //    [Description("عکس ، کپی شناسنامه و کارت ملی مدیر و بهره بردار")]
    //    salahiat,
    //    [Description("مدارک بهره بردار جهت صدور مجوز ")]
    //    Bahrebardar,
    //    [Description("کپی برابر اصل سند ملک/اجاره نامه")]
    //    Sanad_Ejarenameh,
    //    [Description("بیمه نامه های : آتش سوزی،مسئولیت کارفرما در قبال کارکنان ، مراجعین و استفاده کنندگان")]
    //    Bimenameh,
    //    [Description("تاییدیه منوی غذا  به نرخ نامه غذایی")]
    //    Nerkhnameh,
    //    [Description("تاییدیه منوی غذا  با رسپی (فارسی و لاتین )")]
    //    Nerkhnameh,
    //    [Description("تصویر آخرین مجوز فعالیت اقتصادی('در صورت تمدید مجوز ')")]
    //    Mojavez,
    //}
}