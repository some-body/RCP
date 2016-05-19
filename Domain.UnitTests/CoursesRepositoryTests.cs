using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Repositories;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using Domain.Entities;
using System.Linq;
using TestUtils;
using Domain.Contexts;

namespace Domain.UnitTests
{
    [TestClass]
    public class CoursesRepositoryTests
    {
        [TestMethod]
        public void Courses_GetAll_DbContextHasOneRecord_ReturnsOneRecords()
        {
            // Arrange.
            var coursesRepo = SetupRepo();

            // Act.
            var data = coursesRepo.GetAll();
            var actual = data.Count();

            // Assert.
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Courses_Save_SaveCourseWithId1_UpdatingCourse()
        {
            // Arrange.
            var coursesRepo = SetupRepo();

            // Act.
            var courseToSave = new Course
            {
                Id = 1,
                Name = "TestName2",
                Description = "TestDescription2",
                MaterialText = "TestMaterialText2"
            };

            coursesRepo.Save(courseToSave);

            // Assert.
            var expectedName = "TestName2";

            var result = coursesRepo.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedName, result.Name);
        }

        private CoursesRepository SetupRepo()
        {
            var dbContextMoq = new Mock<ICoursesContext>();
            var coursesMoq = new Mock<DbSet<Course>>();

            var testCoursesSet = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "TestName1",
                    Description = "TestDescription1",
                    MaterialText = "TestMaterialText1"
                }
            };
            coursesMoq.SetupIQueryable(testCoursesSet.AsQueryable());

            dbContextMoq
                .Setup(ctx => ctx.Courses)
                .Returns(coursesMoq.Object);

            return new CoursesRepository(dbContextMoq.Object);
        }
    }
}
