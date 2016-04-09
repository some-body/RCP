using System;
using System.Security.Principal;
using System.Web;
using Domain.Entities;
using System.Web.Security;

namespace AdminFrontend.Auth
{
    public class CustomAuthentication : IAuthentication
    {
        private const string cookieName = "__AUTH_COOKIE";

        private IPrincipal _currentUser;

        public IPrincipal CurrentUser
        {
            get
            {
                throw new NotImplementedException();
                //if (_currentUser == null)
                //{
                //    try
                //    {
                //        HttpCookie authCookie = HttpContext.Request.Cookies.Get(cookieName);
                //        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                //        {
                //            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                //            _currentUser = new UserProvider(ticket.Name, Repository);
                //        }
                //        else
                //        {
                //            _currentUser = new UserProvider(null, null);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        _currentUser = new UserProvider(null, null);
                //    }
                //}
                //return _currentUser;
            }
        }

        public HttpContext HttpContext { get; set; }

        public SystemUser Login(string userName, string Password, bool isPersistent)
        {
            throw new NotImplementedException();
            //User retUser = Repository.Login(userName, Password);
            //if (retUser != null)
            //{
            //    CreateCookie(userName, isPersistent);
            //}
            //return retUser;
        }

        public SystemUser Login(string userName)
        {
            throw new NotImplementedException();
            //User retUser = Repository.Users.FirstOrDefault(p => string.Compare(p.Email, userName, true) == 0);
            //if (retUser != null)
            //{
            //    CreateCookie(userName);
            //}
            //return retUser;
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[cookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                  1,
                  userName,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  string.Empty,
                  FormsAuthentication.FormsCookiePath);

            // Encrypt the ticket.
            var encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            var AuthCookie = new HttpCookie(cookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext.Response.Cookies.Set(AuthCookie);
        }
    }
}