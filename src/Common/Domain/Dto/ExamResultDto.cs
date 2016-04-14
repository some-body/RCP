using System.Collections.Generic;

namespace Domain.Dto
{
    public class ExamResultDto
    {
        public int WorkerId { get; set; }
        public int CourseId { get; set; }
        public ICollection<int> CheckedAnswersIds { get; set; }
        public ICollection<int> QuestionsIds { get; set; }
    }
}
