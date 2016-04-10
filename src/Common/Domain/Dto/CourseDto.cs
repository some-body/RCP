using Domain.Entities;

namespace Domain.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CourseDto(Course course)
        {
            Id = course.Id ?? 0;
            Name = course.Name;
            Description = course.Description;
        }
    }
}
