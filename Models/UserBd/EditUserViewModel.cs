using System.ComponentModel.DataAnnotations;

namespace DeLavant_CourseWeb.Models.UserBd
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Неверный формат почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 70, ErrorMessage = "Фамилия может содержать от 2 до 70 символов")]
        public string? UserSurName { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 50, ErrorMessage = "Имя может содержать от 2 до 50 символов")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 50, ErrorMessage = "Имя пользователя может содержать от 2 до 50 символов")]
        [Display(Name = "Логин")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Length(2, 70, ErrorMessage = "Отчество может содержать от 2 до 70 символов")]
        public string? UserFatherName { get; set; }
        public int? SelectedAreaId { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public List<int?> SelectedPostIds { get; set; }
    }
}
