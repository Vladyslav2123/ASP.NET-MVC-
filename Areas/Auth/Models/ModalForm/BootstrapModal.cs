namespace HW_11.Areas.Auth.Models.ModalForm;

public enum ModalSize
{
	Small,
	Large,
	Medium
}

public class BootstrapModal
{
	public string? ID { get; set; }
	public string? AreaLabeledId { get; set; }
	public ModalSize? Size { get; set; }
	public string ModalSizeClass
	{
		get
		{
			switch (Size)
			{
				case ModalSize.Small:
					return "modal-sm";
				case ModalSize.Large:
					return "modal-lg";
				case ModalSize.Medium:
				default:
					return "";
			}
		}
	}
}
