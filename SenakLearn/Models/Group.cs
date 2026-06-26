using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Table("Group", Schema = "dbo")]
    public  class Group: BaseEntity
    {
        [Display(Name = "نام گروه ")]
        public string DropDownTitle { get; set; }
    }
    [Table("GroupDetail", Schema = "dbo")]
    public class GroupDetail : BaseEntity
    {
        [Display(Name = "نام و نام خانوادگی")]
        public string Name { get; set; }
        [Display(Name = "شماره موبایل"),GenericMaxLength(11)]
        public string Mobile { get; set; }
        [Display(Name = "ایمیل"),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "نام شرکت یا شغل")]
        public string Company { get; set; }
        [Display(Name = "تاریخ تولد")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
        [NotMapped]
        public virtual string BirthDayShamsi
        {
            get => BirthDay?.ToPersianDate();
            set => BirthDay = value.ToGregorianDate();
        }
        [Display(Name = "گروه کاربری")]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}