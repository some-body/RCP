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
    public class WorkersRangeController : ApiController
    {
        private IRepository<Worker> _workersRepository;

        public WorkersRangeController()
        {
            _workersRepository = new WorkersRepository();
        }

        [HttpPost]
        public IEnumerable<WorkerDto> GetWorkersByIds([FromBody] ICollection<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<WorkerDto>();

            return _workersRepository
                .GetAll()
                .Where(e => e.Id.HasValue && ids.Contains(e.Id.Value))
                .Select(e => new WorkerDto
                {
                    Id = e.Id ?? 0,
                    FullName = e.FullName
                })
                .AsEnumerable();
        }
    }
}
