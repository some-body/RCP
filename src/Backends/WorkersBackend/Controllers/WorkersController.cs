﻿using Distributed;
using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Tools;

namespace WorkersBackend.Controllers
{
    public class WorkersController : ApiController
    {
        private IRepository<Worker> _workersRepository;
        private HashGenerator _hashGenerator;

        public WorkersController()
        {
            _workersRepository = new WorkersRepository();
            _hashGenerator = new HashGenerator();
        }

        // GET: api/Workers
        public IEnumerable<WorkerDto> Get()
        {
            return _workersRepository
                .GetAll()
                .Select(e => new WorkerDto
                {
                    Id = e.Id ?? 0,
                    FullName = e.FullName
                })
                .AsEnumerable();
        }

        // GET: api/Workers/5
        public Worker Get(int id)
        {
            try
            {
                var worker = _workersRepository.GetById(id);
                return worker;
            }
            catch
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
        }

        // POST: api/Workers
        public QueryResult Post([FromBody]Worker entity)
        {
            var result = new QueryResult();
            try
            {
                entity.PasswordHash = entity.PasswordHash != null
                    ? _hashGenerator.Generate(entity.PasswordHash)
                    : null;

                _workersRepository.Save(entity);
                //ActionContext.Response.StatusCode = HttpStatusCode.Created;
                result.Success = true;
            }
            catch (Exception ex)
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }

        // DELETE: api/Workers/5
        public QueryResult Delete(int id)
        {
            var result = new QueryResult();
            try
            {
                _workersRepository.RemoveById(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                //ActionContext.Response.StatusCode = HttpStatusCode.NotFound;
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            return result;
        }
    }
}
