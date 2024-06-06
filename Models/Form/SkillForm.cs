using System.ComponentModel.DataAnnotations;

namespace HW_11.Models.Form;

public class SkillForm
{
	private readonly Skill? _skill;

	public string? ImageUrl => _skill?.Images.FirstOrDefault()?.GetImage();

	public SkillForm()
	{
	}

	public SkillForm(Skill skill)
	{
		_skill = skill;
		Name = skill.Name;
	}

	public int? Id => _skill?.Id;

	[Display(Name = "Навички")]
	[Required(ErrorMessage = "Ім`я обов`зково")]
	[MaxLength(20, ErrorMessage = "До 20 символів")]
	[MinLength(3, ErrorMessage = "від 3 символів")]
	public string? Name { get; set; }

	[Display(Name = "Зображення")]
	public List<IFormFile>? Image { get; set; }
}