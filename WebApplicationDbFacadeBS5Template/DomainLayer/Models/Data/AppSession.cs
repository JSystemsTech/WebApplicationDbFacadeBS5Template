using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.Services.Configuration;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class AppSession: DbDataModel
    {
        public Guid Guid { get; private set; }
        public Guid UserGuid { get; private set; }
        public Guid? ProxyUserGuid { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public bool IsProxyMode { get; private set; }
        public bool IsActive { get; private set; }
        public IEnumerable<string> Roles { get; private set; }
        public IEnumerable<string> Groups { get; private set; }
        public string ProxyName { get; private set; }
        public string Name { get; private set; }
        public bool IsAdmin { get; private set; }
        public string LastLogin { get; private set; }
        public string ProxyLastLogin { get; private set; }

        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            UserGuid = GetColumn<Guid>("UserGuid");
            ProxyUserGuid = GetColumn<Guid?>("ProxyUserGuid");
            ExpireDate = GetColumn<DateTime>("ExpireDate");
            IsProxyMode = GetColumn<bool>("IsProxyMode");
            IsActive = GetColumn<bool>("IsActive");
            Roles = GetEnumerableColumn<string>("Roles");
            Groups = GetEnumerableColumn<string>("Groups");
            ProxyName = GetColumn<string>("ProxyName");
            Name = GetColumn<string>("Name");
            IsAdmin = GetColumn<bool>("IsAdmin");
            LastLogin = GetFormattedDateTimeStringColumn("LastLogin", ApplicationConfiguration.LastLoginDateFormat);
            ProxyLastLogin = GetFormattedDateTimeStringColumn("ProxyLastLogin", ApplicationConfiguration.LastLoginDateFormat);
        }
        internal static AppSession CreateEmpty()
            => new AppSession()
            {
                Guid = Guid.NewGuid(),
                UserGuid = Guid.NewGuid(),
                ProxyUserGuid = null,
                ExpireDate = DateTime.UtcNow.AddSeconds(-1),
                Roles = new string[0],
                Groups = new string[0],
                Name = ""
            };
    }
}