using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Teacher")]
    public class ExamResultsController : RCPController
    {
        public ActionResult Index()
        {
            // TODO: Реализовать.
            var model = new ExamResultsViewModel();
            return View(model);
        }
    }
}