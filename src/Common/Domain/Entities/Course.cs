using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Course
    {
        public string MaterialText { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
