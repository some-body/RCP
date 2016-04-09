using AdminFrontend.ViewModels;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace AdminFrontend.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult LogOn()
        {
            return View(new LogOnViewModel());
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                throw new NotImplementedException();
                // TODO: Заменить на свою проверку.
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}