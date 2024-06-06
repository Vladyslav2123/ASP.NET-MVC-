using System.ComponentModel.DataAnnotations;

namespace HW_11.Models.Form;

public class UserSkillForm
{
	private readonly UserSkill _skill;

	public UserSkillForm() { }

	public UserSkillForm(UserSkill skill)
	{
		_skill = skill;
		UserId = skill.UserId;
		SkillId = skill.SkillId;
		ProficiencyLevel = skill.ProficiencyLevel;
	}

	public int UserId { get; set; }

	[Display(Name = "Навичка")]
	public int SkillId { get; set; }

	[Display(Name = "Значення навички")]
	[Required(ErrorMessage = "Ім`я обов`зково")]
	[Range(0, 100, ErrorMessage = "Значення повинно бути між 0 та 100")]
	public int ProficiencyLevel { get; set; }

}
