using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SenakLearn.Models
{

    public interface IBaseEntity
    {
        int Id { get; set; }
    }
    public abstract class BaseEntity//<TPrimaryKey>
        :IBaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        [NotMapped]
        public virtual string CreatedDateShamsi
        {
            get => CreatedDate.ToPersianDateTime();
            set => CreatedDate = value.ToGregorianDate();
        }
        public virtual DateTime? UpdateDate { get; set; }
        [NotMapped]
        public virtual string UpdateDateShamsi
        {
            get => UpdateDate?.ToPersianDate();
            set => UpdateDate = value.ToGregorianDate();
        }
        [NotMapped]
        public virtual string act { get; set; }
       // [NotMapped]
       // public virtual string DropDownTitle { get; set; }
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
                throw new Exception(current1);
            }
        }
    }
}