using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamBackend.Tools
{
    public class ExamResultChecker
    {
        private IRepository<Course> _coursesRepository;

        public ExamResultChecker(IRepository<Course> coursesRepo)
        {
            _coursesRepository = coursesRepo;
        }

        public ExamResult Check(ExamResultDto result)
        {
            var course = _coursesRepository.GetById(result.CourseId);
            if (course == null)
                return null;

            int corrrectQuestionsCount = 0;
            foreach (var q in course.Questions)
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
                Id = null,
                WorkerId = result.WorkerId,
                CourseId = result.CourseId,
                Date = DateTime.Now.Date,
                Percentage = rank,
                IsSuccess = isPassed
            };

            return examResult;
        }

    }
}