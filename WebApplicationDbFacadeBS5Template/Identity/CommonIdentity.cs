using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.Extensions;
using WebApplicationDbFacadeBS5Template.Services;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.Identity
{
    public class CommonIdentity: IIdentity
    {
        public string Name { get; private set; }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }

        internal static CommonIdentity CreateAuthenticated(string name)
        => new CommonIdentity()
        {
            Name = name,
            AuthenticationType = "TokenClaim",
            IsAuthenticated = true
        };
        internal static CommonIdentity Public
        => new CommonIdentity()
        {
            AuthenticationType = "Public",
            IsAuthenticated = false
        };

    }

    public class CommonPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public Guid UserGuid { get; private set; }
        public Guid? ProxyUserGuid { get; private set; }
        internal Guid SessionGuid { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public bool IsAdmin { get; private set; }
        public bool IsProxyMode { get; private set; }
        public string ProxyName { get; private set; }
        public string Name { get; private set; }
        public string LastLogin { get; private set; }
        public string LastLoginDisplay { get; private set; }
        public Guid ActingUserGuid => IsProxyMode && ProxyUserGuid is Guid pGuid ? pGuid : UserGuid;
        private IEnumerable<string> Roles { get; set; }
        private IEnumerable<string> Groups { get; set; }

        internal static CommonPrincipal CreateAuthenticated(
            Guid userGuid,
            Guid? proxyUserGuid,
            Guid sessionGuid,
            DateTime expireDate,
            bool isAdmin,
            bool isProxyMode,
            string proxyName,
            string name,
            string lastLogin,
            string lastLoginDisplay,
            IEnumerable<string> roles,
            IEnumerable<string> groups,
            string identityName
            )
        {
            var user = new CommonPrincipal()
            {
                UserGuid = userGuid,
                ProxyUserGuid = proxyUserGuid,
                SessionGuid = sessionGuid,
                ExpireDate = expireDate,
                IsAdmin = isAdmin,
                IsProxyMode = isProxyMode,
                ProxyName = proxyName,
                Name=name,
                LastLogin = lastLogin,
                LastLoginDisplay = lastLoginDisplay,
                Roles= roles,
                Groups = groups
            };
            user.Identity = CommonIdentity.CreateAuthenticated(identityName);
            return user;
        }
        internal static CommonPrincipal Public
        => new CommonPrincipal()
        {
            Identity = CommonIdentity.Public
        };

        public bool IsInRole(string role)
        => Roles.Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
        public bool IsInGroup(string group)
        => Groups.Any(r => string.Equals(r, group, StringComparison.OrdinalIgnoreCase));

        public bool HasProxyAccessToUser(Guid proxyUserGuid)
        => IsAdmin || this.CheckProxyAccess(proxyUserGuid);

        public bool CanEditUserProfile(Guid userGuid)
        => ActingUserGuid == userGuid;
        public bool CanViewUserProfile(Guid userGuid)
        => ActingUserGuid == userGuid;
    }

}