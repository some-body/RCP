using System.Collections.Generic;

namespace Domain.Entities
{
    public class Worker : Entity
    {
        public string FullName { get; set; }
        public virtual ICollection<AppointedCourse> AppointedCourses { get; set; }

        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
