using System.Collections.Generic;

namespace Domain.Dto
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<AnswerDto> Answers { get; set; } 
    }
}
