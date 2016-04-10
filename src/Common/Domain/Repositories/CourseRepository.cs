using Domain.Entities;
using System.Data.Entity;

namespace Domain.Repositories
{
    public class CoursesRepository : Repository<Course>
    {
        protected override DbSet<Course> GetEntityList()
        {
            return _dbContext.Courses;
        }
    }
}
