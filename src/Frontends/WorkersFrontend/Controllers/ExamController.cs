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
        public static IDictionary<string, ICollection<int>> _examQuestionsForUser { get; private set; } 
            = new Dictionary<string, ICollection<int>>();

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
            var data = new List<int> { 1, 2 };

            var courses = _examQueryProvider.Post<ICollection<CourseDto>, ICollection<int>>(action, data);
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
            // TODO: Работника проставлять из кук.
            var token = "123";

            // TODO: Проверка на то, что курс с таким id есть у работника.

            var action = "api/Exam/GetRandomQuestionsForCourse";
            var data = "courseId=" + courseId.ToString();
            var questions = _examQueryProvider.Get<ICollection<QuestionDto>>(action, data);

            var questionsIds = questions.Select(q => q.Id).ToList();
            _examQuestionsForUser.Add(token, questionsIds);

            return Json(new ExamViewModel
            {
                Questions = questions
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitExam(SubmitExamViewModel result)
        {
            // TODO: Работника проставлять из кук.
            var token = "123";
            var workerId = 123;

            // TODO: Сказать, что ошибка.
            if (!_examQuestionsForUser.ContainsKey(token))
                return Json(false);

            var questionsIds = _examQuestionsForUser[token];

            var examResultDto = new ExamResultDto
            {
                WorkerId = workerId,
                CourseId = result.CourseId,
                QuestionsIds = questionsIds,
                CheckedAnswersIds = result.CheckedAnswersIds
            };

            var action = "api/Exam/SaveExamResult";
            var isPassed = _examQueryProvider.Post<bool, ExamResultDto>(action, examResultDto);

            _examQuestionsForUser.Remove(token);

            return Json(isPassed);
        }
    }
}