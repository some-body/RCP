using Distributed;
using Domain.Dto;
using System.Web;
using System.Web.Mvc;
using AdminFrontend.ViewModels;

namespace AdminFrontend.Controllers
{
    public class AuthController : Controller
    {
        private CustomApiQueryProvider _sessionQueryProvider;

        public AuthController()
        {
            var backendUrl = Properties.Resources.SessionBackendURL;
            _sessionQueryProvider = new CustomApiQueryProvider(backendUrl);
        }

        [HttpGet]
        public ActionResult Index(RedirectViewModel model)
        {
            var authViewModel = new AuthViewModel
            {
                ReturnToUrl = model.ReturnToUrl
            };

            return View(authViewModel);
        }

        [HttpPost]
        public ActionResult Index1(AuthViewModel model)
        {
            var redirectUrl = HttpUtility.UrlDecode(model.ReturnToUrl);
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Password))
            {
                return RedirectToAction("Index", new RedirectViewModel
                {
                    ReturnToUrl = redirectUrl
                });
            }

            var loginDto = new LoginDto
            {
                Login = model.Login,
                Password = model.Password
            };

            var result = _sessionQueryProvider.Post<WorkerSignInDto, LoginDto>("api/SystemUsers/SignIn", loginDto);
            if(result == null)
            {
                var url = "/Auth/Index?ReturnToUrl=" + redirectUrl;
                return Redirect(url);
                //return RedirectToAction("Index", "Auth", new RedirectViewModel
                //{
                //    ReturnToUrl = redirectUrl
                //});
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

        [HttpGet]
        public ActionResult SignOut()
        {
            var tokenCookie = Request.Cookies["token"];
            if (tokenCookie != null)
            {
                Response.Cookies.Remove("token");
                _sessionQueryProvider.Post<int?, string>("api/SystemUsers/SignOut", tokenCookie.Value);
            }
            return RedirectToAction("Index", "Home", null);
        }
    }
}