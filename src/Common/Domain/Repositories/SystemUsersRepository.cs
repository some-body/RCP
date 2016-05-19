using Domain.Contexts;
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

        public SystemUsersRepository(ISaveContext context)
        {
            _dbContext = context;
        }

        protected override IDbSet<SystemUser> GetEntityList()
        {
            return ((IUsersContext)_dbContext).SystemUsers;
        }
    }
}
