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
    public class SystemUserRepositoryTests
    {
        [TestMethod]
        public void SystemUsers_GetAll_DbContextHasOneRecord_ReturnsOneRecords()
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
        public void SystemUsers_Save_SaveUserWithId1_UpdatingCourse()
        {
            // Arrange.
            var usersRepo = SetupRepo();

            // Act.
            var userToSave = new SystemUser
            {
                Id = 1,
                Login = "sys2",
                PasswordHash = "1234",
                Role = "Admin"
            };

            usersRepo.Save(userToSave);

            // Assert.
            var expectedLogin = "sys2";

            var result = usersRepo.GetById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLogin, result.Login);
        }

        private SystemUsersRepository SetupRepo()
        {
            var dbContextMoq = new Mock<IUsersContext>();
            var usersMoq = new Mock<DbSet<SystemUser>>();

            var testUsersSet = new List<SystemUser>
            {
                new SystemUser
                {
                    Id = 1,
                    Login = "sys",
                    PasswordHash = "123",
                    Role = "Admin"
                }
            };
            usersMoq.SetupIQueryable(testUsersSet.AsQueryable());

            dbContextMoq
                .Setup(ctx => ctx.SystemUsers)
                .Returns(usersMoq.Object);

            return new SystemUsersRepository(dbContextMoq.Object);
        }
    }
}
