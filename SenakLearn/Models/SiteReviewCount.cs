using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    [Table("SiteReviewCount", Schema = "dbo")]
    public class SiteReviewCount
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Date { get; set; }
        //['بازدید سایت', 'کتاب', 'مقاله', 'دوره های آفلاین','کلاسهای آنلاین','نمایش ویدیو','ادوبی'];
        public int Adobe { get; set; }
        public int Video { get; set; }
        public int VideoNotFree { get; set; }
        public int Online { get; set; }
        public int Course { get; set; }
        public int Paper { get; set; }
        public int Book { get; set; }
        public int Site { get; set; }
        [NotMapped]
        public string DateF
        {
            get
            {
                var d = Date.ToString();
                if (d.Length != 8)
                {
                    return d;
                }
                return d.Substring(0, 4) + "/" + d.Substring(4, 2) + "/" + d.Substring(6, 2);
            }
        }

    }
    public enum SiteReviewCountType
    {
        Site,
        Book,
        Paper,
        Course,
        Online,
        Video,
        VideoNotFree,
        Adobe
    }
}