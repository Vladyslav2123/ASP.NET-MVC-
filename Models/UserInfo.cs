namespace HW_11.Models;

public class UserInfo
{
	public UserInfo()
	{
		Images = new List<Image>();
		UserSkills = new List<UserSkill>();
	}

	public int Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public int PositionId { get; set; }
	public string Description { get; set; } = null!;
	public bool IsActive { get; set; }
	public DateTime Birthday { get; set; }

	public virtual Position Position { get; set; }
	public virtual ICollection<Image> Images { get; set; }
	public virtual ICollection<UserSkill> UserSkills { get; set; }
}