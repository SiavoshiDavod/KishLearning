namespace SiteSetting
{
    public class GetSetting
    {
        public static readonly GetSetting Instance = new GetSetting();
        public static SiteSetting setting = null;
        public SiteSetting Get()
        {
            if (setting==null)
            {
                Set(new SiteSetting());
            }
            return setting;
        }
        public void Set(SiteSetting set)
        {
            setting = set;
        }
    }
}
