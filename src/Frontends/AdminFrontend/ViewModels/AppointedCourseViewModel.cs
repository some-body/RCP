using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminFrontend.ViewModels
{
    public class AppointedCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}