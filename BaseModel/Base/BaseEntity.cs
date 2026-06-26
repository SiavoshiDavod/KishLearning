using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BaseModel
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
                throw new Exception(current1);
            }
        }
    }

}
