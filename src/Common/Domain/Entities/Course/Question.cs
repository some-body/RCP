using System.Collections.Generic;

namespace Domain.Entities
{
    public class Question : Entity
    {
        public string Text { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
