using System.ComponentModel.DataAnnotations;

namespace HW_11.Models;

public class MapMarker
{
	public int Id { get; set; }

	[Display(Name = "Назва маркера")]
	public string? Title { get; set; }

	public string Lat { get; set; }
	public string Lng { get; set; }
}