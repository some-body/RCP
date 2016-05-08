using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Repositories;
using Moq;
using System.Data.Entity;
using System.Collections.Generic;
using Domain.Entities;
using System.Linq;

namespace Domain.UnitTests
{
    [TestClass]
    public class CoursesRepositoryTests
    {
        [TestMethod]
        public void GetAll_DbContextHasOneRecord_ReturnsTwoRecords()
        {
            // Arrange.
            var dbContextMoq = new Mock<DbContext>();
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
            
            //var courseRepo = new CoursesRepository(dbContextMoq.Object, 
            //    ctx => testCoursesSet.AsQueryable());
        }
    }
}
