using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WorkersBackend.Controllers
{
    public class WorkersController : ApiController
    {
        private IRepository<Worker> _workersRepository;

        public WorkersController()
        {
            _workersRepository = new WorkersRepository();
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
            var worker = _workersRepository.GetById(id);
            return worker;
        }

        // POST: api/Workers
        public void Post([FromBody]Worker entity)
        {
            _workersRepository.Save(entity);
        }

        // DELETE: api/Workers/5
        public void Delete(int id)
        {
            _workersRepository.RemoveById(id);
        }
    }
}
