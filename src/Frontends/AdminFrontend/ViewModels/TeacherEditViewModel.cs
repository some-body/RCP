using System.ComponentModel.DataAnnotations;

namespace AdminFrontend.ViewModels
{
    public class TeacherEditViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}