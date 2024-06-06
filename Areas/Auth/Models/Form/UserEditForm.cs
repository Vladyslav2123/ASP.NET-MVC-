using HW_11.Models;
using System.ComponentModel.DataAnnotations;

namespace HW_11.Areas.Auth.Models.Form;

public class UserEditForm
{
	private readonly Account _account;

	public UserEditForm() { }

	public UserEditForm(Account account)
	{
		UserRoles = new List<UserRolesForm>();

		_account = account;
		Name = _account.UserName!;
		Email = _account.Email!;
	}

	public string? SrcImage
	{
		get
		{
			if (_account != null && _account.Image != null)
			{
				return _account.Image.GetImage();
			}
			else
			{
				return null;
			}
		}
	}

	[Display(Name = "Ім'я")]
	[Required(ErrorMessage = "обов`зково")]
	public string Name { get; set; } = null!;

	[Display(Name = "Пошта")]
	[EmailAddress]
	[Required(ErrorMessage = "обов`зково")]
	public string Email { get; set; } = null!;

	public List<UserRolesForm> UserRoles { get; set; }

	[Display(Name = "Зображення")]
	public IFormFile? Image { get; set; }
}