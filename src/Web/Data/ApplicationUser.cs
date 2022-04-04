using Microsoft.AspNetCore.Identity;

namespace Web.Data;

public class ApplicationUser : IdentityUser
{
    public ICollection<Advertisement>? Advertisements { get; set; }
}
