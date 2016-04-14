using Domain.Entities;
using System.Data.Entity;

namespace Domain.Repositories
{
    public class SystemUsersRepository : Repository<SystemUser>
    {
        public SystemUsersRepository()
        {
            _dbContext = new UsersContext();
        }

        protected override DbSet<SystemUser> GetEntityList()
        {
            return ((UsersContext)_dbContext).SystemUsers;
        }
    }
}
