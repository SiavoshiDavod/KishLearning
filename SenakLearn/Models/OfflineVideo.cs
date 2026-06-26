using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class OfflineVideo : ParentChildEntity
    {
        //[NonEmptyGuid]
        [Display(Name = "ویدیو/صدا")]
        public Guid VideoId { get; set; }
        [Display(Name = "رایگان است؟")]
        public bool IsFree { get; set; }
        [Display(Name = "دوره/پادکست")]
        public int learn_coursId { get; set; }
        [NotMapped]
        public string Title { get; set; }
        [ForeignKey("learn_coursId"),JsonIgnore]
        public learn_cours learn_cours { get; set; }

        public override void Validate()
        {
            base.Validate();
            if (VideoId==null || VideoId==Guid.Empty)
                throw new Exception("لطفا ویدیو را وارد نمایید.");
        }
    }
}