using System.ComponentModel.DataAnnotations;

namespace WorkersFrontend.ViewModels
{
    public class AuthViewModel
    {
        public string ReturnToUrl { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        public string Password { get; set; }
    }
}