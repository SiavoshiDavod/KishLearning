using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public class Advertising : BaseEntity
    {
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]

        public Resturant Resturant { get; set; }

        [DisplayName(" توضیحات")]
        [GenericStringLength(4000)]
        [AllowHtml]
        public string Description { get; set; }

        [DisplayName(" رزرو")]
        [GenericStringLength(200)]
        public string LinkReserve { get; set; }

        [GenericStringLength(100)]
        [DisplayName("عکس بنر اصلی")]
        public string ImageUrl { get; set; }
        [DisplayName("مورد تایید است؟")]
        public bool Archive { get; set; }

        [NotMapped]
        public string ResturantName => Resturant?.Name;

        public ICollection<AdvertisingAttachement> AdvertisingAttachements { get; set; }

    }

    public class AdvertisingAttachement : BaseEntity
    {
        [GenericStringLength(100)]
        [DisplayName("مسیر فایل")]
        public string ImageUrl { get; set; }
        public bool IsVideo { get; set; }
        public int AdvertisingId { get; set; }
        [ForeignKey("AdvertisingId")]
        [JsonIgnore]
        public Advertising Advertising { get; set; }
    }
   
  }