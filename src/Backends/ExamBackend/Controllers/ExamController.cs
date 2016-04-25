﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using Tools;

namespace ExamBackend.Controllers
{
    public class ExamController : ApiController
    {
        private IRepository<ExamResult> _examResultsRepository;
        private IRepository<Course> _coursesRepository;

        public ExamController()
        {
            _examResultsRepository = new ExamResultsRepository();
            _coursesRepository = new CoursesRepository();
        }

        [HttpGet]
        public IEnumerable<QuestionDto> GetRandomQuestionsForCourse(int courseId, int count = 3)
        {
            // TODO: Выбирать не все, а пачку
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
            var course = _coursesRepository.GetById(result.CourseId);
            if (course == null)
                return false;

            int corrrectQuestionsCount = 0;
            foreach(var q in course.Questions)
            {
                var incorrect = q.Answers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id ?? 0)
                    .Except(result.CheckedAnswersIds);

                if (!incorrect.Any())
                    corrrectQuestionsCount++;
            }

            float questionsCount = course.Questions.Count;
            int rank = (int)(100 * corrrectQuestionsCount / questionsCount);
            bool isPassed = rank >= course.MinPercentage;

            var examResult = new ExamResult
            {
                WorkerId = result.WorkerId,
                CourseId = result.CourseId,
                Date = DateTime.Now.Date,
                Percentage = rank,
                IsSuccess = isPassed
            };

            examResult.Id = null;
            _examResultsRepository.Save(examResult);

            return isPassed;
        }

        [HttpGet]
        public IEnumerable<ExamResult> GetExamResultsForWorker(int workerId)
        {
            return _examResultsRepository
                .GetAll()
                .Where(e => e.WorkerId == workerId);
        }

        [HttpGet]
        public IEnumerable<ExamResult> GetExamResultsForCourse(int courseId)
        {
            return _examResultsRepository
                .GetAll()
                .Where(e => e.CourseId == courseId);
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
