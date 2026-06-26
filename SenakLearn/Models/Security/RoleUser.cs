using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models.Security
{
    [Description("نقش های کاربران")]
    public class RoleUser
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public learn_user User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

      
    }
}