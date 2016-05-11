using Domain.Entities;
using System.Data.Entity;

namespace Domain.Contexts
{
    public interface ICoursesContext : ISaveContext
    {
        DbSet<Answer> Answers { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Course> Courses { get; set; }
    }
}
