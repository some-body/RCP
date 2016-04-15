﻿using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Teacher")]
    public class CoursesController : RCPController
    {
        // GET: Courses
        public ActionResult Index()
        {
            // TODO: Реализовать.
            var model = new CoursesViewModel();
            return View(model);
        }
    }
}