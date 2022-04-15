using DbFacade.DataLayer.ConnectionService;
using DbFacade.Factories;
using DbFacade.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Connection
{
    internal class MainSQLDbConnection: SqlConnectionConfig<MainSQLDbConnection>
    {
        private MainSQLDbConnection() {  }
        public static MainSQLDbConnection Connection = new MainSQLDbConnection();
        
        protected override string GetDbConnectionString() 
            => ConnectionStringManager.GetConnectionStringConfig(ApplicationConfiguration.MainSQLDbConnectionKey).connectionString;

        protected override string GetDbConnectionProvider() 
            => ConnectionStringManager.GetConnectionStringConfig(ApplicationConfiguration.MainSQLDbConnectionKey).connectionProvider;

        protected override async Task<string> GetDbConnectionStringAsync()
        {
            var config = await ConnectionStringManager.GetConnectionStringConfigAsync(ApplicationConfiguration.MainSQLDbConnectionKey);
            return config.connectionString;
        }

        protected override async Task<string> GetDbConnectionProviderAsync()
        {
            var config = await ConnectionStringManager.GetConnectionStringConfigAsync(ApplicationConfiguration.MainSQLDbConnectionKey);
            return config.connectionProvider;
        }

        protected override void OnError(Exception ex, IDbCommandSettings dbCommandSettings)
        {
            if(dbCommandSettings.CommandId != LogApplicationError.CommandId)
            {
                AppDomainFacade.LogApplicationError(ex);
            }
        }

        public static IDbCommandConfig ResolveUserIdentity = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Identity_Resolve]", "Resolve User Identity");
        public static IDbCommandConfig CreateAppSession = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppSession_Create]", "Create App Session");
        public static IDbCommandConfig UpdateAppSession = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppSession_Update]", "Update App Session");
        public static IDbCommandConfig GetAppSession = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppSession_Get]", "Get App Session");
        public static IDbCommandConfig EndAppSession = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppSession_End]", "End App Session");
        public static IDbCommandConfig GetUserRoles = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Access_Role_Resolve]", "Get User Roles");
        public static IDbCommandConfig GetUserGroups = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Access_Group_Resolve]", "Get User Groups");
        public static IDbCommandConfig UserSearch = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Search]", "User Search");
        public static IDbCommandConfig GetApplicationUseMetrics = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[METRICS_Application_Use_Get]", "Get Application Use Metrics");
        public static IDbCommandConfig GetApplicationUseByDayMetrics = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[METRICS_Application_Use_ByDay_Get]", "Get Application Use By Day Metrics");
        public static IDbCommandConfig ApplicationUseSearch = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[METRICS_Application_Use_ByUser_Search]", "Application Use Search");
        public static IDbCommandConfig GetApplicationUserCount = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[METRICS_Application_User_Count_Get]", "Get Application User Count");
        public static IDbCommandConfig GetOnlineUsers = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Online_Get]", "Get Online Users");
        public static IDbCommandConfig LogApplicationError = DbCommandConfigFactory<MainSQLDbConnection>.CreateTransactionCommand("[dbo].[AppError_Add]", "Log Application Error");
        public static IDbCommandConfig GetApplicationErrors = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppError_Get]", "Get Application Errors");
        public static IDbCommandConfig GetApplicationErrorMetrics = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[METRICS_Application_Errors_ByDay_Get]", "Get Application Error Metrics");

        public static IDbCommandConfig GetGroupRoleLookup = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Access_Role_LU_Group_Get]", "Get Group Role Lookup");

        public static IDbCommandConfig GetUserRoleLookup = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Access_Role_LU_User_Get]", "Get User Role Lookup");
        public static IDbCommandConfig GetUserGroupLookup = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Access_Group_LU_User_Get]", "Get User Group Lookup");

        public static IDbCommandConfig GetFile = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppFileCache_Get]", "Get File");
        public static IDbCommandConfig UploadFile = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppFileCache_Add]", "Upload File");

        public static IDbCommandConfig GetAppAnnuncements = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppAnnoncement_Get]", "Get App Annuncements");
        public static IDbCommandConfig AddAppAnnuncement = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppAnnoncement_Add]", "Add App Annuncement");
        public static IDbCommandConfig UpdateAppAnnuncement = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[AppAnnoncement_Update]", "Update App Annuncement");

        public static IDbCommandConfig GetUserSettings = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Settings_Get]", "Get User Settings");
        public static IDbCommandConfig UpdateUserSettings = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[User_Settings_Update]", "Update User Settings");

        public static IDbCommandConfig GetAddressStateLookupList = DbCommandConfigFactory<MainSQLDbConnection>.CreateFetchCommand("[dbo].[LU_State_Get]", "Get Address State Lookup List");
        
    }
}