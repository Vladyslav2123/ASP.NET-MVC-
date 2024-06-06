using System.ComponentModel.DataAnnotations;

namespace HW_11.Models;

public class Skill
{
	public Skill()
	{
		Images = new List<Image>();
		UserSkills = new List<UserSkill>();
	}

	public int Id { get; set; }

	[Display(Name = "Навичка")]
	public string Name { get; set; }

	public virtual ICollection<Image> Images { get; set; }
	public virtual ICollection<UserSkill> UserSkills { get; set; }
}
