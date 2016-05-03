using System;
using System.Web.Mvc;

namespace AdminFrontend.Logging
{
    public class RCPLoggingAttribute : ActionFilterAttribute
    {
        public readonly string Folder = "/log/";
        public readonly string FileName = "app.log";

        public static readonly bool Stop = true;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Stop)
                return;

            var fullname = string.Concat(Folder, FileName);

            var url = filterContext.RequestContext.HttpContext.Request.Url;
            var ip = filterContext.RequestContext.HttpContext.Request.UserHostAddress;

            var message = $"[{DateTime.Now.ToString()}], | {ip} - {url}";

            System.IO.File.AppendAllText(fullname, message);
        }
    }
}