using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SurveyWeb
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
    public abstract class BaseEntity//<TPrimaryKey>
        : IBaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }

        //[NotMapped]
        //public string CreatedDateShamsi => CreatedDate.ToPersianDate();
        [NotMapped]
        public string CreatedDateShamsi
        {
            get { return CreatedDate.ToPersianDate(); }
            set { CreatedDate = value.ToGregorianDate().Value; }
        }
        public virtual DateTime? UpdateDate { get; set; }
        [NotMapped]
        public virtual string UpdateDateShamsi
        {
            get { return UpdateDate?.ToPersianDate(); }
            set { UpdateDate = value.ToGregorianDate(); }
        }
        [NotMapped]
        public virtual string act { get; set; }

        #region transient props

        #endregion
        public virtual void Validate()
        {
            List<ValidationResult> resultList = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), resultList, true);
            if (resultList.Count > 0)
            {//throw new BusinessException(resultList.First().ErrorMessage);
                var current1 = "";
                current1 = resultList.Aggregate(current1,
                                    (current, ve) => current + (ve.ErrorMessage + "</br>"));
                throw new HandledException(current1);
            }
        }
    }
    public enum Province : byte
    {
        [Description("تهران")]
        Tehran = 21,
        [Description("البرز")]
        Alborz = 26,
        [Description("قم")]
        Qum = 25,
        [Description("مرکزی")]
        Markazi = 86,
        [Description("زنجان")]
        Zanjan = 24,
        [Description("سمنان")]
        Semnam = 23,
        [Description("همدان")]
        Hamadan = 81,
        [Description("قزوین")]
        Qazvin = 28,
        [Description("اصفهان")]
        Isfahan = 31,
        [Description("آذربایجان غربی")]
        AzerbaijanGharbi = 44,
        [Description("مازندران")]
        Mazandaran = 11,
        [Description("کهگیلویه و بویراحمد")]
        KohgiluyehBoyerAhmad = 74,
        [Description("کرمانشاه")]
        Kermanshah = 83,
        [Description("خراسان رضوی")]
        KhorasanRazavi = 51,
        [Description("اردبیل")]
        Ardabil = 45,
        [Description("گلستان")]
        Golestan = 17,
        [Description("آذربایجان شرقی")]
        AzerbaijanSharghi = 41,
        [Description("سیستان و بلوچستان")]
        SistanBaluchestan = 54,
        [Description("کردستان")]
        Kordestan = 87,
        [Description("فارس")]
        Fars = 71,
        [Description("لرستان")]
        Lorestan = 66,
        [Description("کرمان")]
        Kerman = 34,
        [Description("خراسان جنوبی")]
        KhorasanJonobi = 56,
        [Description("گیلان")]
        Gilan = 13,
        [Description("بوشهر")]
        Bousher = 77,
        [Description("هرمزگان")]
        Hormozgan = 76,
        [Description("خوزستان")]
        Khozestan = 61,
        [Description("چهار محال و بختیاری")]
        ChaharMahaalBakhtiari = 38,
        [Description("خراسان شمالی")]
        KhorasanShomali = 58,
        [Description("یزد")]
        Yazd = 35,
        [Description("ایلام")]
        Ilam = 84,
    }
}