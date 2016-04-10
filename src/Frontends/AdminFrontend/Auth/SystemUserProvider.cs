using Domain;
using Domain.Entities;
using Domain.Repositories;
using System.Security.Principal;

namespace AdminFrontend.Auth
{
    public class SystemUserProvider : IPrincipal
    {
        private UserIndentity userIdentity { get; set; }

        public IIdentity Identity
        {
            get
            {
                return userIdentity;
            }
        }

        public SystemUserProvider(string name, IRepository<SystemUser> repository)
        {
            userIdentity = new UserIndentity();
            userIdentity.Init(name, repository);
        }

        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                return false;
            }
            return userIdentity.User.Role.ToString() == role;
        }

        public override string ToString()
        {
            return userIdentity.Name;
        }
    }
}