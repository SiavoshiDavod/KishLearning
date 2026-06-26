using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace SenakLearn.Models
{
    [Description("گروه آزمون")]
    public class GroupAzmoon:BaseEntity
    {
        public GroupAzmoon()
        {
            AzmoonEntities = new HashSet<AzmoonEntity>(); ;
        }
        [GenericRequired,GenericStringLength(20)]
        [System.ComponentModel.DataAnnotations.Display(Name = "نام گروه")]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<AzmoonEntity> AzmoonEntities { get; set; }
    }
}