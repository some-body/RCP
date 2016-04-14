namespace Domain.Dto
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
