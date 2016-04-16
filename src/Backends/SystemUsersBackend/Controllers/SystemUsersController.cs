using Distributed;
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
        public QueryResult Post([FromBody]SystemUser entity)
        {
            var result = new QueryResult();
            try
            {
                _systemUsersRepository.Save(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        // DELETE: api/SystemUsers/5
        public QueryResult Delete(int id)
        {
            var result = new QueryResult();
            try
            {
                _systemUsersRepository.RemoveById(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
