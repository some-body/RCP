using AdminFrontend.Controllers;
using Distributed;
using Domain.Dto;
using Domain.Entities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Auth
{
    public class RCPAuthorizeAttribute : ActionFilterAttribute
    {
        private CustomApiQueryProvider _sessionQueryProvider;

        public string Roles { get; set; } = "Admin, Teacher";

        public RCPAuthorizeAttribute()
        {
            var backendUrl = Properties.Resources.SessionBackendURL;
            _sessionQueryProvider = new CustomApiQueryProvider(backendUrl);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rolesArr = Roles.Split(',')
                .Select(r => r.Trim())
                .ToList();
            

            var tokenValue = filterContext.HttpContext.Request.Cookies["token"];

            if (tokenValue == null)
            {
                RedirectToAuth(filterContext);
                return;
            }

            var user = _sessionQueryProvider.Post<SystemUser, string>("api/SystemUsers/GetByToken", tokenValue.Value);
            if(user == null || !rolesArr.Contains(user.Role))
            {
                filterContext.HttpContext.Response.Cookies[tokenValue.Name].Expires = DateTime.Now.AddDays(-1);
                RedirectToAuth(filterContext);
                return;
            }
            else
            {
                var contr = (RCPController)filterContext.Controller;
                contr.UserId = user.Id.Value;
                contr.UserRole = user.Role;
            }
        }

        private void RedirectToAuth(ActionExecutingContext filterContext)
        {
            var redirectParam = "ReturnToUrl=" + filterContext.HttpContext.Request.Url;
            //var errorParam = "error=" + HttpUtility.UrlEncode("Данное действие требует авторизации");
            var authUrl = "/Auth/SignIn?" + redirectParam;// + "&" + errorParam;
            filterContext.Result = new RedirectResult(authUrl);

            //filterContext.Result = new RedirectToRouteResult()
        }
    }
}