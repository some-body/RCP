using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
