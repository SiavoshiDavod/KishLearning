using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models
{
    [Description("دسترسی کارتابل")]
    public class CartableUserAccess : BaseEntity
    {
        public int UserId { get; set; }
        [ForeignKey("CartableId")]
        public Cartable Cartable { get; set; }
        public int CartableId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}