using Domain.Entities;
using System.Data.Entity;

namespace Domain
{
    public class ExamContext : DbContext
    {
        public DbSet<ExamResult> ExamResults { get; set; }
    }
}
