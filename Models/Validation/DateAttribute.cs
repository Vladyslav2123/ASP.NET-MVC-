using System.ComponentModel.DataAnnotations;

namespace HW_11.Models.Validation;

public class DateAttribute : ValidationAttribute
{
	public override bool IsValid(object? value)
	{
		DateTime? dateOfBirth = value as DateTime?;
		return dateOfBirth == null || dateOfBirth <= DateTime.Now.Date;
	}
}