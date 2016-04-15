using Distributed;
using Domain.Dto;
using System.Web;
using System.Web.Mvc;
using WorkersFrontend.Auth;
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

            var loginDto = new LoginDto
            {
                Login = model.Login,
                Password = model.Password
            };

            var result = _sessionQueryProvider.Post<WorkerSignInDto, LoginDto>("api/Workers/SignIn", loginDto);
            if(result == null)
            {
                return RedirectToAction("Index", new RedirectViewModel
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

        [HttpGet]
        public ActionResult SignOut()
        {
            var tokenCookie = Request.Cookies["token"];
            if (tokenCookie != null)
            {
                Response.Cookies.Remove("token");
                _sessionQueryProvider.Post<int?, string>("api/Workers/SignOut", tokenCookie.Value);
            }
            return RedirectToAction("Index", "Home", null);
        }
    }
}