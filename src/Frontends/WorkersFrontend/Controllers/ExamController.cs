using Distributed;
using Domain.Dto;
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
            var action = "api/Exam/GetRandomQuestionsForCourse";
            var data = "courseId=" + courseId.ToString();
            var questions = _examQueryProvider.Get<ICollection<QuestionDto>>(action, data);
            return View(new ExamViewModel { Questions = questions });
        }
    }
}