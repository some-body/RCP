using Domain.Dto;
using System.Collections.Generic;

namespace WorkersFrontend.ViewModels
{
    public class ExamViewModel
    {
        public ICollection<QuestionDto> Questions { get; set; }
    }
}