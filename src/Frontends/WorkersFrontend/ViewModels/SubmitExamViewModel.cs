using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersFrontend.ViewModels
{
    public class SubmitExamViewModel
    {
        public int CourseId { get; set; }
        public ICollection<int> CheckedAnswersIds { get; set; }
    }
}