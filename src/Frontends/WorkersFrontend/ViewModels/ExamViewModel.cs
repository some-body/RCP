using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersFrontend.ViewModels
{
    public class ExamViewModel
    {
        public ICollection<QuestionDto> Questions { get; set; }
    }
}