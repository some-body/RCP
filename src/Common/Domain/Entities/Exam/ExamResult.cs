using System;

namespace Domain.Entities
{
    public class ExamResult : Entity
    {
        public int WorkerId { get; set; }
        public int Percentage { get; set; }
        public bool IsSuccess { get; set; }
        public int CourseId { get; set; }
        public DateTime Date { get; set; }
    }
}
