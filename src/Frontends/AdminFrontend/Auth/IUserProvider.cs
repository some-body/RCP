using Domain.Entities;

namespace AdminFrontend.Auth
{
    interface IUserProvider
    {
        SystemUser User { get; set; }
    }
}
