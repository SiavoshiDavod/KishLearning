namespace SurveyWeb.Models.wrapper
{
    public class CartableUserAccessVm : BaseEntity
    {
        public string User { get; set; }
        public string Cartable { get; set; }
        public CartableType CartableType { get; set; }
        public string CartableTypeName { get { return CartableType.ToString(); } set { } }
    }
}