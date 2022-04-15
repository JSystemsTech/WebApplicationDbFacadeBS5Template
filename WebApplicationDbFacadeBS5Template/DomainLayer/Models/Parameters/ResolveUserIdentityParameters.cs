using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters
{
    public class ResolveUserIdentityParameters
    {
        public string EDIDI { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}