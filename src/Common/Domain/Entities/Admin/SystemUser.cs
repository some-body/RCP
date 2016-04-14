namespace Domain.Entities
{
    public class SystemUser : Entity
    {
        public string Role { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }
    }
}
