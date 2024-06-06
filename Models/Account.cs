using Microsoft.AspNetCore.Identity;

namespace HW_11.Models;

public class Account : IdentityUser<int>
{
    public string? FullName { get; set; }

    public virtual Image? Image { get; set; }
}
