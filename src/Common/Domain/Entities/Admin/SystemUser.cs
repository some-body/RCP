namespace Domain.Entities
{
    public enum SystemRole { Teacher = 0, Admin = 1 };

    class SystemUser : Entity
    {
        public SystemRole Role { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }
    }
}
