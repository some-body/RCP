using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
