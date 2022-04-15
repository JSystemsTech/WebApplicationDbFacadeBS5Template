using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Attributes;

namespace WebApplicationDbFacadeBS5Template.Controllers
{
    public class AuthController : ControllerBase
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        [FederatedAuthenticationFilter]
        public ActionResult Login()
        {
            return RedirectToAction("Index","Home");
        }
    }
}