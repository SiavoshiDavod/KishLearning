using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SenakLearn.Models
{
    public class OnlineClassAccorationDetails : ParentChildEntity
    {
        [Display(Name = "عنوان")]
        public int OnlineClassAccorationId { get; set; }
        [ForeignKey("OnlineClassAccorationId"), JsonIgnore]
        public OnlineClassAccoration OnlineClassAccoration { get; set; }
        [ForeignKey("ParentId"), JsonIgnore]
        public OnlineClassAccorationDetails Parent { get; set; }
        [JsonIgnore]
        public ICollection<OnlineClassAccorationDetails> Childs { get; set; }
       

    }
    public class OnlineClassAccorationDetailsViewModel
    {
        public int Id { get; set; }
        public int OnlineClassAccorationId { get; set; }
        public string OnlineClassAccoration { get; set; }
        public string Parent { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string act { get; set; }
    }

}