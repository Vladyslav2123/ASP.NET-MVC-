using HW_11.Models;
using System.ComponentModel.DataAnnotations;

namespace HW_11.Areas.Auth.Models.Form;

public class UserCreateForm
{
	public UserCreateForm()
	{
		UserRoles = new List<UserRolesForm>();
		MapMarker = new MapMarker();
	}

	[Display(Name = "Ім'я")]
	[Required(ErrorMessage = "Будь ласка, введіть ім'я")]
	public string Name { get; set; } = null!;

	[Display(Name = "Пошта")]
	[Required(ErrorMessage = "Будь ласка, введіть адресу електронної пошти")]
	[EmailAddress(ErrorMessage = "Будь ласка, введіть коректну адресу електронної пошти")]
	public string Email { get; set; } = null!;

	[Display(Name = "Пароль")]
	[Required(ErrorMessage = "обов`зково")]
	public string Password { get; set; } = null!;

	[Display(Name = "Підтвердити пароль")]
	[Required(ErrorMessage = "обов`зково")]
	[Compare("Password", ErrorMessage = "Паролі не співпадають")]
	public string ConfirmPassword { get; set; } = null!;

	public List<UserRolesForm> UserRoles { get; set; }

	[Display(Name = "Зображення")]
	public IFormFile? Image { get; set; }

	public MapMarker MapMarker { get; set; }
}