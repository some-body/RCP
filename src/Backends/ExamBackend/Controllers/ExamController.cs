using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using Tools;
using ExamBackend.Tools;
using System.Net;

namespace ExamBackend.Controllers
{
    public class ExamController : ApiController
    {
        private IRepository<ExamResult> _examResultsRepository;
        private IRepository<Course> _coursesRepository;
        private ExamResultChecker _examResultChecker;

        public ExamController()
        {
            _examResultsRepository = new ExamResultsRepository();
            _coursesRepository = new CoursesRepository();
            _examResultChecker = new ExamResultChecker(_coursesRepository);
        }

        [HttpGet]
        public IEnumerable<QuestionDto> GetRandomQuestionsForCourse(int courseId, int count = 5)
        {
            var questions = _coursesRepository.GetById(courseId)
                .Questions
                .RandomSelect(count)
                .Select(q => new QuestionDto
                {
                    Id = q.Id ?? 0,
                    Text = q.Text,
                    Answers = q.Answers.Select(a => new AnswerDto
                    {
                        Id = a.Id ?? 0,
                        Text = a.Text
                    }).ToList()
                })
                .AsEnumerable();

            Request.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
            return questions;
        }

        [HttpPost]
        public bool SaveExamResult(ExamResultDto result)
        {
            var checkedResult = _examResultChecker.Check(result);

            if (checkedResult == null)
                return false;

            _examResultsRepository.Save(checkedResult);

            return checkedResult.IsSuccess;
        }

        [HttpGet]
        public IEnumerable<ExamResult> GetExamResultsForWorker(int workerId)
        {
            try
            {
                return _examResultsRepository
                    .GetAll()
                    .Where(e => e.WorkerId == workerId);
            }
            catch
            {
                ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
        }

        [HttpGet]
        public IEnumerable<ExamResult> GetExamResultsForCourse(int courseId)
        {
            try
            {
                return _examResultsRepository
                    .GetAll()
                    .Where(e => e.CourseId == courseId);
            }
            catch
            {
                ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
        }

        [HttpPost]
        public IEnumerable<CourseDto> GetCoursesByIds([FromBody] ICollection<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<CourseDto>();

            return _coursesRepository
                .GetAll()
                .Where(e => e.Id.HasValue && ids.Contains(e.Id.Value))
                .Select(e => new CourseDto
                {
                    Id = e.Id ?? 0,
                    Name = e.Name,
                    Description = e.Description
                })
                .AsEnumerable();
        }
    }
}
