using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("ارتباطات کارتابل")]
    public class CartableRelation : BaseEntity
    {
        //[InverseProperty("From")]
        [ForeignKey("From")]
        public Cartable FromCartable { get; set; }

        //[InverseProperty("To")]
        [ForeignKey("To")]
        public Cartable ToCartable { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        [NotMapped]
        public string ToName { get; set; }
    }
}