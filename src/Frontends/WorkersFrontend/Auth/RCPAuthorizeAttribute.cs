using Distributed;
using Domain.Dto;
using System;
using System.Web.Mvc;
using WorkersFrontend.Controllers;

namespace WorkersFrontend.Auth
{
    public class RCPAuthorizeAttribute : ActionFilterAttribute
    {
        private CustomApiQueryProvider _sessionQueryProvider;

        public RCPAuthorizeAttribute()
        {
            var backendUrl = Properties.Resources.SessionBackendURL;
            _sessionQueryProvider = new CustomApiQueryProvider(backendUrl);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var tokenValue = filterContext.HttpContext.Request.Cookies["token"];

            if (tokenValue == null)
            {
                RedirectToAuth(filterContext);
                return;
            }

            var worker = _sessionQueryProvider.Post<WorkerDto, string>("api/Workers/GetByToken", tokenValue.Value);
            if(worker == null)
            {
                filterContext.HttpContext.Response.Cookies[tokenValue.Name].Expires = DateTime.Now.AddDays(-1);
                RedirectToAuth(filterContext);
                return;
            }
            else
            {
                ((RCPController)filterContext.Controller).UserId = worker.Id;
            }
        }

        private void RedirectToAuth(ActionExecutingContext filterContext)
        {
            var authUrl = "/Auth/SignIn?ReturnToUrl=" + filterContext.HttpContext.Request.Url;
            filterContext.Result = new RedirectResult(authUrl);
        }
    }
}