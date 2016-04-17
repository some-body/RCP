using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using Distributed;
using Domain.Dto;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Teacher")]
    public class CoursesController : RCPController
    {
        private ApiQueryProvider<ICollection<CourseDto>, Course> _courseQueryProvider;

        public CoursesController()
        {
            var backendUrl = Properties.Resources.PreparationBackendURL;
            _courseQueryProvider = new ApiQueryProvider<ICollection<CourseDto>, Course>(backendUrl, "Courses");
        }

        // GET: Courses
        public ActionResult Index()
        {
            var courses = _courseQueryProvider.Get();

            var tableData = new TableViewModel
            {
                ColumnsNames = new string[] { "Название", "Описание" },
                Rows = courses.Select(c => new Row
                {
                    Id = c.Id,
                    Columns = new string[] { c.Name, c.Description }
                }).ToList(),
                EditAction = "/Courses/Edit/",
                DeleteAction = "Courses/Delete/",
                AddAction = "Courses/Add"
            };

            var model = new CoursesViewModel
            {
                TableData = tableData
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Редактирование курса";
            return View(id);
        }

        [HttpPost]
        public JsonResult GetCourse(int id)
        {
            var course = id != -1
                ? _courseQueryProvider.Get(id)
                : new Course
                {
                    Id = -1,
                    Name = "",
                    Description = "",
                    MaterialText = "",
                    Questions = new List<Question>()
                };

            return Json(course);
        }

        public ActionResult Add()
        {
            ViewBag.Title = "Создание курса";
            return View("Edit", -1);
        }

        [HttpPost]
        public JsonResult Save(Course model)
        {
            if (model.Id.HasValue && model.Id.Value == -1)
                model.Id = null;

            if (!ModelState.IsValid)
                return Json("Проверьте правильность заполненных полей");

            var result = _courseQueryProvider.Post(model);
            var msg = result.Success
                ? "Успех"
                : result.Message;

            return Json(new
            {
                returnTo = Url.Action("Index"),
                success = result.Success,
                msg
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _courseQueryProvider.Delete(id);

            var msg = result.Success
                ? "Успех"
                : result.Message;

            return Json(new
            {
                success = result.Success,
                msg
            });
        }
    }
}