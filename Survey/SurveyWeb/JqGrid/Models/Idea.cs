using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("ایده ها")]
    public class Idea : UserBaseInformation
    {

        [Display(Name = "ضرورت/مشکل")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Problem { get; set; }

        [Display(Name = "پیشنهاد/راهکار")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Proposal { get; set; }

        [Display(Name = "توضیحات")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "هزینه های احتمالی پیشنهاد")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Cost { get; set; }

        [Display(Name = "سود احتمالی پیشنهاد")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Benefit { get; set; }

        [Display(Name = "نمونه تجربیات")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Experience { get; set; }

        //مستندات(آپلود فایل): حداقل ۵ سند شامل عکس، فایل پی دی اف، فایل ورد و یا اکسل...بتواند آپلود کند.
        [Display(Name = "پیوست1")]
        [GenericStringLength(100)]
        public string Attachment1 { get; set; }

        [Display(Name = "پیوست2")]
        [GenericStringLength(100)]
        public string Attachment2 { get; set; }

        [Display(Name = "پیوست3")]
        [GenericStringLength(100)]
        public string Attachment3 { get; set; }

        [Display(Name = "پیوست4")]
        [GenericStringLength(100)]
        public string Attachment4 { get; set; }

        [Display(Name = "پیوست5")]
        [GenericStringLength(100)]
        public string Attachment5 { get; set; }

       

    }
}