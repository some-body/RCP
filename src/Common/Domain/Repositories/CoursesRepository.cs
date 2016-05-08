using Domain.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Domain.Repositories
{
    public class CoursesRepository : Repository<Course>
    {
        private Func<DbContext, DbSet<Course>> _entityListProvider = null;

        public CoursesRepository()
        {
            _dbContext = new CoursesContext();
        }

        public CoursesRepository(DbContext context, Func<DbContext, DbSet<Course>> entityListProvider)
        {
            _dbContext = context;
            _entityListProvider = entityListProvider;
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
            if (_entityListProvider == null)
                return ((CoursesContext)_dbContext).Courses;
            else
                return _entityListProvider.Invoke(_dbContext);
        }
    }
}
