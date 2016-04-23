﻿using Distributed;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tools;

namespace SystemUsersBackend.Controllers
{
    public class SystemUsersController : ApiController
    {
        private IRepository<SystemUser> _systemUsersRepository;
        private HashGenerator _hashGenerator;

        public SystemUsersController()
        {
            _systemUsersRepository = new SystemUsersRepository();
            _hashGenerator = new HashGenerator();
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
            entity.PasswordHash = entity.PasswordHash != null
                ? _hashGenerator.Generate(entity.PasswordHash)
                : null;

            var result = new QueryResult();
            try
            {
                _systemUsersRepository.Save(entity);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
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
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }
    }
}
