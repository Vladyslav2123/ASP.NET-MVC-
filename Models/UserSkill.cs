namespace HW_11.Models;

public class UserSkill
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public int SkillId { get; set; }
	public int ProficiencyLevel { get; set; }

	public virtual UserInfo User { get; set; }
	public virtual Skill Skill { get; set; }
}