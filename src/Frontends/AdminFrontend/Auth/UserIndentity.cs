using Domain;
using Domain.Entities;
using Domain.Repositories;
using System.Linq;
using System.Security.Principal;

namespace AdminFrontend.Auth
{
    public class UserIndentity : IIdentity, IUserProvider
    {
        public SystemUser User { get; set; }

        public string AuthenticationType
        {
            get
            {
                return typeof(SystemUser).ToString();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return User != null;
            }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Login;
                }
                //иначе аноним
                return "anonym";
            }
        }

        public void Init(string login, IRepository<SystemUser> repository)
        {
            if (!string.IsNullOrEmpty(login))
            {
                User = repository.GetAll().FirstOrDefault(e => e.Login == login);
            }
        }
    }
}