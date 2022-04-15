using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.Extensions.Html;

namespace WebApplicationDbFacadeBS5Template.Models.Profile
{
    public class UserProfileVM
    {
        public Guid UserGuid { get; set; }
        public string Name { get; set; }
        public Guid? ImageFileGuid { get; set; }
        public IEnumerable<NavTabVM> Tabs { get; set; }
    }
    public class UserProfileNameVM
    {
        public Guid UserGuid { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Middle Initial")]
        [MaxLength(1)]
        public string MiddleInitial { get; set; }

        public Guid? ImageFileGuid { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase Image { get; set; }
    }
    public class UserProfileAboutVM
    {
        public Guid UserGuid { get; set; }
        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string HomeAddressStreet { get; set; }
        [Display(Name = "Street 2")]
        public string HomeAddressStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public string HomeAddressCity { get; set; }
        [Required]
        [Display(Name = "State")]
        public Guid StateGuid { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string HomeAddressCountry { get; set; }
        [Required]
        [Display(Name = "ZIP")]
        public string HomeAddressZIP { get; set; }

        private AddressState SelectedState => AppDomainFacade.GetAddressStateLookupList().FirstOrDefault(m => m.Guid == StateGuid);
        public string StateName => SelectedState != null ? SelectedState.Name : "";
        public SelectList<AddressState> StateSelectList
            => AppDomainFacade.GetAddressStateLookupList().ToSelectList(m => new {data_tokens= $"{m.Code}", data_subtext=m.Code }, m => m.Guid, m => m.Name, StateGuid, m => m.Group);

    }
}