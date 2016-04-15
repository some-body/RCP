using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemUsersBackend.Controllers
{
    public class SystemUsersController : ApiController
    {
        private IRepository<SystemUser> _systemUsersRepository;

        public SystemUsersController()
        {
            _systemUsersRepository = new SystemUsersRepository();
        }

        // GET: api/SystemUsers
        public IEnumerable<SystemUser> Get()
        {
            return _systemUsersRepository
                .GetAll()
                .AsEnumerable();
        }

        // GET: api/SystemUsers/5
        public SystemUser Get(int id)
        {
            return _systemUsersRepository.GetById(id);
        }

        // POST: api/SystemUsers
        public void Post([FromBody]SystemUser entity)
        {
            _systemUsersRepository.Save(entity);
        }

        // DELETE: api/SystemUsers/5
        public void Delete(int id)
        {
            _systemUsersRepository.RemoveById(id);
        }
    }
}
