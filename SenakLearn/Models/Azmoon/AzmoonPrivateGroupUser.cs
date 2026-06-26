using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models.Security
{
    [Description("کاربران گروه اختصاصی آزمون")]
    public class AzmoonPrivateGroupUser
    {
        [Key]
        public int Id { get; set; }
        public int AzmoonPrivateGroupId { get; set; }

        [ForeignKey("AzmoonPrivateGroupId")]
        public Security.AzmoonPrivateGroup AzmoonPrivateGroup { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public learn_user User { get; set; }
    }
}