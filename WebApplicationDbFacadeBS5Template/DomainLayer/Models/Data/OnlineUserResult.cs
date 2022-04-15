using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class OnlineUserResult : DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string EDIPI { get; private set; }

        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Name = GetColumn<string>("Name");
            Email = GetColumn<string>("Email");
            EDIPI = GetColumn<string>("EDIPI");
        }
    }
}