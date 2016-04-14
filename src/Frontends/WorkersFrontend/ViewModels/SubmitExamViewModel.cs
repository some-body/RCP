using System.Collections.Generic;

namespace WorkersFrontend.ViewModels
{
    public class SubmitExamViewModel
    {
        public int CourseId { get; set; }
        public ICollection<int> CheckedAnswersIds { get; set; }
    }
}