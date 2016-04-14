using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Admin")]
    public class TeachersController : Controller
    {
        public ActionResult Index()
        {
            // TODO: Реализовать.
            var model = new TeachersViewModel();
            return View(model);
        }
    }
}