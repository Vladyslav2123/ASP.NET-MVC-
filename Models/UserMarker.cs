namespace HW_11.Models;

public class UserMarker
{
	public int Id { get; set; }

	public int AccountId { get; set; }
	public int MapMarkerId { get; set; }

	public virtual Account? Account { get; set; }
	public virtual MapMarker? MapMarker { get; set; }
}
