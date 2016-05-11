using Domain.Entities;
using System.Data.Entity;

namespace Domain.Contexts
{
    public interface IExamContext : ISaveContext
    {
        DbSet<ExamResult> ExamResults { get; set; }
    }
}
