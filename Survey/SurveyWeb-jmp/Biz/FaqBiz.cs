namespace SurveyWeb.Biz
{
    public class FaqBiz : RepositoryBase<Models.Faq>
    {
        public static readonly FaqBiz Instance = new FaqBiz();
    }
    public class RegulationBiz : RepositoryBase<Models.Regulation>
    {
        public static readonly RegulationBiz Instance = new RegulationBiz();
    }
    
}