using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("لاگ کارتابل")]
    public class CartableLog : BaseEntity
    {
        [GenericMaxLength(1000)]
        public string Description { get; set; }
        public int EntityId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("From")]
        public Cartable FromCartable { get; set; }
        [ForeignKey("To")]
        public Cartable ToCartable { get; set; }
        public int From { get; set; }
        public int To { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Display(Name = "نوع کارتابل ")]
        public CartableType CartableType { get; set; }

    }
}