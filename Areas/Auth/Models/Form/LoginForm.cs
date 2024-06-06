using System.ComponentModel.DataAnnotations;

namespace HW_11.Areas.Auth.Models.Form;

public class LoginForm
{
    [Display(Name = "Логін")]
    [Required(ErrorMessage = "обов`зково")]
    public string Login { get; set; } = null!;

    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "обов`зково")]
    public string Password { get; set; } = null!;
}