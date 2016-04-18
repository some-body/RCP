using Domain.Entities;
using System.Collections.Generic;
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
            if(entity.Questions != null)
            {
                var context = (CoursesContext)_dbContext;
                var existingCourse = context.Courses.FirstOrDefault(e => e.Id == id);
                if(existingCourse != null)
                {
                    var questionsToRemove = new List<Question>();
                    foreach(var q in context.Questions)
                    {
                        if (existingCourse.Questions.Contains(q))
                        {
                            questionsToRemove.Add(q);
                        }
                    }
                    //var questionsToRemove = context.Questions
                    //    .Where(q => existingCourse.Questions.Contains(q))
                    //    .ToList();
                    existingCourse.Questions.Clear();
                }
            }

            base.Update(id, entity);
        }

        protected override DbSet<Course> GetEntityList()
        {
            return ((CoursesContext)_dbContext).Courses;
        }
    }
}
