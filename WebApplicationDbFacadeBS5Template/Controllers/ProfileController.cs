using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Attributes;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.Models;
using WebApplicationDbFacadeBS5Template.Models.Profile;

namespace WebApplicationDbFacadeBS5Template.Controllers
{
    public class ProfileController : AuthenticatedControllerBase
    {
        protected override void ResolveDataTableOptions()
        {
            
        }
        protected override void OnActionExecutingCustom(ActionExecutingContext filterContext)
        {
            
        }
        private IEnumerable<NavTabVM> GetProfileNavTabs(Guid userGuid, bool edit = false) => new NavTabVM[] {
            new NavTabVM("Dashboard", Url.Action("ProfileSection_Dashboard", new{ userGuid, edit }),true),
            new NavTabVM("About", Url.Action("ProfileSection_About", new{ userGuid, edit })),
            new NavTabVM("Details", Url.Action("ProfileSection_Details", new{ userGuid, edit }))
        };
        private void SetViewEditMode(Guid userGuid, bool edit = false)
        {
            ViewBag.Edit = ApplicationUser.CanEditUserProfile(userGuid) && edit;
        }
        [AjaxOnly]
        public PartialViewResult ProfileSection_Dashboard(Guid userGuid, bool edit = false)
        => TryGetPartialView(() => {
            if (!ApplicationUser.CanViewUserProfile(userGuid))
            {
                return UnauthorizedPartialView("Your are not authorized to view this user profile");
            }
            SetViewEditMode(userGuid, edit);
            return PartialView("ProfileSection_Dashboard");
        });
        [AjaxOnly]
        public PartialViewResult ProfileSection_About(Guid userGuid, bool edit = false)
        => TryGetPartialView(() => {
            if (!ApplicationUser.CanViewUserProfile(userGuid))
            {
                return UnauthorizedPartialView("Your are not authorized to view this user profile");
            }
            SetViewEditMode(userGuid, edit);
            return PartialView("ProfileSection_About", new UserProfileAboutVM() { UserGuid = userGuid});
        });
        [AjaxOnly]
        public PartialViewResult ProfileSection_Details(Guid userGuid, bool edit = false)
        => TryGetPartialView(() => {
            if (!ApplicationUser.CanViewUserProfile(userGuid))
            {
                return UnauthorizedPartialView("Your are not authorized to view this user profile");
            }
            SetViewEditMode(userGuid, edit);
            return PartialView("ProfileSection_Details");
        });
        // GET: Profile
        public ActionResult Index(Guid userGuid, bool edit = false)
        {
            if (!ApplicationUser.CanViewUserProfile(userGuid))
            {
                return RedirectToUnauthorized("Your are not authorized to view this user profile");
            }
            var response = AppDomainFacade.GetUserSettings(ApplicationUser.ActingUserGuid, out UserSettings userSettings);
            if (response.HasError)
            {
                return ErrorPartialView(response.Error);
            }
            SetViewEditMode(userGuid, edit);
            return View(new UserProfileVM() { UserGuid = ApplicationUser.ActingUserGuid, ImageFileGuid = userSettings.ProfilePictureFileGuid,Tabs = GetProfileNavTabs(ApplicationUser.ActingUserGuid, edit) });
        }
        [AjaxOnly]
        public PartialViewResult ProfileNameForm(Guid userGuid)
        => TryGetPartialView(() => {
            var response = AppDomainFacade.GetUserSettings(userGuid, out UserSettings userSettings);
            if (response.HasError)
            {
                return ErrorPartialView(response.Error);
            }
            var vm = new UserProfileNameVM() { UserGuid = userGuid, FirstName="Test", LastName="User", ImageFileGuid = userSettings.ProfilePictureFileGuid };
            return PartialView("ProfileNameForm", vm);
        });
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public JsonResult UpdateName(UserProfileNameVM vm)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelStateJson("Unable to edit name");
            }
            //var response = AppDomainFacade.UpdateAppAnnuncement(vm.ToUpdateParameters(), out AppAnnouncement result);
            //if (response.HasError)
            //{
            //    return Json(new { }, Notification.Error("Error while trying to edit Annoucement", response.Error));
            //}
            if(vm.Image != null)
            {
                var response = AppDomainFacade.UpdateUserProfilePicture(vm.UserGuid, vm.Image, out UserSettings result);
                if (response.HasError)
                {
                    return Json(new { }, Notification.Error("Error Edit Name", response.Error));
                }
            }
            
            return Json(new { }, Notification.Success("Complete Edit Name", $"Successfully edited Name."));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public JsonResult UpdateAbout(UserProfileAboutVM vm)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelStateJson("Unable to edit About section");
            }
            //call db here
            

            return Json(new { }, Notification.Success("Complete Edit About Section", $"Successfully edited About Section."));
        }
    }
}