using Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace Domain.Repositories
{
    public class CoursesRepository : Repository<Course>
    {
        public CoursesRepository()
        {
            _dbContext = new CoursesContext();
        }

        public override void Update(int id, Course entity)
        {
            if (entity.Questions != null)
            {
                var existingCourse = GetEntityList().FirstOrDefault(e => e.Id == id);

                if (existingCourse != null)
                    existingCourse.Questions.Clear();
            }
            base.Update(id, entity);
        }

        protected override DbSet<Course> GetEntityList()
        {
            return ((CoursesContext)_dbContext).Courses;
        }
    }
}
