using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<AnswerDto> Answers { get; set; } 
    }
}
