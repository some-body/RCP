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
                return Admin();
            else if (UserRole == "Teacher")
                return Teacher();
            else
                return new HttpUnauthorizedResult();
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