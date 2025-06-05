using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeLavant_CourseWeb.Models.UserBd
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [MinLength(3, ErrorMessage = "Участок должен содержать больше 3 символов.")]
        [Display(Name = "Участок")]
        public string? NameArea { get; set; }

        public List<User>? Users { get; set; }
    }   
}
