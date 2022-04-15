using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public abstract class LookupItem : DbDataModel
    {
        public Guid Guid { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            Code = GetColumn<string>("Code");
            Name = GetColumn<string>("Name");
            Description = GetColumn<string>("Description");
            IsActive = GetColumn<bool>("IsActive");
        }
    }
    public class UserRole : LookupItem { }
    public class UserGroup : LookupItem { }
}