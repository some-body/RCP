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
        public JsonResult SaveByAdmin(WorkerEditByAdminViewModel model)
        {
            if (model.Id == -1)
                model.Id = null;

            if (!ModelState.IsValid)
                return Json("Проверьте правильность заполненных полей");

            var worker = new Worker
            {
                Id = model.Id,
                Login = model.Login,
                PasswordHash = model.Password != PasswordPlaceholder
                    ? model.Password
                    : null,
                FullName = model.FullName,
                AppointedCourses = null
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
        public JsonResult SaveByTeacher(WorkerEditByTeacherViewModel model)
        {
            var appointedCourses = model.AppointedCourses != null
                ? model.AppointedCourses.Select(c => new AppointedCourse
                {
                    CourseId = c
                }).ToList()
                : new List<AppointedCourse>();

            var worker = new Worker
            {
                Id = model.Id,
                Login = null,
                PasswordHash = null,
                FullName = null,
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

            ICollection<CourseDto> allCourses = null;
            bool coursesAreAvailable = true;
            try
            {
                allCourses = _coursesQueryProvider.Get();
            }
            catch
            {
                coursesAreAvailable = false;
            }

            ICollection<ExamResult> examResults = null;
            bool examResultsAreAvailable = true;
            try
            {
                examResults = _examQueryProvider
                    .Get<ICollection<ExamResult>>("api/Exam/GetExamResultsForWorker", "workerId=" + id);
            }
            catch
            {
                examResultsAreAvailable = false;
                examResults = new List<ExamResult>();
            }

            ViewBag.Title = "Информация о работнике";

            var result = Json(new
            {
                Id = worker.Id.Value,
                FullName = worker.FullName,
                AllCourses = allCourses != null
                    ? allCourses.Select(c => new AppointedCourseViewModel
                    {
                        Id = c.Id,
                        Name = c.Name + ". " + c.Description + ".",
                        IsChecked = coursesIds.Contains(c.Id)
                    }).ToList()
                    : new List<AppointedCourseViewModel>(),
                ExamResults = examResults
                    .OrderByDescending(e => e.Date)
                    .Select(e => new
                    {
                        CourseName = allCourses.FirstOrDefault(c => c.Id == e.CourseId).Name,
                        Date = e.Date.ToShortDateString(),
                        Result = e.Percentage + " баллов, " + (e.IsSuccess ? "сдано" : "не сдано")
                    }),
                CoursesAreAvailable = coursesAreAvailable,
                ExamResultsAreAvailable = examResultsAreAvailable
            });

            return result;
        }

        private ActionResult EditByAdmin(int id)
        {
            ViewBag.Title = "Редактирование работника";
            return View("EditByAdmin", id);
        }

        private ActionResult EditByTeacher(int id)
        {
            ViewBag.Title = "Редактирование работника";
            return View("EditByTeacher", id);
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

            var model = new WorkersViewModel
            {
                TableData = tableData
            };
            
            return View("Index", model);
        }

        private ActionResult Teacher()
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
                DeleteAction = null,
                AddAction = null
            };

            var model = new WorkersViewModel
            {
                TableData = tableData
            };

            return View("Index", model);
        }
    }
}