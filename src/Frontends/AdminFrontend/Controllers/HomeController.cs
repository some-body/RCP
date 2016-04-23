using AdminFrontend.Auth;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    public class HomeController : RCPController
    {
        [RCPAuthorize]
        public ActionResult Index()
        {
            if (UserRole == "Admin")
                return Admin();
            else if (UserRole == "Teacher")
                return Teacher();
            else
                return new HttpUnauthorizedResult();
        }

        public ActionResult About()
        {
            return View();
        }

        private ActionResult Admin()
        {
            return View("Admin");
        }

        private ActionResult Teacher()
        {
            return View("Teacher");
        }
    }
}