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
    public class WorkersRepositoryTests
    {
        [TestMethod]
        public void Workers_GetAll_DbContextHasOneRecord_ReturnsOneRecords()
        {
            // Arrange.
            var usersRepo = SetupRepo();

            // Act.
            var data = usersRepo.GetAll();
            var actual = data.Count();

            // Assert.
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Workers_Save_SaveWorkerWithId1_UpdatingCourse()
        {
            // Arrange.
            var usersRepo = SetupRepo();

            // Act.
            var workerToSave = new Worker
            {
                Id = 1,
                Login = "worker1",
                PasswordHash = "1234",
                FullName = "Иван1"
            };

            usersRepo.Save(workerToSave);

            // Assert.
            var expectedFullName = "Иван1";

            var result = usersRepo.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedFullName, result.FullName);
        }

        private WorkersRepository SetupRepo()
        {
            var dbContextMoq = new Mock<IUsersContext>();
            var usersMoq = new Mock<DbSet<Worker>>();

            var testUsersSet = new List<Worker>
            {
                new Worker
                {
                    Id = 1,
                    Login = "sys",
                    PasswordHash = "123",
                    FullName = "Иван"
                }
            };
            usersMoq.SetupIQueryable(testUsersSet.AsQueryable());

            dbContextMoq
                .Setup(ctx => ctx.Workers)
                .Returns(usersMoq.Object);

            return new WorkersRepository(dbContextMoq.Object);
        }
    }
}
