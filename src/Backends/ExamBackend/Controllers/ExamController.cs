﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;

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
        public IEnumerable<QuestionDto> GetRandomQuestionsForCourse(int courseId, int count = 10)
        {
            // TODO: Выбирать не все, а пачку
            var questions = _coursesRepository.GetById(courseId)
                .Questions
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
            
            // TODO: Запретить кэширование.
            return questions;
        }

        [HttpPost]
        public bool SaveExamResult(ExamResultDto result)
        {
            // TODO: Проверять, что ответы верны.
            var course = _coursesRepository.GetById(result.CourseId);
            if (course == null)
                return false;

            var correctAnswersIds = new List<int>();
            foreach(var q in course.Questions)
            {
                var answers = q.Answers
                    .Where(a => a.IsCorrect)
                    .Select(a => a.Id.Value)
                    .ToList();

                correctAnswersIds.AddRange(answers);
            }

            var isPassed = correctAnswersIds
                .OrderBy(a => a)
                .SequenceEqual(
                    result.CheckedAnswersIds
                    .OrderBy(a => a)
                );

            var examResult = new ExamResult
            {
                WorkerId = result.WorkerId,
                CourseId = result.CourseId,
                IsSuccess = isPassed
            };

            examResult.Id = null;
            _examResultsRepository.Save(examResult);

            return isPassed;
        }

        [HttpPost]
        public IEnumerable<CourseDto> GetCoursesByIds([FromBody] ICollection<int> ids)
        {
            if (ids == null || !ids.Any())
                return null;

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
