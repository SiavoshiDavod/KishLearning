using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.Resturan
{
    public class Shekayat : BaseEntity
    {
        [GenericStringLength(200), GenericRequired, Display(Name = "موضوع")]
        public string Title { get; set; }

        [GenericStringLength(25), GenericRequired, Display(Name = "نام و نام خانوادگي")]
        public string Name { get; set; }
        [GenericStringLength(50), GenericRequired, Display(Name = "پست الکترونیک"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [GenericStringLength(12), GenericRequired, Display(Name = "تلفن همراه")]
        public string Mobile { get; set; }

        [Display(Name = "متن درخواست")]
        [GenericStringLength(3000), DataType(DataType.MultilineText)]
        public string Description { get; set; }

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
      
        public TypeShekayat TypeShekayatId { get; set; }
        [Display(Name = "نوع درخواست")]
        public string TypeShekayatName => EnumExtention.GetDescription(TypeShekayatId);

        [Display(Name = "مرکز")]
        public int? ResturantId { get; set; }

        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }
    }

    public enum TypeShekayat : byte
    {
        [Description("شکایات")]
        Shek,
        [Description("انتقاد")]
        Enteghad,
        [Description("پیشنهاد")]
        Pishnahad,
        [Description("تقدیر")]
        Taghdir
    }
}