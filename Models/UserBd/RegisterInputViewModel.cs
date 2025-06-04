using System.ComponentModel.DataAnnotations;

namespace DeLavant_CourseWeb.Models.UserBd
{
    public class RegisterInputViewModel
    {

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Пароль должен быть минимум {2} и максимум {1} символов длиной.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 70, ErrorMessage = "Фамилия может содержать от 2 до 70 символов")]
        public string? UserSurName { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 50, ErrorMessage = "Имя может содержать от 2 до 50 символов")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 70, ErrorMessage = "Отчество может содержать от 2 до 70 символов")]
        public string? UserFatherName { get; set; }
        public int? SelectedAreaId { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public List<int?> SelectedPostIds { get; set; }
    }
}
