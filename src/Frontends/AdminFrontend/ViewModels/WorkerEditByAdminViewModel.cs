using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminFrontend.ViewModels
{
    public class WorkerEditByAdminViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<int> WorkersCoursesIds { get; set; }

        //public ICollection<WorkersCourseViewModel> AllCourses { get; set; }
    }
}