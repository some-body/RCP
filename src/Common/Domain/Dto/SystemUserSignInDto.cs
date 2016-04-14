using Domain.Entities;

namespace Domain.Dto
{
    public class SystemUserSignInDto
    {
        public Token Token { get; set; }

        public SystemUser User { get; set; }
    }
}
