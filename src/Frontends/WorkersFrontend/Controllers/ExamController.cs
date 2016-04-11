using Distributed;
using Domain.Dto;
using Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkersFrontend.ViewModels;

namespace WorkersFrontend.Controllers
{
    public class ExamController : Controller
    {
        private CustomApiQueryProvider _examQueryProvider;

        public ExamController()
        {
            var backendUrl = Properties.Resources.ExamBackendURL;
            _examQueryProvider = new CustomApiQueryProvider(backendUrl);
        }

        // GET: Exam
        public ActionResult Index()
        {
            var action = "api/Exam/GetCoursesByIds";
            // TODO: Получать список курсов работника
            var data = "[1, 2]";

            var courses = _examQueryProvider.Post<ICollection<CourseDto>, string>(action, data);
            return View(new CoursesViewModel
            {
                Courses = courses
            });
        }

        public ActionResult StartExam(int courseId)
        {
            return View(new StartExamViewModel { CourseId = courseId });
        }

        public JsonResult GetRandomQuestions(int courseId)
        {
            var action = "api/Exam/GetRandomQuestionsForCourse";
            var data = "courseId=" + courseId.ToString();
            var questions = _examQueryProvider.Get<ICollection<QuestionDto>>(action, data);

            return Json(new ExamViewModel
            {
                Questions = questions
            }, JsonRequestBehavior.AllowGet);
        }

        public RedirectToRouteResult SubmitExam(SubmitExamViewModel result)
        {
            // TODO: Работника проставлять из кук.
            var result = new ExamResult
            {
                WorkerId = 1,
                CourseId = 
            }

            _examQueryProvider.Post<bool, ExamResult>()

            return RedirectToAction("Index", "Home", null);
        }
    }
}