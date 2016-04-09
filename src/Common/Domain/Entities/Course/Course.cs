using System.Collections.Generic;

namespace Domain.Entities
{
    public class Course : Entity
    {
        public string Name { get; set; }
        public string MaterialText { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
