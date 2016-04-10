using Distributed;
using Domain.Dto;
using Domain.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using WorkersFrontend.ViewModels;

namespace WorkersFrontend.Controllers
{
    public class PreparationController : Controller
    {
        private ApiQueryProvider<ICollection<CourseDto>, Course> _courseQueryProvider;

        public PreparationController()
        {
            var backendUrl = Properties.Resources.PreparationBackendURL;
            _courseQueryProvider = new ApiQueryProvider<ICollection<CourseDto>, Course>(backendUrl, "Courses");
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = _courseQueryProvider.Get();
            return View(new CoursesViewModel
            {
                Courses = courses
            });
        }

        public ActionResult Course(int id)
        {
            var course = _courseQueryProvider.Get(id);
            var model = new CoursePreparationViewModel
            {
                Name = course.Name,
                Description = course.Description,
                MaterialText = course.MaterialText
            };

            return View(model);
        }
    }
}