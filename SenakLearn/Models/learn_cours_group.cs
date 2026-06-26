using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    //[DisplayName("گروه بندی دوره ها")]
    //[DisplayPluralName("گروه بندی های دوره ها")]
    public class learn_cours_group
    {
        //public learn_cours_groupMetadata()
        //{
        //    this.learn_cours = new HashSet<learn_cours>();
        //}
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "عنوان گروه دوره")]
        [MaxLength(50, ErrorMessage = "حداکثر طول {0} تعداد {1} می باشد")]
        public string name { get; set; }
        [Required(ErrorMessage = "وارد کردن {0} الزامی است")]
        [Display(Name = "فعال/غیرفعال")]
        [JsonIgnore]
        public bool status { get; set; }
        [Display(Name = "کلاسهای آنلاین")]
        public bool Online { get; set; }
        [Display(Name = "کلاسهای آفلاین")]
        public bool Offline { get; set; }
        [Display(Name = "مقالات")]
        public bool Paper { get; set; }
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "عکس")]
        public string ImageUrl { get; set; }
        [Display(Name = "ترتیب")]
        public int Order { get; set; }

        [Display(Name = "کتاب")]
        public bool Book { get; set; }

        [Display(Name = "جزوه")]
        public bool Booklet { get; set; }

        [Display(Name = "تعداد کلاسهای آنلاین")]
        public int OnlineCount { get; set; }
        [Display(Name = "تعداد کلاسهای آفلاین")]
        public int OfflineCount { get; set; }
        [Display(Name = "تعداد مقالات")]
        public int PaperCount { get; set; }
        [Display(Name = "تعداد کتاب")]
        public int BookCount { get; set; }

        [Display(Name = "تعداد جزوه")]
        public int BookletCount { get; set; }
        //public virtual ICollection<learn_cours> learn_cours { get; set; }
        [NotMapped]
        public int SumCount => BookCount + BookletCount + PaperCount + OfflineCount + OnlineCount;
    }
    public enum CoursGroupCountType
    {
        Book,
        Booklet,
        Paper,
        Online,
        Offline
    }
}
