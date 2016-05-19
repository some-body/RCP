using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    public class DefaultController : Controller
    {
        [HttpPost]
        public JsonResult Test()
        {
            Thread.Sleep(1000);
            return Json(new Random().Next(), JsonRequestBehavior.AllowGet);
        }
    }
}