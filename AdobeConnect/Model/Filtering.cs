using AdobeConnectSDK.Model;

namespace AdobeConnectService.AdobeConnect.Model
{
    public class PrincipalFilter
    {
        public long? GroupId { get; set; }
        public long? PrincipalId { get; set; }
        public long? ManagerId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LikeName { get; set; }
    }
    public class PermaissionFilter
    {
        public long AclId { get; set; }
        public long PrincipalId { get; set; }
    }
    public class ScoFilter
    {
        public long SocId { get; set; }
        public string filterName { get; set; }
        public string filterGtDate  { get; set; }
        public string filterltDate  { get; set; }
        public string filterLikeName { get; set; }
    }
}
