using DbFacade.DataLayer.CommandConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.DomainLayer.Connection;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;
using WebApplicationDbFacadeBS5Template.Extensions;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Methods
{
    public class AppDbMethods
    {
        public static readonly IDbCommandMethod<ResolveUserIdentityParameters> ResolveUserIdentity
            = MainSQLDbConnection.ResolveUserIdentity.CreateMethod<ResolveUserIdentityParameters>(
                p => {
                    p.Add("EDIPI", p.Factory.Create(m => m.EDIDI));
                    p.Add("SSN", p.Factory.Create(m => m.SSN));
                    p.Add("FirstName", p.Factory.Create(m => m.FirstName));
                    p.Add("MiddleInitial", p.Factory.Create(m => m.MiddleInitial));
                    p.Add("LastName", p.Factory.Create(m => m.LastName));
                    p.Add("Email", p.Factory.Create(m => m.Email));
                    p.Add("UserGuid", p.Factory.OutputGuid());
                });
        public static readonly IDbCommandMethod<(Guid userGuid, int length), AppSession> CreateAppSession
            = MainSQLDbConnection.CreateAppSession.CreateMethod<(Guid userGuid, int length), AppSession>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m.userGuid));
                    p.Add("Length", p.Factory.Create(m => m.length));
                },
                v => {
                    v.Add(v.Rules.GreaterThan(m => m.length, 0));
                });
        public static readonly IDbCommandMethod<(Guid userGuid, Guid? proxyUserGuid), AppSession> UpdateAppSession
            = MainSQLDbConnection.UpdateAppSession.CreateMethod<(Guid userGuid, Guid? proxyUserGuid), AppSession>(
                p => {
                    p.Add("SessionGuid", p.Factory.Create(m => m.userGuid));
                    p.Add("ProxyUserGuid", p.Factory.Create(m => m.proxyUserGuid));
                });
        public static readonly IDbCommandMethod<Guid, AppSession> GetAppSession
            = MainSQLDbConnection.GetAppSession.CreateMethod<Guid, AppSession>(
                p => {
                    p.Add("SessionGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid> EndAppSession
            = MainSQLDbConnection.EndAppSession.CreateMethod<Guid>(
                p => {
                    p.Add("SessionGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<UserSearchParameters, UserSearchResult> UserSearch
            = MainSQLDbConnection.UserSearch.CreateMethod<UserSearchParameters, UserSearchResult>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m.UserGuid));
                    p.Add("FirstName", p.Factory.Create(m => m.FirstName));
                    p.Add("MiddleInitial", p.Factory.Create(m => m.MiddleInitial));
                    p.Add("LastName", p.Factory.Create(m => m.LastName));
                    p.Add("Email", p.Factory.Create(m => m.Email));
                    p.Add("EDIPI", p.Factory.Create(m => m.EDIPI));
                    p.Add("ExcludeAdmin", p.Factory.Create(m => m.ExcludeAdmin));
                    p.Add("OnlyAdmin", p.Factory.Create(m => m.OnlyAdmin));
                });
        public static readonly IDbCommandMethod<(string code, int? lastNDays), ApplicationUseMetricsResult> GetApplicationUseMetrics
            = MainSQLDbConnection.GetApplicationUseMetrics.CreateMethod<(string code, int? lastNDays), ApplicationUseMetricsResult>(
                p => {
                    p.Add("LastNDays", p.Factory.Create(m => m.lastNDays));
                    p.Add("Code", p.Factory.Create(m => m.code));
                });
        public static readonly IDbCommandMethod<int?, ApplicationUseByDayMetricsResult> GetApplicationUseByDayMetrics
            = MainSQLDbConnection.GetApplicationUseByDayMetrics.CreateMethod<int?, ApplicationUseByDayMetricsResult>(
                p => {
                    p.Add("LastNDays", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<UserSearchParameters, ApplicationUseSearchResult> ApplicationUseSearch
            = MainSQLDbConnection.ApplicationUseSearch.CreateMethod<UserSearchParameters, ApplicationUseSearchResult>(
                p => {
                    p.Add("FirstName", p.Factory.Create(m => m.FirstName));
                    p.Add("MiddleInitial", p.Factory.Create(m => m.MiddleInitial));
                    p.Add("LastName", p.Factory.Create(m => m.LastName));
                    p.Add("Email", p.Factory.Create(m => m.Email));
                    p.Add("EDIPI", p.Factory.Create(m => m.EDIPI));
                    p.Add("LastNDays", p.Factory.Create(m => m.LastNDays));
                });
        public static readonly IParameterlessDbCommandMethod<ApplicationUserCount> GetApplicationUserCount
            = MainSQLDbConnection.GetApplicationUserCount.CreateParameterlessMethod<ApplicationUserCount>();
        public static readonly IParameterlessDbCommandMethod<OnlineUserResult> GetOnlineUsers
            = MainSQLDbConnection.GetOnlineUsers.CreateParameterlessMethod<OnlineUserResult>();
        public static readonly IDbCommandMethod<Exception> LogApplicationError
            = MainSQLDbConnection.LogApplicationError.CreateMethod<Exception>(
                p => {
                    p.Add("Type", p.Factory.Create(m => m.GetType().Name));
                    p.Add("Message", p.Factory.Create(m => m.Message));
                    p.Add("StackTrace", p.Factory.Create(m => m.StackTrace));
                    p.Add("Source", p.Factory.Create(m => m.Source));
                    p.Add("InnerMessage", p.Factory.Create(m => m.InnerException is Exception e ? e.Message : ""));
                    p.Add("InnerStackTrace", p.Factory.Create(m => m.InnerException is Exception e ? e.StackTrace : ""));
                    p.Add("InnerSource", p.Factory.Create(m => m.InnerException is Exception e ? e.Source : ""));
                },
                v => {
                    v.Add(v.Rules.Required(m => m.GetType().Name));
                    v.Add(v.Rules.Required(m => m.Message));
                });
        public static readonly IDbCommandMethod<int?, ApplicationErrorMetricsResult> GetApplicationErrorMetrics
            = MainSQLDbConnection.GetApplicationErrorMetrics.CreateMethod<int?, ApplicationErrorMetricsResult>(
                p => {
                    p.Add("LastNDays", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid?, ApplicationErrorResult> GetApplicationErrors
            = MainSQLDbConnection.GetApplicationErrors.CreateMethod<Guid?, ApplicationErrorResult>(
                p => {
                    p.Add("ErrorGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid, UserRole> GetUserRoleLookup
            = MainSQLDbConnection.GetUserRoleLookup.CreateMethod<Guid, UserRole>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid, UserRole> GetGroupRoleLookup
            = MainSQLDbConnection.GetGroupRoleLookup.CreateMethod<Guid, UserRole>(
                p => {
                    p.Add("GroupGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid, UserGroup> GetUserGroupLookup
            = MainSQLDbConnection.GetUserGroupLookup.CreateMethod<Guid, UserGroup>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<Guid, AppFileRecord> GetFile
            = MainSQLDbConnection.GetFile.CreateMethod<Guid, AppFileRecord>(
                p => {
                    p.Add("FileGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<(Guid userGuid, HttpPostedFileBase file, string uploadType), AppFileRecord> UploadFile
            = MainSQLDbConnection.UploadFile.CreateMethod<(Guid userGuid, HttpPostedFileBase file, string uploadType), AppFileRecord>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m.userGuid));
                    p.Add("FileName", p.Factory.Create(m => m.file.FileName));
                    p.Add("Data", p.Factory.Create(m => m.file.ToArray()));
                    p.Add("MIMEType", p.Factory.Create(m => m.file.ContentType));
                    p.Add("UploadType", p.Factory.Create(m => m.uploadType));
                }, v => {
                    v.Add(v.Rules.Required(m => m.file.FileName));
                    v.Add(v.Rules.Required(m => m.file.ContentType));
                    v.Add(v.Rules.Required(m => m.uploadType));
                });
        public static readonly IDbCommandMethod<AppAnnouncementAddParameters, AppAnnouncement> AddAppAnnuncement
            = MainSQLDbConnection.AddAppAnnuncement.CreateMethod<AppAnnouncementAddParameters, AppAnnouncement>(
                p => {
                    p.Add("Title", p.Factory.Create(m => m.Title));
                    p.Add("Message", p.Factory.Create(m => m.Message));
                    p.Add("PriorityCode", p.Factory.Create(m => m.PriorityCode));
                    p.Add("ActiveDate", p.Factory.Create(m => m.ActiveDate));
                    p.Add("ExpireDate", p.Factory.Create(m => m.ExpireDate));
                    p.Add("ImageFileGuid", p.Factory.Create(m => m.ImageFileGuid));
                },
                v=> {
                    v.Add(v.Rules.Required(m => m.Title));
                    v.Add(v.Rules.Required(m => m.Message));
                    v.Add(v.Rules.Required(m => m.PriorityCode));
                });
        public static readonly IDbCommandMethod<AppAnnouncementUpdateParameters, AppAnnouncement> UpdateAppAnnuncement
            = MainSQLDbConnection.UpdateAppAnnuncement.CreateMethod<AppAnnouncementUpdateParameters, AppAnnouncement>(
                p => {
                    p.Add("Guid", p.Factory.Create(m => m.Guid));
                    p.Add("Title", p.Factory.Create(m => m.Title));
                    p.Add("Message", p.Factory.Create(m => m.Message));
                    p.Add("PriorityCode", p.Factory.Create(m => m.PriorityCode));
                    p.Add("ActiveDate", p.Factory.Create(m => m.ActiveDate));
                    p.Add("ExpireDate", p.Factory.Create(m => m.ExpireDate));
                    p.Add("ImageFileGuid", p.Factory.Create(m => m.ImageFileGuid));
                });
        public static readonly IDbCommandMethod<(Guid? guid, bool onlyActive), AppAnnouncement> GetAppAnnuncements
            = MainSQLDbConnection.GetAppAnnuncements.CreateMethod<(Guid? guid, bool onlyActive), AppAnnouncement>(
                p => {
                    p.Add("Guid", p.Factory.Create(m => m.guid));
                    p.Add("OnlyActive", p.Factory.Create(m => m.onlyActive));
                });
        public static readonly IDbCommandMethod<Guid, UserSettings> GetUserSettings
            = MainSQLDbConnection.GetUserSettings.CreateMethod<Guid, UserSettings>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m));
                });
        public static readonly IDbCommandMethod<(Guid userGuid, bool darkMode, Guid? profilePictureFileGuid), UserSettings> UpdateUserSettings
            = MainSQLDbConnection.UpdateUserSettings.CreateMethod<(Guid userGuid, bool darkMode, Guid? profilePictureFileGuid), UserSettings>(
                p => {
                    p.Add("UserGuid", p.Factory.Create(m => m.userGuid));
                    p.Add("DarkMode", p.Factory.Create(m => m.darkMode));
                    p.Add("ProfilePictureFileGuid", p.Factory.Create(m => m.profilePictureFileGuid));
                });
        public static readonly IParameterlessDbCommandMethod<AddressState> GetAddressStateLookupList
            = MainSQLDbConnection.GetAddressStateLookupList.CreateParameterlessMethod<AddressState>();
    }
}