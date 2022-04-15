using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDbFacadeBS5Template.Models;

namespace WebApplicationDbFacadeBS5Template.Controllers
{
    public class ModalController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult Modal(Modal vm)
        {
            return PartialView("Modal", vm);
        }
    }
}