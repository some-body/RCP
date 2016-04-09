using System.Collections.Generic;

namespace Domain.Entities
{
    public class Worker : Entity
    {
        public string FullName { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
