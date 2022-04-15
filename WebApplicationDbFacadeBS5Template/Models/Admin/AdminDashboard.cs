using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;

namespace WebApplicationDbFacadeBS5Template.Models.Admin
{
    public class AdminDashboard
    {
        public UserSearchParameters UserSearchParameters { get; set; }
        internal static AdminDashboard Create()
        => new AdminDashboard()
        {
            UserSearchParameters = new UserSearchParameters()
        };
    }
}