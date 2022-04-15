using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Attributes;
using WebApplicationDbFacadeBS5Template.DomainLayer;
using WebApplicationDbFacadeBS5Template.Models;
using WebApplicationDbFacadeBS5Template.Models.Admin;
using WebApplicationDbFacadeBS5Template.Models.DataTable;
using WebApplicationDbFacadeBS5Template.Models.Helpers.DataTable;

namespace WebApplicationDbFacadeBS5Template.Controllers
{    
    public class HomeController : AuthenticatedControllerBase
    {
        protected override void ResolveDataTableOptions() {
            
        }
        public ActionResult Index()
        {
            var announcments = AppDomainFacade.GetAppAnnuncements();
            ViewBag.Announcments = announcments.ToAnnouncmentList();
            return View();
        }
        public ActionResult Sandbox()
        {
            return View();
        }
    }
}