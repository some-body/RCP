using Domain.Dto;
using Domain.Entities;
using Domain.Repositories;
using SessionBackend.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SessionBackend.Controllers
{
    public class WorkersController : ApiController
    {
        public static IDictionary<string, WorkerTokenRecord> TokensList { get; private set; } = new Dictionary<string, WorkerTokenRecord>();

        private const int ExpiresInSeconds = 200;

        private IRepository<Worker> _workersRepository;

        public WorkersController()
        {
            _workersRepository = new WorkersRepository();
        }

        [HttpPost]
        public SignInDto SignIn([FromBody]LoginDto loginDto)
        {
            var passwordHash = Hash(loginDto.Password);

            var worker =  _workersRepository.GetAll()
                .FirstOrDefault(w => w.Login == loginDto.Login && w.PasswordHash == passwordHash);

            if (worker == null)
            {
                return null;
            }

            var token = GenerateToken(worker.Login, worker.PasswordHash);
            var workerDto = new WorkerDto
            {
                Id = worker.Id.Value,
                FullName = worker.FullName
            };

            TokensList.Add(token.Value, new WorkerTokenRecord { Token = token, Worker = workerDto });

            return new SignInDto
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
        public WorkerDto GetWorkerByToken([FromBody]string tokenValue)
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

        private Token GenerateToken(params string[] strs)
        {
            var seed = Hash(string.Concat(strs));
            var value = GenerateString(seed);

            return new Token
            {
                Value = value,
                ExpiresOn = DateTime.Now.AddSeconds(ExpiresInSeconds),
            };
        }

        private static string GenerateString(string seed)
        {
            var value = seed + DateTime.Now.ToShortDateString()
                + DateTime.Now.ToShortTimeString()
                + DateTime.Now.Millisecond.ToString();

            return Math.Abs(value.GetHashCode()).ToString().PadLeft(10, '5');
        }

        private string Hash(string str)
        {
            return Math.Abs(str.GetHashCode()).ToString();
        }
    }
}
