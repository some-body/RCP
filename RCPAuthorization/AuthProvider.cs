using Domain.Entities;
using System;

namespace RCPAuthorization
{
    public class SystemUserAuthProvider
    {
        public SystemUser GetUser(string login, string password)
        {
            return new SystemUser()
            {
                Login = login,
                Role = SystemRole.Teacher
            };
            // TODO: Реализовать.
            throw new NotImplementedException();
        }
    }
}
