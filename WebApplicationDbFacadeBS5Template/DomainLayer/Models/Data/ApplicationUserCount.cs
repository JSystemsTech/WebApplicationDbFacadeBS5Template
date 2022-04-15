using DbFacade.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data
{
    public class ApplicationUserCount:DbDataModel
    {
        public int Count { get; private set; }
        public int Online { get; private set; }
        public int Offline => Count - Online;
        protected override void Init()
        {
            Count = GetColumn<int>("Count");
            Online = GetColumn<int>("Online");
        }
    }
}