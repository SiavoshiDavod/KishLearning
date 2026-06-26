using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("تماس با ما")]
    public class ContactUs: BaseEntity
    {
        [GenericRequired, GenericStringLength(20)]
        [Display(Name = "نام ", Description = "نام شناسنامه خود را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "ایمیل", Description = "لطفا ایمیل خود را وارد کنید")]
        [GenericRequired, GenericStringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "شماره تماس")]
        [GenericStringLength(20)]
        public string Tel { get; set; }

        [Display(Name = "آی پی")]
        [GenericStringLength(20)]
        public string Ip { get; set; }

        [Display(Name = "عنوان ")]
        public string Title { get; set; }

        [Display(Name = "شرح ")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "وضعیت کارتابل")]
        [ForeignKey("CartableId")]
        public Cartable Cartable { get; set; }
        [Display(Name = "وضعیت کارتابل")]
        public int CartableId { get; set; }
    }
}