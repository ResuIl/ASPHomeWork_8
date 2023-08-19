using Microsoft.AspNetCore.Identity;

namespace ASPHomeWork_8.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public int Year { get; set; }
}
