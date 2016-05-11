using Domain.Entities;
using System.Data.Entity;

namespace Domain.Contexts
{
    public interface IUsersContext : ISaveContext
    {
        DbSet<SystemUser> SystemUsers { get; set; }
        DbSet<Worker> Workers { get; set; }
        DbSet<AppointedCourse> AppointedCourses { get; set; }
    }
}
