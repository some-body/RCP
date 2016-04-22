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
    public class SystemUsersController : ApiController
    {
        public static IDictionary<string, SystemUserTokenRecord> TokensList { get; private set; } = new Dictionary<string, SystemUserTokenRecord>();

        private const int ExpiresInSeconds = 20000;

        private IRepository<SystemUser> _systemUsersRepository;
        private TokenGenerator _tokenGenerator;
        private HashGenerator _hashGenerator;

        public SystemUsersController()
        {
            _systemUsersRepository = new SystemUsersRepository();
            _tokenGenerator = new TokenGenerator(ExpiresInSeconds);
            _hashGenerator = new HashGenerator();
        }

        [HttpPost]
        public SystemUserSignInDto SignIn([FromBody]LoginDto loginDto)
        {
            var passwordHash = _hashGenerator.Generate(loginDto.Password);

            System.IO.File.AppendAllText("C:/St/log.txt", passwordHash);
            //var temp = _systemUsersRepository.GetAll()
            //    .Select(e => e.Login);

            //foreach (var t in temp)
            //{
            //    System.IO.File.AppendAllText("C:/St/log.txt", t + "\r\n");
            //}

            var user = _systemUsersRepository.GetAll()
                .FirstOrDefault(w => w.Login == loginDto.Login && w.PasswordHash == passwordHash);


            if (user == null)
                return null;

            var token = _tokenGenerator.GenerateToken(user.Login, user.PasswordHash);

            TokensList.Add(token.Value, new SystemUserTokenRecord { Token = token, User = user });

            return new SystemUserSignInDto
            {
                Token = token,
                User = user
            };
        }

        [HttpPost]
        public void SignOut([FromBody]string tokenValue)
        {
            if (TokensList.ContainsKey(tokenValue))
                TokensList.Remove(tokenValue);
        }

        [HttpPost]
        public SystemUser GetByToken([FromBody]string tokenValue)
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

            return tokenRecord.User;
        }
    }
}
