using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminFrontend.ViewModels
{
    public class WorkerEditByTeacherViewModel
    {
        public int? Id { get; set; }

        public ICollection<int> AppointedCourses { get; set; }
    }
}