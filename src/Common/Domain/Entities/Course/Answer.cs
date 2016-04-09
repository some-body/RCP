namespace Domain.Entities
{
    public class Answer : Entity
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
