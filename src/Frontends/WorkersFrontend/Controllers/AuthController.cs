using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersFrontend.ViewModels;

namespace WorkersFrontend.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult SignIn(RedirectViewModel model)
        {
            return View(model);
        }

        public ActionResult Authorize(AuthViewModel model)
        {

        }
    }
}