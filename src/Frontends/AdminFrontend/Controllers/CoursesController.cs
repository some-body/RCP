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
        private ApiQueryProvider<ICollection<WorkerDto>, Worker> _workersQueryProvider;
        private CustomApiQueryProvider _workersRangeQueryProvider;

        private ApiQueryProvider<ICollection<CourseDto>, Course> _courseQueryProvider;
        private CustomApiQueryProvider _examQueryProvider;

        public CoursesController()
        {
            var workersBackendUrl = Properties.Resources.WorkersBackendURL;
            _workersQueryProvider = new ApiQueryProvider<ICollection<WorkerDto>, Worker>(workersBackendUrl, "Workers");

            var backendUrl = Properties.Resources.PreparationBackendURL;
            _courseQueryProvider = new ApiQueryProvider<ICollection<CourseDto>, Course>(backendUrl, "Courses");

            var examBackendUrl = Properties.Resources.ExamBackendURL;
            _examQueryProvider = new CustomApiQueryProvider(examBackendUrl);

            _workersRangeQueryProvider = new CustomApiQueryProvider(workersBackendUrl);
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
                InfoAction = "Courses/ExamResults/",
                EditAction = "Courses/Edit/",
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
                : new Course()
                {
                    Id = -1,
                    Name = "",
                    Description = "",
                    MaterialText = "",
                    MinPercentage = 75,
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

        public ActionResult ExamResults(int id)
        {
            var examResults = _examQueryProvider
                .Get<ICollection<ExamResult>>("api/Exam/GetExamResultsForCourse", "courseId=" + id);

            var workersIds = examResults
                .Select(e => e.WorkerId)
                .Distinct()
                .ToList();

            var workers = _workersRangeQueryProvider
                .Post<ICollection<WorkerDto>, ICollection<int>>("api/WorkersRange/GetWorkersByIds", workersIds);

            var model = new ExamResultsViewModel
            {
                ExamResults = examResults.Select(e => new ExamResultViewModel
                {
                    FullName = workers.FirstOrDefault(w => w.Id == e.WorkerId)?.FullName ?? "-",
                    ExamDate = e.Date.ToShortDateString(),
                    Result = e.Percentage + " ,баллов, " + (e.IsSuccess ? "сдано" : "не сдано")
                }).ToList()
            };

            return View(model);
        }
    }
}