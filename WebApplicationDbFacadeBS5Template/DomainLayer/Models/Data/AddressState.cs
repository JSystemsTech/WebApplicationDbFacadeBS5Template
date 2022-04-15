using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class AddressState: DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Group { get; private set; }
        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Code = GetColumn<string>("Code");
            Name = GetColumn<string>("Name");
            Group = GetColumn<string>("Group");
        }
    }
}