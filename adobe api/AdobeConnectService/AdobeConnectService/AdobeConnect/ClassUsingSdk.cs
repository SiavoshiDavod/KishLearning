using AdobeConnectSDK;
using AdobeConnectSDK.Common;
using AdobeConnectSDK.Extensions;
using AdobeConnectSDK.Interfaces;
using AdobeConnectSDK.Model;
using AdobeConnectService.AdobeConnect.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AdobeConnectService
{
    public class ClassUsingSdk
    {
        private static CookieContainer cookieContainer;
        private static HttpClientHandler clienthandler;
        private HttpClient httpClient;
        //private const string ServiceURL = "http://46.209.20.165/api/xml";
        public string User { get; private set; }
        public string Password { get; private set; }
        public UserInfoViewModel userInfoViewModel { get; private set; }
        public bool IsLogin { get; private set; }
        public AdobeConnectXmlAPI Api { get; private set; }
        private IMapper mapper { get; set; }
        public ClassUsingSdk(string user /*= "rahmatymahdi@gmail.com"*/, string pass /*= "123456"*/)
        {
            cookieContainer = new CookieContainer();
            clienthandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = cookieContainer };
            httpClient = new HttpClient(clienthandler);
            User = user;
            Password = pass;
            ISdkSettings sdkSetting = new SdkSettings { httpClient = httpClient, ServiceURL = AdobeConnectSDK.Common.Constants.ServiceURL, NetUser = User, NetPassword = Password };
            ICommunicationProvider CommunicationProvider = new HttpCommunicationProvider() { Settings = sdkSetting };
            Api = new AdobeConnectXmlAPI(CommunicationProvider, sdkSetting);

            var s = Api?.Login();
            if (s.Code == StatusCodes.OK)
                IsLogin = true;
            else
                throw new Exception("ایمیل یا رمز عبور در سامانه ادوبی معتبر نیست");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInfo, UserInfoViewModel>();
                cfg.CreateMap<MeetingItem, MeetingItemViewModel>();
                cfg.CreateMap<PrincipalListItem, PrincipalListItemViewModel>();
                cfg.CreateMap<ScoShortcut, ScoShortcutViewModel>();
                cfg.CreateMap<EventInfo, EventInfoViewModel>();
                cfg.CreateMap<MeetingUpdateItemViewModel, MeetingUpdateItem>();
                cfg.CreateMap<MeetingDetail, MeetingDetailViewModel>();
            });
            mapper = config.CreateMapper();
        }

        //downloadFiles
        //http://server-domain/url-path/output/url-path.zip?download=zip

        #region Principal
        public IEnumerable<PrincipalListItem> GetPrincipalList(PrincipalFilter model, bool? isUser = true, bool IsMemberOfGroupIdFilter = false)
        {
            var filter = new List<string>();
            if (model != null)
            {
                if (model.GroupId != null)
                {
                    filter.Add("group-id=" + model.GroupId);
                }
                if (model.PrincipalId != null)
                {
                    filter.Add("principal-id=" + model.PrincipalId);
                }
                if (!string.IsNullOrEmpty(model.Email))
                {
                    filter.Add("filter-login=" + model.Email);
                }
                if (!string.IsNullOrEmpty(model.Name))
                {
                    filter.Add("filter-name=" + model.Name);
                }
                if (!string.IsNullOrEmpty(model.LikeName))
                {
                    filter.Add("filter-like-name=" + model.LikeName);
                }

                if (model.ManagerId != null)
                {
                    filter.Add("filter-manager-id=" + model.ManagerId);
                }
            }

            if (IsMemberOfGroupIdFilter)
            {
                filter.Add("filter-is-member=true");
            }

            if (isUser == true)
            {
                filter.Add("filter-type=user");
            }
            else if (isUser == false)
            {
                filter.Add("filter-type=group");
            }
            var s = this.Api?.GetPrincipalList(string.Join("&", filter));

            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);

            return new List<PrincipalListItem>();

        }
        public PrincipalInfo GetPrincipalById(long principalId)
        {
            var s = this.Api?.GetPrincipalInfo(principalId.ToString());

            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);

            throw new Exception("کاربری با مشخصات داده شده یافت نشد");
        }

        public Principal UserCreate(PrincipalSetupViewModel model, bool isCreated = true)
        {
            PrincipalSetup principalSetup = new PrincipalSetup
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Login = model.Email,
                Email = model.Email,
                Description = model.Description,
                Password = model.Password,
                SendEmail = true,
                PrincipalType = PrincipalTypes.user,
                HasChildren = false
            };
            if (!isCreated)
            {
                principalSetup.PrincipalId = model.PrincipalId;
            }
            return PrincipalUpdate(principalSetup);
        }

        public Principal GroupCreate(PrincipalSetupGroupViewModel model, bool isCreated = true)
        {
            PrincipalSetup principalSetup = new PrincipalSetup
            {
                Name = model.Name,
                Description = model.Description,
                PrincipalType = PrincipalTypes.group,
                HasChildren = true
            };
            if (!isCreated)
            {
                principalSetup.PrincipalId = model.PrincipalId;
            }
            return PrincipalUpdate(principalSetup);
        }

        private Principal PrincipalUpdate(PrincipalSetup principalSetup)
        {
            Principal ret = null;
            ApiStatus s = this.Api?.PrincipalUpdate(principalSetup, out ret);
            if (s.Code == StatusCodes.OK && ret != null)
            {
                return ret;
            }
            Result(s);
            throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception + " " + s.ResultDocument);
        }

        private bool Result(ApiStatus s)
        {
            if (s.Code == StatusCodes.OK)
            {
                return true;
            }
            throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception + " " + s.ResultDocument);
        }

        public bool UserRemove(long principalId)
        {
            ApiStatus s = this.Api?.PrincipalDelete(new[] { principalId.ToString() });
            return Result(s);
        }

        public bool ResetPassword(long userId, string newPass)
        {
            var s = this.Api?.UpdatePassword(userId.ToString(), newPass);
            return Result(s);
        }
        public bool ChangePassword(string newPass)
        {
            var userId = GetCurrentUserInfoViewModel()?.UserId;
            var s = this.Api?.PrincipalUpdatePwd(userId, Password, newPass);
            return Result(s);
        }

        public bool GroupMembershipUpdate(long groupId, long userId, bool isMember = true)
        {
            var s = this.Api?.PrincipalGroupMembershipUpdate(groupId.ToString(), userId.ToString(), isMember);
            return Result(s);
        }

        public UserInfoViewModel GetCurrentUserInfoViewModel()
        {
            if (!string.IsNullOrEmpty(userInfoViewModel?.UserId))
            {
                return userInfoViewModel;
            }
            var source = this.Api?.UserInfo.Result;
            if (source == null) return null;
            userInfoViewModel = mapper.Map<UserInfo, UserInfoViewModel>(source);
            return userInfoViewModel;
        }

        public void Cleanup()
        {
            this.Api?.Logout();
            Api = null;
        }

        #endregion Principal

        #region Permission

        public IEnumerable<PermissionInfo> GetPermissionsInfo(PermaissionFilter model)
        {
            var s = this.Api?.GetPermissionsInfo(model.AclId.ToString(), model.PrincipalId.ToString(), "");
            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new List<PermissionInfo>();
        }

        public bool PermissionsReset(long aclId)
        {
            var s = this.Api?.PermissionsReset(aclId.ToString());
            return Result(s);
        }

        public bool PermissionsUpdate(PermaissionFilter model, PermissionId permissionId)
        {
            var s = this.Api?.PermissionsUpdate(model.AclId.ToString(), model.PrincipalId.ToString(), permissionId);
            return Result(s);
        }
        public bool PermissionSubscriptionUpdate(PermaissionFilter model, bool SubscripeUnSubscripe)
        {
            if (SubscripeUnSubscripe)
            {
                var s = this.Api?.ParticipantSubscribe(model.AclId.ToString(), model.PrincipalId.ToString());
                return Result(s);
            }
            else
            {
                var s = this.Api?.ParticipantUnsubscribe(model.AclId.ToString(), model.PrincipalId.ToString());
                return Result(s);
            }
          
        }
        public bool SpecialPermissionsUpdate(long aclId, SpecialPermissionId specialPermissionId)
        {
            var s = this.Api?.SpecialPermissionsUpdate(aclId.ToString(), specialPermissionId);
            return Result(s);
        }

        #endregion Permission

        #region SCO

        public IEnumerable<ScoShortcut> GetSCOshotcuts(bool isMeeting = true)
        {
            var s = this.Api?.GetSCOshotcuts();

            if (s.Result != null)
            {
                if (isMeeting)
                    return s.Result.Where(shortcut => shortcut.Type == "meetings");

                return s.Result;
            }

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new List<ScoShortcut>();
        }

        public MeetingDetail GetMeetingDetail(long scoId)
        {
            var s = this.Api?.GetMeetingDetail(scoId.ToString());
            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new MeetingDetail();
        }

        public IEnumerable<MeetingItem> GetAllMeetings(string likeName)
        {
            var s = this.Api?.GetAllMeetings(likeName);
            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new List<MeetingItem>();

        }

        public IEnumerable<MeetingItem> GetAllMeetings(long scoId)
        {
            var s = this.Api?.GetMeetingsInRoom(scoId.ToString());
            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new List<MeetingItem>();

        }
        public bool ScoDelete(long scoId)
        {
            var s = this.Api?.ScoDelete(new[] { scoId.ToString() });
            return Result(s);
        }

        public MeetingDetail MeetingUpdate(MeetingUpdateItemViewModel model, bool isCreated = true)
        {
            MeetingDetail ret = null;
            MeetingUpdateItem item = new MeetingUpdateItem()
            {
                Name = model.Name,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ScoId = /*isCreated ? null :*/ model.ScoId?.ToString(),
                FolderId = /*isCreated ?*/ model?.ToString()/* : null*/,
                UrlPath = model.UrlPath,
                ScoTag = model.ScoTag,
                Description = model.Description
            };
            //ApiStatus s = this.Api?.ScoUpdate(item, out ret);
            ApiStatus s = null;
            if (isCreated)
            {
                s = this.Api?.MeetingCreate(item, out ret);
            }
            else
            {
                s = this.Api?.MeetingUpdate(item, out ret);
            }
            if (s.Code == StatusCodes.OK && ret != null)
            {
                return ret;
            }
            Result(s);
            throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception + " " + s.ResultDocument);
        }

        #endregion SCO


        public IEnumerable<MeetingItem> GetMyMeetings(string likeName)
        {
            var s = this.Api?.GetMyMeetings();
            if (s.Result != null)
                return s.Result;

            if (s.Code != StatusCodes.OK)
                throw new Exception(s.Code + " " + s.SubCode + " " + s.InvalidField + " " + s.Exception);
            return new List<MeetingItem>();
        }

        public bool GetQuizzesInRoom(long scoId)
        {
            var s = this.Api?.GetQuizzesInRoom(scoId.ToString());
            return Result(s);
        }
        public IEnumerable<EventInfoViewModel> ReportMyEvents()
        {
            var source = this.Api?.ReportMyEvents().Result;
            if (source == null) return null;
            IEnumerable<EventInfoViewModel> destination = mapper.Map<IEnumerable<EventInfo>, IEnumerable<EventInfoViewModel>>(source);
            return destination;
        }
    }
}
