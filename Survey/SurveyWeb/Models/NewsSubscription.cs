using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    [Description("خبرنامه")]
    public class NewsSubscription:BaseEntity
    {

        [Display(Name = "ایمیل")]
        [GenericRequired, GenericStringLength(50), DataType(DataType.EmailAddress, ErrorMessage = "لطفا ایمیل معتبر وارد کنید")]
        public string Email { get; set; }
    }
}