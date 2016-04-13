using Domain.Entities;
using System.Data.Entity;

namespace Domain
{
    public class UsersContext : DbContext
    {
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<AppointedCourse> AppointedCourses { get; set; }
    }
}
