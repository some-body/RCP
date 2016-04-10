using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PreparationBackend.Controllers
{
    public class CoursesController : ApiController
    {
        private IRepository<Course> _courseRepository;

        public CoursesController()
        {
            _courseRepository = new CoursesRepository();
        }

        // GET: api/Courses
        public IEnumerable<CourseDto> Get()
        {
            return _courseRepository
                .GetAll()
                .Select(e => new CourseDto
                {
                    Id = e.Id ?? 0,
                    Name = e.Name,
                    Description = e.Description
                })
                .AsEnumerable();
        }

        // GET: api/Courses/5
        public Course Get(int id)
        {
            return _courseRepository.GetById(id);
        }

        // POST: api/Courses
        public void Post([FromBody]Course entity)
        {
            _courseRepository.Save(entity);
        }

        // DELETE: api/Courses/5
        public void Delete(int id)
        {
            _courseRepository.RemoveById(id);
        }
    }
}
