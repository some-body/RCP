using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdminFrontend.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Поле Текущий пароль обязательно для заполнения")]
        [DisplayName("Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Поле Новый пароль обязательно для заполнения")]
        [DisplayName("Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле Подтверждение обязательно для заполнения")]
        [DisplayName("Подтверждение")]
        public string NewPasswordConfirm { get; set; }
    }
}