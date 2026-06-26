namespace SurveyWeb.Models.wrapper
{
    public class CartableLogVM : BaseEntity
    {
        public string Description { get; set; }
        public string User { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public CartableType CartableType { get; set; }
        public string CartableTypeName { get { return CartableType.ToString(); } set { } }
    }
}