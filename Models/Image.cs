namespace HW_11.Models;

public class Image
{
	public int Id { get; set; }
	public string? FileName { get; set; }

	public int? UserId { get; set; }
	public virtual UserInfo? User { get; set; }

	public int? SkillId { get; set; }
	public virtual Skill? Skill { get; set; }

	public string GetImage() => "/uploads/" + FileName;
}