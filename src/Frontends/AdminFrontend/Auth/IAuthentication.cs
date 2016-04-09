using Domain.Entities;
using System.Security.Principal;
using System.Web;

namespace AdminFrontend.Auth
{
    interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        SystemUser Login(string login, string password, bool isPersistent);

        SystemUser Login(string login);

        void LogOut();

        IPrincipal CurrentUser { get; }
    }
}
