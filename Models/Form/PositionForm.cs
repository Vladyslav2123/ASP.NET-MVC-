using System.ComponentModel.DataAnnotations;

namespace HW_11.Models.Form;

public class PositionForm
{

	public PositionForm()
	{
	}

	public PositionForm(Position position)
	{
		Title = position.Title;
	}

	[Display(Name = "Назва позиції")]
	[MaxLength(20, ErrorMessage = "До 20 символів")]
	[MinLength(3, ErrorMessage = "від 3 символів")]
	[Required(ErrorMessage = "Назва позиції обовязкова")]
	public string? Title { get; set; }
}
