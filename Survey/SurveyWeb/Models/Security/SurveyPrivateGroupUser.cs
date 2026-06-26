using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.Security
{
    [Description("کاربران گروه اختصاصی نظرسنجی")]
    public class SurveyPrivateGroupUser
    {
        [Key]
        public int Id { get; set; }
        public int SurveyPrivateGroupId { get; set; }

        [ForeignKey("SurveyPrivateGroupId")]
        public Security.SurveyPrivateGroup SurveyPrivateGroup { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}