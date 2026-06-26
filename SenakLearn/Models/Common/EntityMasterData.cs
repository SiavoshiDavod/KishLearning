using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.Common
{
    [DisplayName("اطلاعات پایه")]
    public class EntityMasterData : BaseEntity
    {
        [Key]
        public override int Id { get; set; }
        public  string Title { get; set; }
        public int TypeEntity { get; set; }
        [NotMapped]
        public override DateTime CreatedDate { get; set; }
        [NotMapped]
        public override DateTime? UpdateDate { get; set; }
    }
}