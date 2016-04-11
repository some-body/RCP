using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    class ExamResultDto
    {
        public int WorkerId { get; set; }
        public int CourseId { get; set; }
        // TODO: Подумать, как быть с рандомным сетом вопросов. (может хеш?)
        public ICollection<int> AnswersIds { get; set; }
    }
}
