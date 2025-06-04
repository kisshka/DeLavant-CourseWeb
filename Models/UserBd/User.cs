using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DeLavant_CourseWeb.Models.UserBd;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{

    [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
    [Length(5, 70, ErrorMessage = "Фамилия может содержать от 5 до 70 символов")]
    [Display(Name = "Фамилия")]
    public string? UserSurName { get; set; }


    [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
    [Length(5, 50, ErrorMessage = "Имя может содержать от 2 до 50 символов")]
    [Display(Name = "Имя")]
    public string? Name { get; set; }


    [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
    [Length(5, 70, ErrorMessage = "Отчество может содержать от 5 до 70 символов")]
    [Display(Name = "Отчество")]
    public string? UserFatherName { get; set; }
   
  
    public List<Post>? Posts { get; set; }

   
    public int? IdArea { get; set; }

    [ForeignKey(nameof(IdArea))]
    public Area? Areas { get; set; }
}