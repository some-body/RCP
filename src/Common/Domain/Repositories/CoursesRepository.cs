using Domain.Contexts;
using Domain.Entities;
using System;
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

        public CoursesRepository(ISaveContext context)
        {
            _dbContext = context;
        }

        public override void Update(int id, Course entity)
        {
            if (entity.Questions != null)
            {
                var list = GetEntityList();
                var existingCourse = list.FirstOrDefault(e => e.Id == id);

                if (existingCourse != null)
                    existingCourse.Questions.Clear();
            }
            base.Update(id, entity);
        }

        protected override IDbSet<Course> GetEntityList()
        {
            return ((ICoursesContext)_dbContext).Courses;
        }
    }
}
