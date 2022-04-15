using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class UserSettings:DbDataModel
    {
        public Guid Guid { get; private set; }
        public bool DarkMode { get; private set;}
        public Guid? ProfilePictureFileGuid { get; private set; }
        protected override void Init()
        {
            Guid = GetColumn<Guid>("Guid");
            DarkMode = GetColumn<bool>("DarkMode");
            ProfilePictureFileGuid = GetColumn<Guid?>("ProfilePictureFileGuid");
        }
    }

}