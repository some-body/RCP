using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using Distributed;
using Domain.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize]
    public class WorkersController : RCPController
    {
        private ApiQueryProvider<ICollection<WorkerDto>, Worker> _workersQueryProvider;
        private ApiQueryProvider<ICollection<CourseDto>, Course> _coursesQueryProvider;
        private CustomApiQueryProvider _examQueryProvider;
        const string PasswordPlaceholder = "***";

        public WorkersController()
        {
            var workersBackendUrl = Properties.Resources.WorkersBackendURL;
            _workersQueryProvider = new ApiQueryProvider<ICollection<WorkerDto>, Worker>(workersBackendUrl, "Workers");

            var preparationBackendUrl = Properties.Resources.PreparationBackendURL;
            _coursesQueryProvider = new ApiQueryProvider<ICollection<CourseDto>, Course>(preparationBackendUrl, "Courses");

            var examBackendUrl = Properties.Resources.ExamBackendURL;
            _examQueryProvider = new CustomApiQueryProvider(examBackendUrl);
        }

        public ActionResult Index()
        {
            if (UserRole == "Admin")
                return Admin();
            else if (UserRole == "Teacher")
                return Teacher();
            else
                return new HttpUnauthorizedResult();
        }

        public ActionResult Edit(int id)
        {
            if (UserRole == "Admin")
                return EditByAdmin(id);
            else if (UserRole == "Teacher")
                return EditByTeacher(id);
            else
                return new HttpUnauthorizedResult();
        }

        [RCPAuthorize(Roles ="Admin")]
        public ActionResult Add()
        {
            ViewBag.Title = "Создание работника";
            return View("EditByAdmin", -1);
        }

        [HttpPost]
        public JsonResult Save(WorkerEditByAdminViewModel model)
        {
            if (model.Id == -1)
                model.Id = null;

            if (!ModelState.IsValid)
                return Json("Проверьте правильность заполненных полей");

            var appointedCourses = model.WorkersCoursesIds == null
                ? null
                : model.WorkersCoursesIds.Select(c => new AppointedCourse
                {
                    CourseId = c
                }).ToList();

            var worker = new Worker
            {
                Id = model.Id,
                Login = model.Login,
                PasswordHash = model.Password != PasswordPlaceholder
                    ? model.Password
                    : null,
                FullName = model.FullName,
                AppointedCourses = appointedCourses
            };

            var result = _workersQueryProvider.Post(worker);
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
            var result = _workersQueryProvider.Delete(id);

            var msg = result.Success
                ? "Успех"
                : result.Message;

            return Json(new
            {
                success = result.Success,
                msg
            });
        }

        [HttpPost]
        public JsonResult GetWorkerInfoForAdmin(int id)
        {
            if(id == -1)
            {
                return Json(new
                {
                    FullName = "",
                    Login = "",
                    Password = ""
                });
            }

            var worker = _workersQueryProvider.Get(id);

            ViewBag.Title = "Редактирование работника";
            return Json(new
            {
                Id = worker.Id.Value,
                FullName = worker.FullName,
                Login = worker.Login,
                Password = PasswordPlaceholder
            });
        }

        [HttpPost]
        public JsonResult GetWorkerInfoForTeacher(int id)
        {
            var worker = _workersQueryProvider.Get(id);
            var coursesIds = worker.AppointedCourses.Select(e => e.CourseId).ToList();
            var appointedCourses = _examQueryProvider.Post<ICollection<CourseDto>, ICollection<int>>("api/Exam/GetCoursesByIds", coursesIds);

            var allCourses = _coursesQueryProvider.Get();

            ViewBag.Title = "Редактирование работника";
            return Json(new
            {
                Id = worker.Id.Value,
                FullName = worker.FullName,
                WorkersCoursesIds = appointedCourses != null
                    ? appointedCourses.Select(c => c.Id).ToList()
                    : new List<int>(),
                AllCourses = allCourses != null
                    ? allCourses.Select(c => new WorkersCourseViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                    : new List<WorkersCourseViewModel>()
            });
        }

        private ActionResult EditByAdmin(int id)
        {
            return View("EditByAdmin", id);
        }

        private ActionResult EditByTeacher(int id)
        {
            throw new NotImplementedException();
        }

        private ActionResult Admin()
        {
            var workers = _workersQueryProvider.Get();

            var tableData = new TableViewModel
            {
                ColumnsNames = new string[] { "ФИО" },
                Rows = workers.Select(t => new Row
                {
                    Id = t.Id,
                    Columns = new string[] { t.FullName }
                }).ToList(),
                EditAction = "Workers/Edit/",
                DeleteAction = "Workers/Delete/",
                AddAction = "Workers/Add/"
            };

            var model = new WorkersForAdminViewModel
            {
                TableData = tableData
            };
            
            return View("Admin", model);
        }

        private ActionResult Teacher()
        {
            // TODO: Реализовать.
            var model = new WorkersForTeacherViewModel();
            return View("Teacher", model);
        }
    }
}