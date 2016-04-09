using AdminFrontend.ViewModels;
using RCPAuthorization;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace AdminFrontend.Controllers
{
    public class AuthController : Controller
    {
        private SystemUserAuthProvider _systemUserAuthProvider;

        public AuthController()
        {
            _systemUserAuthProvider = new SystemUserAuthProvider();
        }

        [HttpGet]
        public ActionResult LogOn(string ReturnUrl)
        {
            return View(new LogOnViewModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        public ActionResult LogOn(LogOnViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = model.ReturnUrl;
                var user = _systemUserAuthProvider.GetUser(model.Login, model.Password);
                if (user != null)
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

            return View(model);
        }
    }
}