using Distributed;
using Domain.Dto;
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
        private CustomApiQueryProvider _sessionQueryProvider;

        public AuthController()
        {
            var backendUrl = Properties.Resources.SessionBackendURL;
            _sessionQueryProvider = new CustomApiQueryProvider(backendUrl);
        }

        // GET: Auth
        public ActionResult SignIn(RedirectViewModel model)
        {
            var authViewModel = new AuthViewModel
            {
                ReturnToUrl = model.ReturnToUrl
            };

            return View(authViewModel);
        }

        public ActionResult Authorize(AuthViewModel model)
        {
            var redirectUrl = HttpUtility.UrlDecode(model.ReturnToUrl);

            var loginDto = new LoginDto
            {
                Login = model.Login,
                Password = model.Password
            };

            var result = _sessionQueryProvider.Post<SignInDto, LoginDto>("api/Workers/SignIn", loginDto);
            if(result == null)
            {
                return RedirectToAction("SignIn", new RedirectViewModel
                {
                    ReturnToUrl = redirectUrl
                });
            }
            else
            {
                var tokenCookie = new HttpCookie("token")
                {
                    Value = result.Token.Value,
                    Expires = result.Token.ExpiresOn
                };
                Response.SetCookie(tokenCookie);
                return Redirect(redirectUrl);
            }
        }
    }
}