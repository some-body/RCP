using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities;
using System.Collections.Generic;
using Domain.Dto;
using Moq;
using Domain.Repositories;
using ExamBackend.Tools;

namespace ExamBackend.UnitTests
{
    [TestClass]
    public class ExamResultCheckerTests
    {
        [TestMethod]
        public void Check_AllCorrect_100PercentResult()
        {
            // Arrange.
            var examResultChecker = CreateSimpleExamResultChecker();

            var examResult = new ExamResultDto
            {
                CourseId = 1,
                WorkerId = 1,
                QuestionsIds = new List<int> { 1, 2, 3 },
                CheckedAnswersIds = new List<int> { 1, 5, 8, 9 }
            };


            // Act.
            var actual = examResultChecker.Check(examResult);

            // Assert.
            Assert.AreEqual(null, actual.Id);
            Assert.AreEqual(1, actual.WorkerId);
            Assert.AreEqual(1, actual.CourseId);
            Assert.AreEqual(100, actual.Percentage);
            Assert.AreEqual(true, actual.IsSuccess);
        }

        [TestMethod]
        public void Check_NoCorrect_0PercentResult()
        {
            // Arrange.
            var examResultChecker = CreateSimpleExamResultChecker();

            var examResult = new ExamResultDto
            {
                CourseId = 1,
                WorkerId = 1,
                QuestionsIds = new List<int> { 1, 2, 3 },
                CheckedAnswersIds = new List<int> { 2, 4, 7 }
            };

            // Act.
            var actual = examResultChecker.Check(examResult);

            // Assert.
            Assert.AreEqual(null, actual.Id);
            Assert.AreEqual(1, actual.WorkerId);
            Assert.AreEqual(1, actual.CourseId);
            Assert.AreEqual(0, actual.Percentage);
            Assert.AreEqual(false, actual.IsSuccess);
        }

        [TestMethod]
        public void Check_3AreCorrect_75PercentResult()
        {
            // Arrange.
            var examResultChecker = CreateSimpleExamResultChecker();

            var examResult = new ExamResultDto
            {
                CourseId = 1,
                WorkerId = 1,
                QuestionsIds = new List<int> { 1, 2, 3 },
                CheckedAnswersIds = new List<int> { 1, 5, 9 }
            };

            // Act.
            var actual = examResultChecker.Check(examResult);

            // Assert.
            Assert.AreEqual(null, actual.Id);
            Assert.AreEqual(1, actual.WorkerId);
            Assert.AreEqual(1, actual.CourseId);
            Assert.AreEqual(66, actual.Percentage);
            Assert.AreEqual(false, actual.IsSuccess);
        }

        private ExamResultChecker CreateSimpleExamResultChecker()
        {
            var courseStub = new Course
            {
                Id = 1,
                Questions = GenerateQuestions()
            };

            var examRepoMoq = new Mock<IRepository<ExamResult>>();
            var courseRepoMoq = new Mock<IRepository<Course>>();
            courseRepoMoq
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(courseStub);

            var examResultChecker = new ExamResultChecker(courseRepoMoq.Object);

            return examResultChecker;
        }

        private IList<Question> GenerateQuestions()
        {
            var question1 = new Question
            {
                Id = 1,
                Answers = new List<Answer>
                    {
                        new Answer
                        {
                            Id = 1,
                            IsCorrect = true
                        },
                        new Answer
                        {
                            Id = 2,
                            IsCorrect = false
                        },
                        new Answer
                        {
                            Id = 3,
                            IsCorrect = false
                        },
                    }
            };

            var question2 = new Question
            {
                Id = 2,
                Answers = new List<Answer>
                    {
                        new Answer
                        {
                            Id = 4,
                            IsCorrect = false
                        },
                        new Answer
                        {
                            Id = 5,
                            IsCorrect = true
                        },
                        new Answer
                        {
                            Id = 6,
                            IsCorrect = false
                        },
                    }
            };

            var question3 = new Question
            {
                Id = 3,
                Answers = new List<Answer>
                    {
                        new Answer
                        {
                            Id = 7,
                            IsCorrect = false
                        },
                        new Answer
                        {
                            Id = 8,
                            IsCorrect = true
                        },
                        new Answer
                        {
                            Id = 9,
                            IsCorrect = true
                        },
                    }
            };


            var result = new List<Question> { question1, question2, question3 };

            return result;
        }
    }
}
