using System.ComponentModel.DataAnnotations;

namespace DeLavant_CourseWeb.Models.UserBd
{
    public class Post
    {
        [Key]
        public int IdPost { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [MinLength(3, ErrorMessage = "Должность должна содержать больше 3 символов.")]
        [Display(Name = "Должность")]
        public string? Title { get; set; }

        public List<User>? Users { get; set; }
    }
}
