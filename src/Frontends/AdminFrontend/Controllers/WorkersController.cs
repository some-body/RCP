using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize]
    public class WorkersController : RCPController
    {
        public ActionResult Index()
        {
            if (UserRole == "Admin")
                return Admin();
            else if (UserRole == "Teacher")
                return Teacher();
            else
                return new HttpUnauthorizedResult();
        }

        private ActionResult Admin()
        {
            // TODO: Реализовать.
            var model = new WorkersForAdminViewModel();
            return View("Admin", model);
        }

        private ActionResult Teacher()
        {
            // TODO: Реализовать.
            var model = new WorkersForTeacherViewModel();
            return View("Teacher", model);
        }
    }
}