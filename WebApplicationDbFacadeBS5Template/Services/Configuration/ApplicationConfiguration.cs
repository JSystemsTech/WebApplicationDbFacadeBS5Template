using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.Extensions;

namespace WebApplicationDbFacadeBS5Template.Services.Configuration
{
    public class ApplicationConfiguration
    {
        private static IDictionary<string, object> _AppSettings { get; set; }
        private static IDictionary<string, object> AppSettings
            => _AppSettings ?? ConfigurationManager.AppSettings.AllKeys.ToDictionary(key => key, key => (object)ConfigurationManager.AppSettings.Get(key));
        public static ApplicationEnvironment Environment => AppSettings.GetSetting<ApplicationEnvironment>("Environment");
        public static string EnvironmentName => 
            Environment == ApplicationEnvironment.Localhost ? "Localhost" :
            Environment == ApplicationEnvironment.Development ? "Development":
            Environment == ApplicationEnvironment.Evaluation ? "Evaluation" :
            Environment == ApplicationEnvironment.Production ? "Production" : 
            "Unknown";
        public static bool ShowEnvironmentName => AppSettings.GetSetting("ShowEnvironmentName", true);
        public static IEnumerable<string> Themes => AppSettings.GetEnumerableSetting<string>("Themes");
        public static string DefaultTheme => AppSettings.GetSetting<string>("DefaultTheme");
        public static string ApplicationName => AppSettings.GetSetting<string>("ApplicationName");
        public static string ApplicationDescription => AppSettings.GetSetting<string>("ApplicationDescription");
        public static string EncryptionKey => AppSettings.GetSetting<string>("EncryptionKey");
        public static string AuthenticationCookieName => AppSettings.GetSetting<string>("AuthenticationCookieName");
        public static string LogoutUrl => AppSettings.GetSetting<string>("LogoutUrl");
        public static string AsposeLicensePath => AppSettings.GetSetting<string>("AsposeLicensePath");
        public static string MainSQLDbConnectionKey => AppSettings.GetSetting<string>("MainSQLDbConnectionKey");
        public static string ProxyRequestParam => AppSettings.GetSetting<string>("ProxyRequestParam");
        public static string ProxyEndParam => AppSettings.GetSetting<string>("ProxyEndParam");
        public static string DarkModeChangeParam => AppSettings.GetSetting<string>("DarkModeChangeParam");
        public static string LastLoginDateFormat => AppSettings.GetSetting("LastLoginDateFormat", "MM/dd/yyyy HH:mm");
        public static string ExternalLoginTokenParam => AppSettings.GetSetting<string>("ExternalLoginTokenParam");

    }
    public class Saml2TokenConfiguration
    {
        private static IDictionary<string, object> _Settings { get; set; }
        protected static IDictionary<string, object> Settings
            => _Settings ??
            (ConfigurationManager.GetSection(SectionName) is NameValueCollection settings ?
            settings.AllKeys.ToDictionary(key => key, key => (object)settings.Get(key)) :
            new Dictionary<string, object>());

        protected static string SectionName = "saml2TokenSettings"; 
        private static string AudienceUriValue => Settings.GetSetting<string>("AudienceUri");
        private static Uri _AudienceUri { get; set; }
        public static Uri AudienceUri => _AudienceUri ?? new Uri(AudienceUriValue);
        public static string ConfirmationMethod => Settings.GetSetting<string>("ConfirmationMethod");
        public static string Issuer => Settings.GetSetting<string>("Issuer");
        public static string Namespace => Settings.GetSetting<string>("Namespace");
        public static string SubjectName => Settings.GetSetting<string>("SubjectName");
        public static int ValidFor => Settings.GetSetting<int>("ValidFor");
        public static int TimeoutWarning => Settings.GetSetting("TimeoutWarning",2) * 60 * 1000;//set to milliseconds

    }
    public class TokenClaimConfiguration
    {
        private static IDictionary<string, object> _Settings { get; set; }
        protected static IDictionary<string, object> Settings
            => _Settings ??
            (ConfigurationManager.GetSection(SectionName) is NameValueCollection settings ?
            settings.AllKeys.ToDictionary(key => key, key => (object)settings.Get(key)) :
            new Dictionary<string, object>());

        protected static string SectionName = "tokenClaimSettings";
        public static string SessionGuidClaim => Settings.GetSetting<string>("SessionGuidClaim");
        public static string ExpireDateClaim => Settings.GetSetting<string>("ExpireDateClaim");

    }
    public class DateTimeFormatConfiguration
    {
        private static IDictionary<string, object> _Settings { get; set; }
        protected static IDictionary<string, object> Settings
            => _Settings ??
            (ConfigurationManager.GetSection(SectionName) is NameValueCollection settings ?
            settings.AllKeys.ToDictionary(key => key, key => (object)settings.Get(key)) :
            new Dictionary<string, object>());

        protected static string SectionName = "dateTimeFormatSettings";
        public static string Date => Settings.GetSetting("Date", "yyyy/MM/dd");
        public static string DateTime => Settings.GetSetting("DateTime", "yyyy/MM/dd hh:mm tt");
        public static string DateTime24Hour => Settings.GetSetting("DateTime24Hour", "yyyy/MM/dd HH:mm");
        public static string DateTimeWithSeconds => Settings.GetSetting("DateTimeWithSeconds", "yyyy/MM/dd hh:mm:ss tt");
        public static string DateTime24HourWithSeconds => Settings.GetSetting("DateTime24HourWithSeconds", "yyyy/MM/dd HH:mm:ss");
        public static string DateTimeMomentSafe => Settings.GetSetting("DateTimeMomentSafe", "yyyy-MM-ddTHH:mm:ss.fffK");
    }
}