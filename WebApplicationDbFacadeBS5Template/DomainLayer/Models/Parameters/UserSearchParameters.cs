using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters
{
    public class UserSearchParameters
    {
        public Guid UserGuid { get; internal set; }
		[Display(Name ="First Name")]
        public string FirstName { get; set; }
		[Display(Name = "Middle Initial")]
		[StringLength(1, MinimumLength = 1)]
		public string MiddleInitial { get; set; }

		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		[Display(Name = "Email")]
		[StringLength(int.MaxValue, MinimumLength = 256)]
		[DataType(DataType.EmailAddress)]
		[EmailAddress]
		public string Email { get; set; }
		[Display(Name = "DoD Id")]
		[StringLength(10, MinimumLength = 10)]
		public string EDIPI { get; set; }
		public bool ExcludeAdmin { get; internal set; }
		public bool OnlyAdmin { get; internal set; }

		public int? LastNDays { get; set; }
		public string OptionsUrl { get; set; }
	}
}