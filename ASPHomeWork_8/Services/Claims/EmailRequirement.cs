using Microsoft.AspNetCore.Authorization;

namespace ASPHomeWork_8.Services.Claims;

public class EmailRequirement : IAuthorizationRequirement
{
    public string? Domain { get; set; }

    public EmailRequirement(string? domain)
    {
        Domain = domain;
    }
}
