using Distributed;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkersFrontend.Controllers
{
    public class CoursesController : Controller
    {
        private ApiQueryProvider<ICollection<Course>, Course> _courseQueryProvider;

        public CoursesController()
        {
            var backendUrl = Properties.Resources.CoursesBackendURL;
            _courseQueryProvider = new ApiQueryProvider<ICollection<Course>, Course>(backendUrl, "Courses");
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = _courseQueryProvider.Get();
            // TODO: ViewModel-ку.
            return View();
        }
    }
}