using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class AzmoonQuestionAnswerdWrapper
    {
        public string Name { get; set; }
        public int EntityId { get; set; }
        public List<AzmoonQuestionWrapper> Questions { get; set; }
        public List<AzmoonAnswerUserWrapper> AnswerUsers { get; set; }
    }
    public class AzmoonQuestionWrapper
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionGroup { get; set; }
        public int QuestionType { get; internal set; }
    }
    public class AzmoonAnswerUserWrapper
    {
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public string Question { get; set; }
        public int? UserId { get; set; }
        public DateTime? Create { get; set; }
        public string CreateStr { get; set; }
        public string Result { get; set; }
        public List<AzmoonAnswerWrapper> Answers { get; set; }

    }
    public class AzmoonAnswerWrapper
    {
        public int QuestionId { get; set; }
        public List<string> Result { get; set; }


    }
}