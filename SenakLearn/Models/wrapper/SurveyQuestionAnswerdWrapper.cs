using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SenakLearn.Models.wrapper
{
    public class SurveyQuestionAnswerdWrapper
    {
        public string Name { get; set; }
        public int EntityId { get; set; }
        public List<SurveyQuestionWrapper> Questions { get; set; }
        public List<SurveyAnswerUserWrapper> AnswerUsers { get; set; }

    }
    public class SurveyQuestionWrapper
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }

    }
    public class SurveyAnswerUserWrapper
    {
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public string Question { get; set; }
        public int? UserId { get; set; }
        public DateTime? Create { get; set; }
        public string CreateStr { get; set; }
        public string Result { get; set; }
        public List<SurveyAnswerWrapper> Answers { get; set; }

    }
    public class SurveyAnswerWrapper
    {
        public int QuestionId { get; set; }
        public List<string> Result { get; set; }
       

    }

}