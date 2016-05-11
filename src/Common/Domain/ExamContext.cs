using Domain.Contexts;
using Domain.Entities;
using System.Data.Entity;

namespace Domain
{
    public class ExamContext : DbContext, IExamContext
    {
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
