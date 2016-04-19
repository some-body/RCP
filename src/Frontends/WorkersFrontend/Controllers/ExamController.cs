using Distributed;
using Domain.Dto;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WorkersFrontend.Auth;
using WorkersFrontend.ViewModels;

namespace WorkersFrontend.Controllers
{
    [RCPAuthorize]
    public class ExamController : RCPController
    {
        public static IDictionary<string, ICollection<QuestionDto>> _examQuestionsForUser { get; private set; } 
            = new Dictionary<string, ICollection<QuestionDto>>();

        private CustomApiQueryProvider _examQueryProvider;
        private ApiQueryProvider<ICollection<WorkerDto>, Worker> _workersQueryProvider;

        public ExamController()
        {
            var examBackendUrl = Properties.Resources.ExamBackendURL;
            _examQueryProvider = new CustomApiQueryProvider(examBackendUrl);

            var workersBackendUrl = Properties.Resources.WorkersBackendURL;
            _workersQueryProvider = new ApiQueryProvider<ICollection<WorkerDto>, Worker>(workersBackendUrl, "Workers");
        }

        // GET: Exam
        public ActionResult Index()
        {
            var action = "api/Exam/GetCoursesByIds";

            var worker = _workersQueryProvider.Get(UserId);
            var coursesIds = worker.AppointedCourses.Select(e => e.CourseId).ToList();
            if(coursesIds != null)
            {
                var courses = _examQueryProvider.Post<ICollection<CourseDto>, ICollection<int>>(action, coursesIds);
                return View(new CoursesViewModel
                {
                    Courses = courses ?? new List<CourseDto>()
                });
            }
            else
            {
                return View(new CoursesViewModel
                {
                    Courses = new List<CourseDto>()
                });
            }
        }

        public ActionResult StartExam(int courseId)
        {
            return View(new StartExamViewModel { CourseId = courseId });
        }

        public JsonResult GetRandomQuestions(int courseId)
        {
            var token = Request.Cookies["token"].Value;

            if (_examQuestionsForUser.ContainsKey(token))
            {
                return Json(new ExamViewModel
                {
                    Questions = _examQuestionsForUser[token]
                }, JsonRequestBehavior.AllowGet);
            }
            // TODO: Проверка на то, что курс с таким id есть у работника.

            var action = "api/Exam/GetRandomQuestionsForCourse";
            var data = "courseId=" + courseId.ToString();
            var questions = _examQueryProvider.Get<ICollection<QuestionDto>>(action, data);

            //var questionsIds = questions.Select(q => q.Id).ToList();
            if (_examQuestionsForUser.ContainsKey(token))
                _examQuestionsForUser.Remove(token);

            _examQuestionsForUser.Add(token, questions);

            return Json(new ExamViewModel
            {
                Questions = questions
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SubmitExam(SubmitExamViewModel result)
        {
            // TODO: Работника проставлять из кук.
            var token = Request.Cookies["token"].Value;
            var workerId = UserId;

            // TODO: Сказать, что ошибка.
            if (!_examQuestionsForUser.ContainsKey(token))
                return Json(false);

            var questions = _examQuestionsForUser[token];

            var examResultDto = new ExamResultDto
            {
                WorkerId = workerId,
                CourseId = result.CourseId,
                QuestionsIds = questions.Select(q => q.Id).ToList(),
                CheckedAnswersIds = result.CheckedAnswersIds
            };

            var action = "api/Exam/SaveExamResult";
            var isPassed = _examQueryProvider.Post<bool, ExamResultDto>(action, examResultDto);

            _examQuestionsForUser.Remove(token);

            if (isPassed)
            {
                var examResults = _examQueryProvider
                    .Get<ICollection<ExamResult>>("api/Exam/GetExamResults", "workerId=" + workerId);

                var existingWorker = _workersQueryProvider.Get(workerId);
                var updatedAppointedCourses = existingWorker.AppointedCourses
                        .Where(e => !examResults.Any(r => r.CourseId == e.CourseId && r.IsSuccess))
                        .ToList();

                var worker = new Worker
                {
                    Id = workerId,
                    AppointedCourses = updatedAppointedCourses,
                    FullName = null,
                    Login = null,
                    PasswordHash = null
                };

                _workersQueryProvider.Post(worker);
            }

            return Json(isPassed);
        }
    }
}