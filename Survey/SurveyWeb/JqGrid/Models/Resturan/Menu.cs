using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    public class ResturantMenu : BaseEntity
    {
        public int ResturantId { get; set; }
        [ForeignKey("ResturantId")]
        public Resturant Resturant { get; set; }
        [DisplayName(" نمایش در تبلیغات")]
        public bool Active { get; set; }
        [DisplayName("تایید جامعه")]
        public bool Accepted { get; set; }
        [DisplayName("نام")]
        //[GenericStringLength(500)]
        public string Description { get; set; }
        [DisplayName(" توضیحات جامعه")]
        //[GenericStringLength(500)]
        public string AdminDescription { get; set; }
        public virtual ICollection<ResturantDetailMenu> ResturantDetailMenus { get; set; }
        [NotMapped]
        public string ResturantName => Resturant?.Name;
        [NotMapped]
        public int Code => Id + 1000;
    }
    public class ResturantDetailMenu : BaseEntity
    {
        [DisplayName("نام غذا")]
        [GenericStringLength(100)]
        public string Name { get; set; }
        [DisplayName(" توضیحات")]
        [GenericStringLength(500)]
        public string Description { get; set; }
        [DisplayName("قیمت قدیم(ریال)")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public int? OldPrice { get; set; }
        [DisplayName("قیمت جدید(ریال)")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public int NewPrice { get; set; }
        [DisplayName("قیمت نهایی(ریال)")]
        [DisplayFormat(DataFormatString = "{0:n0}", ApplyFormatInEditMode = true)]
        public int FinalPrice { get; set; }
        [DisplayName("دسته بندی")]
        public AdvertisingMenuType AdvertisingMenuTypeId { get; set; }

        public int ResturantMenuId { get; set; }
        [ForeignKey("ResturantMenuId")]
        [JsonIgnore]
        public ResturantMenu ResturantMenu { get; set; }

        [NotMapped]
        public string AdvertisingMenuTypeName => EnumExtention.GetDescription(AdvertisingMenuTypeId);
    }
    public enum AdvertisingMenuType : byte
    {
        [Description("غذای ایرانی")]
        Irani,
        [Description("غذای فرنگی")]
        Farangi,
        [Description("غذای سنتی")]
        Sonati,
        [Description("غذای خورشتی")]
        khoreshti,
        [Description("غذای دریایی")]
        Daryaee,
        [Description("غذای خانگی")]
        Khanegi,

        [Description("پیش غذا")]
        PishGhaza,
        [Description("سالاد")]
        Salad,
        [Description("مخلفات")]
        Other,

        [Description("نوشیدنی")]
        Drink,
        [Description("نوشیدنی گرم")]
        DrinkHot,
        [Description("نوشیدنی سرد")]
        DrinkCold,

        [Description("قهوه")]
        ghahve,
        [Description("دمنوش")]
        Damnosh,
        [Description("شیک")]
        Shake,
        [Description("گلاسه")]
        Gelase,
        [Description("بستنی")]
        Bastani,
        [Description("کیک")]
        Cake,

        [Description("مخصوص")]
        Special,
        [Description("مخصوص سرآشپز")]
        Sarahpaz,

        [Description("ساندویچ")]
        Sandwich,
        [Description("پیتزا")]
        Pizza,
        [Description("سوخاری")]
        Sokhari,
        [Description("پاستا")]
        Pasta,
        [Description("استیک")]
        Stake,
        [Description("گریل")]
        Gril,

    }

}