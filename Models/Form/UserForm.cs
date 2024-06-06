using HW_11.Models.Validation;
using System.ComponentModel.DataAnnotations;

namespace HW_11.Models.Form;

public class UserForm
{
    private readonly UserInfo _user;
    public List<string>? SrcImage => _user?.Images.Select(x => x.GetImage()).ToList();
    public int? UserId => _user?.Id;

    public UserForm()
    {
    }

    public UserForm(UserInfo info)
    {
        _user = info;
        FirstName = info.FirstName;
        LastName = info.LastName;
        Description = info.Description;
        IsActive = info.IsActive;
        Birthday = info.Birthday;
        PositionId = info.PositionId;
    }

    [Display(Name = "Імя")]
    [Required(ErrorMessage = "Ім`я обов`зково")]
    [MaxLength(20, ErrorMessage = "До 20 символів")]
    [MinLength(3, ErrorMessage = "від 3 символів")]
    public string FirstName { get; set; }

    [Display(Name = "Прізвище")]
    [MaxLength(20, ErrorMessage = "До 20 символів")]
    [MinLength(3, ErrorMessage = "від 3 символів")]
    [Required(ErrorMessage = "Прізвище обов`зково")]
    public string LastName { get; set; }

    [Display(Name = "Позиція")]
    public int PositionId { get; set; }

    [Display(Name = "Опис")]
    [MaxLength(200, ErrorMessage = "До 200 символів")]
    [MinLength(10, ErrorMessage = "від 10 символів")]
    [Required(ErrorMessage = "Опис обов`зково")]
    public string Description { get; set; }

    [Display(Name = "Активнісь")]
    public bool IsActive { get; set; }

    [Display(Name = "День народження")]
    [DateAttribute(ErrorMessage = "Дата народження не може бути в майбутньому")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "Зображення")]
    public List<IFormFile>? Image { get; set; }
}