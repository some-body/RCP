using System.Collections.Generic;

namespace AdminFrontend.ViewModels
{
    public class WorkerEditByTeacherViewModel
    {
        public int? Id { get; set; }

        public ICollection<int> AppointedCourses { get; set; }
    }
}