using AdminFrontend.Auth;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize]
    public class HomeController : RCPController
    {
        public ActionResult Index()
        {
            if (UserRole == "Admin")
                return RedirectToAction("Admin");
            else if (UserRole == "Teacher")
                return RedirectToAction("Teacher");
            else
                return new HttpUnauthorizedResult();

        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Teacher()
        {
            return View();
        }
    }
}