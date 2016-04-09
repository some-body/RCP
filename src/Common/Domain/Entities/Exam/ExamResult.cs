namespace Domain.Entities
{
    public class ExamResult : Entity
    {
        public bool IsSuccess { get; set; }
        public Course Course { get; set; }
    }
}
