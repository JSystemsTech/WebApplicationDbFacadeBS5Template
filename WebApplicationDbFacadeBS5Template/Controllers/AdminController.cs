using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Attributes;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Data;
using WebApplicationDbFacadeBS5Template.DomainLayer.Models.Parameters;
using WebApplicationDbFacadeBS5Template.Extensions;
using WebApplicationDbFacadeBS5Template.Models;
using WebApplicationDbFacadeBS5Template.Models.Admin;
using WebApplicationDbFacadeBS5Template.Models.DataTable;
using WebApplicationDbFacadeBS5Template.Models.Helpers.DataTable;

namespace WebApplicationDbFacadeBS5Template.Controllers
{
    [IsAdmin]
    public class AdminController : AuthenticatedControllerBase
    {
        protected override void ResolveDataTableOptions()
        {
            RegisterDataTableOptions(
                "UserSearchOptions",
                DataTableOptions<UserSearchResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("UserSearch");
                }, columns =>
                {
                    columns.Add(m => m.Name, c => { c.Data = "Name"; c.Name = "Name"; c.Title = "Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Email, c => { c.Data = "Email"; c.Name = "Email"; c.Title = "Email"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Guid, c => {
                        c.Data = "Actions";
                        c.Name = "Actions";
                        c.Title = "Actions";
                        c.Searchable = false;
                        c.Orderable = false;
                        c.Export = false;
                        c.Render = m => GetDataTableColumnActionButtons(
                            new ActionButtonVM() { Title = "Manage Roles", Action = "manage-roles", ButtonClass = "btn-primary", Description = $"Manage {m.Name}'s Roles", Icon = "fa-user" },
                            new ActionButtonVM() { Title = "Manage Groups", Action = "manage-groups", ButtonClass = "btn-secondary", Description = $"Manage {m.Name}'s Groups", Icon = "fa-users" }
                        );
                    });
                    columns.Add(m => Url.Action("ManageUserRoles", new { userGuid = m.Guid}), c => { c.DataOnly = true; c.Data = "ManageUserRolesUrl"; });
                }));
            RegisterDataTableOptions(
                "ProxyUserSearchOptions",
                DataTableOptions<UserSearchResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("ProxyUserSearch");
                }, columns =>
                {
                    columns.Add(m => m.Name, c => { c.Data = "Name"; c.Name = "Name"; c.Title = "Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Email, c => { c.Data = "Email"; c.Name = "Email"; c.Title = "Email"; c.Filter = new DataTableOptionsFilter(); });
                    
                    columns.Add(m => m.Guid, c => {
                        c.Data = "Select";
                        c.Name = "Select";
                        c.Title = "Select User";
                        c.Searchable = false;
                        c.Orderable = false;
                        c.Export = false;
                        c.Render = m => RenderViewToString($"~/Views/Admin/SelectProxyUser.cshtml", m);
                    });
                }));
            RegisterDataTableOptions(
                "AdminUserSearchOptions",
                DataTableOptions<UserSearchResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("AdminUserSearch");
                }, columns =>
                {
                    columns.Add(m => m.Name, c => { c.Data = "Name"; c.Name = "Name"; c.Title = "Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Email, c => { c.Data = "Email"; c.Name = "Email"; c.Title = "Email"; c.Filter = new DataTableOptionsFilter(); });

                    columns.Add(m => m.Guid, c => {
                        c.Data = "Actions";
                        c.Name = "Actions";
                        c.Title = "Actions";
                        c.Searchable = false;
                        c.Orderable = false;
                        c.Export = false;
                        c.Render = m => GetDataTableColumnActionButtons(
                            new ActionButtonVM() { Title = "Remove", Action = "Remove", ButtonClass = "btn-danger", Description = $"Remove {m.Name} as an Adminisrator", Icon = "fa-user-ban" }
                        );
                    });
                }));
            RegisterDataTableOptions(
                "ApplicationUseSearchOptions",
                DataTableOptions<ApplicationUseSearchResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("ApplicationUseSearch");
                }, columns =>
                {
                    columns.Add(m => m.Name, c => { c.Data = "Name"; c.Name = "Name"; c.Title = "Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Email, c => { c.Data = "Email"; c.Name = "Email"; c.Title = "Email"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.EDIPI, c => { c.Data = "EDIPI"; c.Name = "EDIPI"; c.Title = "DoD Id"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Logins, c => { c.Data = "Logins"; c.Name = "Logins"; c.Title = "Logins"; c.Searchable = false; });
                    columns.Add(m => Math.Round(m.Average, 2), c => { c.Data = "Average"; c.Name = "Average"; c.Title = "Average Session Length (Min)"; c.Searchable = false; });
                    columns.Add(m => Math.Round(m.Min, 2), c => { c.Data = "Min"; c.Name = "Min"; c.Title = "Minimum Session Length (Min)"; c.Searchable = false; });
                    columns.Add(m => Math.Round(m.Max, 2), c => { c.Data = "Max"; c.Name = "Max"; c.Title = "Maximum Session Length (Min)"; c.Searchable = false; });
                    columns.Add(m => Math.Round(m.Total, 2), c => { c.Data = "Total"; c.Name = "Total"; c.Title = "Total Time Logged In (Min)"; c.Searchable = false; });
                }));
            RegisterDataTableOptions(
                "GetOnlineUsersOptions",
                DataTableOptions<OnlineUserResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("GetOnlineUsers");
                }, columns =>
                {
                    columns.Add(m => m.Name, c => { c.Data = "Name"; c.Name = "Name"; c.Title = "Name"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Email, c => { c.Data = "Email"; c.Name = "Email"; c.Title = "Email"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.EDIPI, c => { c.Data = "EDIPI"; c.Name = "EDIPI"; c.Title = "DoD Id"; c.Filter = new DataTableOptionsFilter(); });
                }));
            RegisterDataTableOptions(
                "GetApplicationErrorsOptions",
                DataTableOptions<ApplicationErrorResult>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "desc" } };
                    options.GetRowId = m => m.Guid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("GetApplicationErrors");
                }, columns =>
                {
                    columns.Add(m => m.CreateDateDisplay, c => { c.Data = "CreateDate"; c.Name = "CreateDate"; c.Title = "Date"; c.Filter = DataTableOptionsFilter.CreateDateFilter(); });
                    columns.Add(m => m.Type, c => { c.Data = "Type"; c.Name = "Type"; c.Title = "Type"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Message, c => { c.Data = "Message"; c.Name = "Message"; c.Title = "Message"; c.Filter = new DataTableOptionsFilter(); });
                }));
            RegisterDataTableOptions(
                "GetAnnouncmentsOptions",
                DataTableOptions<AnnouncementVM>.Create(
                options =>
                {
                    options.Order = new object[][] { new object[] { 0, "asc" }, new object[] { 1, "desc" } };
                    options.GetRowId = m => m.AnnouncementGuid.ToString();
                },
                ajax =>
                {
                    ajax.Url = Url.Action("GetAnnouncments");
                }, columns =>
                {
                    columns.Add(m => m.PriorityOrder, c => { c.Data = "PriorityCode"; c.Name = "PriorityCode"; c.Title = "Priority"; c.Render = m => m.PriorityName; c.Filter = DataTableOptionsFilter.CreateSelectFilter(AnnouncementExtensions.AnnoucmentPriorityAsSelectArray); });
                    columns.Add(m => m.ActiveDate, c => { c.Data = "ActiveDate"; c.Name = "ActiveDate"; c.Title = "Active Date"; c.Render = m => m.ActiveDate.ToDateString(); c.Filter = DataTableOptionsFilter.CreateDateFilter(); });
                    columns.Add(m => m.ExpireDate, c => { c.Data = "ExpireDate"; c.Name = "ExpireDate"; c.Title = "Expire Date"; c.Render = m => m.ExpireDate.ToDateString(); });
                    columns.Add(m => m.Title, c => { c.Data = "Title"; c.Name = "Title"; c.Title = "Title"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => m.Message, c => { c.Data = "Message"; c.Name = "Message"; c.Title = "Message"; c.Filter = new DataTableOptionsFilter(); });
                    columns.Add(m => Url.Action("AddAnnouncementForm", new { announcementGuid = m.AnnouncementGuid }), c => { c.DataOnly = true; c.Data = "CopyAnnouncementUrl"; });
                    columns.Add(m => Url.Action("EditAnnouncementForm", new { announcementGuid = m.AnnouncementGuid }), c => { c.DataOnly = true; c.Data = "EditAnnouncementUrl"; });
                    columns.Add(m => "Image", c => {
                        c.Data = "Image";
                        c.Name = "Image";
                        c.Title = "Image";
                        c.Searchable = false;
                        c.Orderable = false;
                        c.Export = false;
                        c.Render = m => GetDataTableColumnImage(m.Title, m.ImageFileGuid, true);
                    });
                    columns.Add(m => "Actions", c => {
                        c.Data = "Actions";
                        c.Name = "Actions";
                        c.Title = "Actions";
                        c.Searchable = false;
                        c.Orderable = false;
                        c.Export = false;
                        c.Render = m => GetDataTableColumnActionButtons(
                            new ActionButtonVM() { Title = "Edit", Action = "Edit", ButtonClass = "btn-primary", Description = $"Edit Announcement", Icon = "fa-edit" },
                            new ActionButtonVM() { Title = "Copy", Action = "Copy", ButtonClass = "btn-secondary", Description = $"Copy Announcement", Icon = "fa-copy" }
                        );
                    });
                }));
        }
        public ActionResult Index()
        {
            //AddError(new Exception("this is a test error"));
            return View(AdminDashboard.Create());
        }
       // => View(AdminDashboard.Create());

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateProxyRequestToken]
        public ActionResult BeginProxy()
        => RedirectToAction("Index","Home");

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ProxyUserSearch(DataTableRequest<UserSearchParameters> request)
        {
            request.Parameters.UserGuid = ApplicationUser.UserGuid;
            var data = AppDomainFacade.ProxyUserSearch(request.Parameters);
            return DataTableResult(data, request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AdminUserSearch(DataTableRequest<UserSearchParameters> request)
        {
            request.Parameters.UserGuid = ApplicationUser.UserGuid;
            var data = AppDomainFacade.AdminUserSearch(request.Parameters);
            return DataTableResult(data, request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApplicationUseSearch(DataTableRequest<UserSearchParameters> request)
        {
            var data = AppDomainFacade.ApplicationUseSearch(request.Parameters);
            return DataTableResult(data, request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetOnlineUsers(DataTableRequest request)
        {
            var data = AppDomainFacade.GetOnlineUsers();
            return DataTableResult(data, request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetApplicationUseMetrics(string code, int? lastNDays)
        {
            var data = AppDomainFacade.GetApplicationUseMetrics(code, lastNDays).OrderBy(m=> m.Value);
            return Json(new { 
                labels = data.Select(m=>m.Label), 
                data = data.Select(m => m.Count),
                xLabel = code == "M" ? "Month" : code == "D" ? "Day": code == "W" ? "Weekday": "Hour",
                yLabel = "Sessions",
                title = "Application Use",
                yTick = "int"
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetApplicationUseByDayMetrics(string code, int? lastNDays)
        {
            var data = AppDomainFacade.GetApplicationUseByDayMetrics(lastNDays).OrderBy(m => m.Date);
            var selectedDataSet = code == "AVG" ? data.Select(m => (object)m.Average) :
                code == "MIN" ? data.Select(m => (object)m.Min) :
                code == "MAX" ? data.Select(m => (object)m.Max) :
                code == "TOTAL" ? data.Select(m => (object)m.Total) :
                code == "LOGINS" ? data.Select(m => (object)m.Logins) :
                data.Select(m => (object)m.Users);

            var yLabel = code == "AVG" ? "Average Session Length" :
                code == "MIN" ? "Minimum Session Length" :
                code == "MAX" ? "Maximum Session Length" :
                code == "TOTAL" ? "Total Time Logged In" :
                code == "LOGINS" ? "Number of Logins" :
                "Number of Unique Users";

            return Json(new
            {
                labels = data.Select(m => m.Label),
                data = selectedDataSet,
                xLabel = "Date",
                yLabel = yLabel,
                title = "Application Use By Day",
                yTick = code == "USERS" || code == "LOGINS" ? "int" : null
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetApplicationUserCount()
        {
            var response = AppDomainFacade.GetApplicationUserCount(out ApplicationUserCount result);
            return Json(new {
                title = "Application Users",
                data = new int[] { result.Offline, result.Online },
                labels = new string[] { "Offline", "Online" },
                backgroundColor = new string[] { "secondary", "primary" }
            });
        }
        [AjaxOnly]
        public PartialViewResult ViewOnlineUsers()
        => PartialView();


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetApplicationErrorMetrics(int? lastNDays)
        {
            var data = AppDomainFacade.GetApplicationErrorMetrics(lastNDays).OrderBy(m => m.Date);
            return Json(new
            {
                labels = data.Select(m => m.Label),
                data = data.Select(m => m.Count),
                xLabel = "Date",
                yLabel = "Count",
                title = "Application Errors",
                yTick = "int"
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetApplicationErrors(DataTableRequest<ApplicationErrorResult> request)
        {
            var data = AppDomainFacade.GetApplicationErrors();
            return DataTableResult(data, request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult UserSearch(DataTableRequest<UserSearchParameters> request)
        {
            request.Parameters.UserGuid = ApplicationUser.UserGuid;
            var data = AppDomainFacade.UserSearch(request.Parameters);
            return DataTableResult(data, request);
        }
        [AjaxOnly]
        public PartialViewResult ManageUserRoles(Guid userGuid)
        {
            return ListManagmentView(Url.Action("UpdateUserRoles", new { userGuid }), AppDomainFacade.GetUserRoleLookup(userGuid), m=>m.Name,m=> m.Description, m=>m.Guid, m=>m.IsActive);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public JsonResult UpdateUserRoles(ListManagementRequestItem<Guid>[] items, Guid userGuid)
        {
            var data = AppDomainFacade.GetUserRoleLookup(userGuid);
            foreach(var change in items.Where(i=> data.Any(m=> m.Guid == i.Value && m.IsActive != i.Selected)))
            {
                
            }
            return Json(new { }, Notification.Success("Complete Update User Roles", $"Successfully updated user roles."));
        }

        [AjaxOnly]
        public PartialViewResult PreviewAnnouncements()
        {
            var response = AppDomainFacade.GetAppAnnuncements();
            return PartialView("AnnouncementCarousel", response.ToAnnouncmentList());
        }
        [AjaxOnly]
        public PartialViewResult AddAnnouncementForm(Guid? announcementGuid = null)
        => TryGetPartialView(() => {
            if(announcementGuid is Guid guid)
            {
                var response = AppDomainFacade.GetAppAnnuncement(guid, out AppAnnouncement result);
                if (response.HasError)
                {
                    return ErrorPartialView(response.Error);
                }
                return PartialView("AnnouncementForm", AnnouncementVM.Create(result));
            }
            return PartialView("AnnouncementForm", AnnouncementVM.Create());
        });
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public JsonResult AddAnnouncement(AnnouncementVM vm)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelStateJson("Unable to add Annoucement");
            }
            var response = AppDomainFacade.AddAppAnnuncement(ApplicationUser.ActingUserGuid, vm.ToAddParameters(), out AppAnnouncement result);
            if (response.HasError)
            {
                return Json(new { }, Notification.Error("Error while trying to add Annoucement", response.Error));
            }
            return Json(new { }, Notification.Success("Complete Add Announcement", $"Successfully added Announcement."));
        }
        
        [AjaxOnly]
        public PartialViewResult EditAnnouncementForm(Guid announcementGuid)
        => TryGetPartialView(() => {
            var response = AppDomainFacade.GetAppAnnuncement(announcementGuid, out AppAnnouncement result);
            if (response.HasError)
            {
                return ErrorPartialView(response.Error);
            }
            var vm = AnnouncementVM.Create(result, "EditAnnouncement");
            return PartialView("AnnouncementForm", vm);
        });
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public JsonResult EditAnnouncement(AnnouncementVM vm)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelStateJson("Unable to edit Annoucement");
            }
            var response = AppDomainFacade.UpdateAppAnnuncement(ApplicationUser.ActingUserGuid, vm.ToUpdateParameters(), out AppAnnouncement result);
            if (response.HasError)
            {
                return Json(new { }, Notification.Error("Error while trying to edit Annoucement", response.Error));
            }
            return Json(new { }, Notification.Success("Complete Edit Announcement", $"Successfully edited Announcement."));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GetAnnouncments(DataTableRequest<AnnouncementVM> request)
        {
            var response = AppDomainFacade.GetAppAnnuncements();
            if (response.HasError)
            {
                return Json(new { }, Notification.Error("Error while trying to get Annoucements", response.Error));
            }
            return DataTableResult(response.ToAnnouncmentList(), request);
        }
        
    }
}