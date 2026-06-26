using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurveyWeb.Models.Security
{
    [Description("دسترسی های نقش")]
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Permisstion Permisstion { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [NotMapped]
        public string act { get; set; }
        [NotMapped]
        public string PermisstionName
        {
            get
            {
                return SurveyWeb.EnumExtention.GetDescription<Permisstion>(this.Permisstion);
                // return this.QuestionType.ToString();
            }
            set { }
        }
    }
   }