namespace HW_11.Models;

public class Position
{
	public Position()
	{
		Users = new List<UserInfo>();
	}

	public int Id { get; set; }
	public string? Title { get; set; }

	public virtual ICollection<UserInfo> Users { get; set; }
}
