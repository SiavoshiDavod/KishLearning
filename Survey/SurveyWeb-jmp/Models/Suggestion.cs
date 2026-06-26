using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("پیشنهادات")]
    public class Suggestion : UserBaseInformation
    {
       
        [Display(Name = "عنوان ایده")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Title { get; set; }

        [Display(Name = "کلیات طرح پیشنهادی")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Proposal { get; set; }

        [Display(Name = "شرح مختصری از محصول/طرح")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [GenericRequired, Display(Name = "مدت ساخت و راه اندازی (ماه)")]
        public int Month { get; set; }

        [GenericRequired, Display(Name = "پیش بینی مدت بهره برداری (سال)")]
        public int Year { get; set; }

        [Display(Name = "مزایای رقابتی و اجرایی طرح برای منطقه")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Benefit { get; set; }

        [Display(Name = "سوابق فعالیتی مرتبط با موضوع طرح (مدارک مربوطه در مرحله بارگذاری اسناد پیوست گردد)")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Experience { get; set; }

        [Display(Name = "پیوست طرح پیشنهادی")]
        [GenericStringLength(100)]
        public string Attachment1 { get; set; }

        [Display(Name = " پیوست سوابق مربوط به صاحب طرح")]
        [GenericStringLength(100)]
        public string Attachment2 { get; set; }

        //[Display(Name = "پیوست3")]
        //[GenericStringLength(100)]
        //public string Attachment3 { get; set; }

        //[Display(Name = "پیوست4")]
        //[GenericStringLength(100)]
        //public string Attachment4 { get; set; }

        //[Display(Name = "پیوست5")]
        //[GenericStringLength(100)]
        //public string Attachment5 { get; set; }

    }
}