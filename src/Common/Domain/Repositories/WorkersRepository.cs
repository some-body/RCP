using Domain.Entities;
using System.Data.Entity;
using System.Linq;

namespace Domain.Repositories
{
    public class WorkersRepository : Repository<Worker>
    {
        public WorkersRepository()
        {
            _dbContext = new UsersContext();
        }

        public override void Update(int id, Worker entity)
        {
            if(entity.AppointedCourses != null)
            {
                var context = ((UsersContext)_dbContext);
                var courses = context.AppointedCourses.ToList();
                courses = courses.Where(e => entity.AppointedCourses.Any(c => c.CourseId == e.CourseId))
                    .ToList();

                context.AppointedCourses.RemoveRange(courses);
            }

            base.Update(id, entity);
        }

        protected override DbSet<Worker> GetEntityList()
        {
            return ((UsersContext)_dbContext).Workers;
        }
    }
}
