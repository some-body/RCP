using System.Collections.Generic;

namespace AdminFrontend.ViewModels
{
    public class ExamResultViewModel
    {
        public string FullName { get; set; }
        public string ExamDate { get; set; }
        public string Result { get; set; }
    }

    public class ExamResultsViewModel
    {
        public ICollection<ExamResultViewModel> ExamResults { get; set; }
    }
}