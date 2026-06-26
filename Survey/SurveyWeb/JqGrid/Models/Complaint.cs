using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    [Description("شکایات")]
    public class Complaint : UserBaseInformation
    {
        //public string NationalCode { get; set; }
        //public string FatherName { get; set; }
        //public string PostalCode { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime Birthday { get; set; }

        [Display(Name = "عنوان شکایت")]
        [GenericStringLength(200)]
        public string Title { get; set; }

        [Display(Name = "شرح شکایت")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "مدرک ضمیمه")]
        [GenericStringLength(100)]
        public string Attachment { get; set; }
    }
}