using Domain.Entities;
using System.Data.Entity;

namespace Domain.Repositories
{
    public class ExamResultsRepository : Repository<ExamResult>
    {
        public ExamResultsRepository()
        {
            _dbContext = new ExamContext();
        }

        protected override IDbSet<ExamResult> GetEntityList()
        {
            return ((ExamContext)_dbContext).ExamResults;
        }
    }
}
