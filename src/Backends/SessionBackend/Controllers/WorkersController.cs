using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using SessionBackend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Tools;

namespace SessionBackend.Controllers
{
    public class WorkersController : ApiController
    {
        public static IDictionary<string, WorkerTokenRecord> TokensList { get; private set; } = new Dictionary<string, WorkerTokenRecord>();

        private const int ExpiresInSeconds = 20000;

        private IRepository<Worker> _workersRepository;
        private TokenGenerator _tokenGenerator;
        private HashGenerator _hashGenerator;

        public WorkersController()
        {
            _workersRepository = new WorkersRepository();
            _tokenGenerator = new TokenGenerator(ExpiresInSeconds);
            _hashGenerator = new HashGenerator();
        }

        [HttpPost]
        public WorkerSignInDto SignIn([FromBody]LoginDto loginDto)
        {
            var passwordHash = _hashGenerator.Generate(loginDto.Password);

            var worker = _workersRepository.GetAll()
                .FirstOrDefault(w => w.Login == loginDto.Login && w.PasswordHash == passwordHash);

            if (worker == null)
                return null;

            var token = _tokenGenerator.GenerateToken(worker.Login, worker.PasswordHash);
            var workerDto = new WorkerDto
            {
                Id = worker.Id.Value,
                FullName = worker.FullName
            };

            TokensList.Add(token.Value, new WorkerTokenRecord { Token = token, Worker = workerDto });

            return new WorkerSignInDto
            {
                Token = token,
                Worker = workerDto
            };
        }

        [HttpPost]
        public void SignOut([FromBody]string tokenValue)
        {
            if (TokensList.ContainsKey(tokenValue))
                TokensList.Remove(tokenValue);
        }

        [HttpPost]
        public WorkerDto GetByToken([FromBody]string tokenValue)
        {
            if (tokenValue == null)
                return null;

            if (!TokensList.ContainsKey(tokenValue))
                return null;

            var tokenRecord = TokensList[tokenValue];

            if (tokenRecord.Token.ExpiresOn < DateTime.Now)
            {
                TokensList.Remove(tokenValue);
                return null;
            }

            return tokenRecord.Worker;
        }
    }
}
