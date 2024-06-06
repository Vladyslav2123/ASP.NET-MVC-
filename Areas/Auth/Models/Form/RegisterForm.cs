using System.ComponentModel.DataAnnotations;

namespace HW_11.Areas.Auth.Models.Form;

public class RegisterForm
{
	[Display(Name = "Ім'я")]
	[Required(ErrorMessage = "обов`зково")]
	public string Name { get; set; } = null!;

	[Display(Name = "Пошта")]
	[Required(ErrorMessage = "обов`зково")]
	public string Login { get; set; } = null!;

	[Display(Name = "Пароль")]
	[Required(ErrorMessage = "обов`зково")]
	public string Password { get; set; } = null!;

	[Display(Name = "Підтвердити пароль")]
	[Required(ErrorMessage = "обов`зково")]
	[Compare("Password", ErrorMessage = "Паролі не співпадають")]
	public string ConfirmPassword { get; set; } = null!;
}