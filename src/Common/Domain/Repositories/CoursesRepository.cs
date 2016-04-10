using Domain.Entities;
using System.Data.Entity;

namespace Domain.Repositories
{
    public class CoursesRepository : Repository<Course>
    {
        public CoursesRepository()
        {
            _dbContext = new CoursesContext();
        }

        protected override DbSet<Course> GetEntityList()
        {
            return ((CoursesContext)_dbContext).Courses;
        }
    }
}
