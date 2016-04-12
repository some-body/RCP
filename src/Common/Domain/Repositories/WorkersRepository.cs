using Domain.Entities;
using System.Data.Entity;

namespace Domain.Repositories
{
    public class WorkersRepository : Repository<Worker>
    {
        public WorkersRepository()
        {
            _dbContext = new UsersContext();
        }

        protected override DbSet<Worker> GetEntityList()
        {
            return ((UsersContext)_dbContext).Workers;
        }
    }
}
