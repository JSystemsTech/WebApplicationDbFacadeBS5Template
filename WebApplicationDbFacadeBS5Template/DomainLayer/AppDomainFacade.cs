using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.DomainLayer.Methods;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.DomainLayer
{
    public class AppDomainFacade
    {
        public static IDbResponse ResolveUserIdentity(ResolveUserIdentityParameters parameters, out Guid userGuid)
        {
            var response = AppDbMethods.ResolveUserIdentity.Execute(parameters);
            userGuid = response.GetOutputValue<Guid>("UserGuid");
            return response;
        }
        public static IDbResponse<AppSession> CreateAppSession(Guid sessionGuid, out AppSession result)
        {
            var response = AppDbMethods.CreateAppSession.Execute((sessionGuid, Saml2TokenConfiguration.ValidFor));
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppSession> UpdateAppSession(Guid userGuid, Guid? proxyUserGuid = null)
            => AppDbMethods.UpdateAppSession.Execute((userGuid, proxyUserGuid));
        public static IDbResponse<AppSession> GetAppSession(Guid sessionGuid)
            => AppDbMethods.GetAppSession.Execute(sessionGuid);
        public static IDbResponse EndAppSession(Guid sessionGuid)
            => AppDbMethods.EndAppSession.Execute(sessionGuid);
        public static IDbResponse<UserSearchResult> UserSearch(UserSearchParameters parameters)
            => AppDbMethods.UserSearch.Execute(parameters);
        public static IDbResponse<UserSearchResult> AdminUserSearch(UserSearchParameters parameters)
        {
            parameters.OnlyAdmin = true;
            return UserSearch(parameters);
        }
        public static IDbResponse<UserSearchResult> ProxyUserSearch(UserSearchParameters parameters)
        {
            parameters.ExcludeAdmin = true;
            return UserSearch(parameters);
        }
        public static IDbResponse<ApplicationUseMetricsResult> GetApplicationUseMetrics(string code, int? lastNDays= null)
            => AppDbMethods.GetApplicationUseMetrics.Execute((code, lastNDays));
        public static IDbResponse<ApplicationUseByDayMetricsResult> GetApplicationUseByDayMetrics(int? lastNDays = null)
            => AppDbMethods.GetApplicationUseByDayMetrics.Execute(lastNDays);
        public static IDbResponse<ApplicationUseSearchResult> ApplicationUseSearch(UserSearchParameters parameters)
            => AppDbMethods.ApplicationUseSearch.Execute(parameters);
        public static IDbResponse<ApplicationUserCount> GetApplicationUserCount(out ApplicationUserCount result)
        {
            var response = AppDbMethods.GetApplicationUserCount.Execute();
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<OnlineUserResult> GetOnlineUsers()
            => AppDbMethods.GetOnlineUsers.Execute();
        public static IDbResponse LogApplicationError<T>(T error)
            where T: Exception
            => AppDbMethods.LogApplicationError.Execute(error);
        public static IDbResponse<ApplicationErrorResult> GetApplicationErrors(Guid? errorGuid = null)
            => AppDbMethods.GetApplicationErrors.Execute(errorGuid);
        public static IDbResponse<ApplicationErrorMetricsResult> GetApplicationErrorMetrics(int? lastNDays = null)
            => AppDbMethods.GetApplicationErrorMetrics.Execute(lastNDays);
        public static IDbResponse<UserRole> GetUserRoleLookup(Guid userGuid)
            => AppDbMethods.GetUserRoleLookup.Execute(userGuid);
        public static IDbResponse<UserRole> GetGroupRoleLookup(Guid groupGuid)
            => AppDbMethods.GetGroupRoleLookup.Execute(groupGuid);
        public static IDbResponse<UserGroup> GetUserGroupLookup(Guid userGuid)
            => AppDbMethods.GetUserGroupLookup.Execute(userGuid);

        public static IDbResponse<AppFileRecord> GetFile(Guid fileGuid, out AppFileRecord result)
        {
            var response = AppDbMethods.GetFile.Execute(fileGuid);
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppFileRecord> UploadFile(Guid userGuid, HttpPostedFileBase file, out AppFileRecord result)
            => UploadFile(userGuid, file, "USER", out result);
        public static IDbResponse<AppFileRecord> UploadFile(Guid userGuid, HttpPostedFileBase file,string uploadType, out AppFileRecord result)
        {
            var response = AppDbMethods.UploadFile.Execute((userGuid,file, uploadType));
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppAnnouncement> AddAppAnnuncement(Guid userGuid, AppAnnouncementAddParameters parameters, out AppAnnouncement result)
        {
            if (!(parameters.ImageFileGuid is Guid) && parameters.ImageFile != null && parameters.ImageFile != default(HttpPostedFileBase))
            {
                UploadFile(userGuid, parameters.ImageFile, "AppAnnuncement", out AppFileRecord fileRecord);
                parameters.ImageFileGuid = fileRecord.Guid;
            }
            var response = AppDbMethods.AddAppAnnuncement.Execute(parameters);
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppAnnouncement> UpdateAppAnnuncement(Guid userGuid, AppAnnouncementUpdateParameters parameters, out AppAnnouncement result)
        {
            if(parameters.ImageFile != null && parameters.ImageFile != default(HttpPostedFileBase))
            {
                UploadFile(userGuid, parameters.ImageFile, "AppAnnuncement", out AppFileRecord fileRecord);
                parameters.ImageFileGuid = fileRecord.Guid;
            }            
            var response = AppDbMethods.UpdateAppAnnuncement.Execute(parameters);
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppAnnouncement> GetAppAnnuncement(Guid guid, out AppAnnouncement result)
        {
            var response = AppDbMethods.GetAppAnnuncements.Execute((guid,false));
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AppAnnouncement> GetAppAnnuncements(bool onlyActive = true)
        => AppDbMethods.GetAppAnnuncements.Execute((null, onlyActive));

        public static IDbResponse<UserSettings> GetUserSettings(Guid userGuid, out UserSettings result)
        {
            var response = AppDbMethods.GetUserSettings.Execute(userGuid);
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<UserSettings> UpdateUserSettings(Guid userGuid, bool darkMode, out UserSettings result)
        {
            var response = AppDbMethods.UpdateUserSettings.Execute((userGuid, darkMode, null));
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<UserSettings> UpdateUserProfilePicture(Guid userGuid, HttpPostedFileBase file, out UserSettings result)
        {
            GetUserSettings(userGuid, out UserSettings userSettings);
            UploadFile(userGuid,file,"ProfileImage", out AppFileRecord fileRecord);
            var response = AppDbMethods.UpdateUserSettings.Execute((userGuid, userSettings.DarkMode, fileRecord.Guid));
            result = response.FirstOrDefault();
            return response;
        }
        public static IDbResponse<AddressState> GetAddressStateLookupList()
        => AppDbMethods.GetAddressStateLookupList.Execute();
    }
}