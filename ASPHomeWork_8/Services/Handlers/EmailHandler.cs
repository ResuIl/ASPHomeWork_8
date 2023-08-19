using ASPHomeWork_8.Services.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASPHomeWork_8.Services.Handlers;

public class EmailHandler : AuthorizationHandler<EmailRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext? context, EmailRequirement? requirement)
    {
        if (context!.User.HasClaim(claim => claim.Type == ClaimTypes.Email))
        {
            if (context!.User.FindFirst(claim => claim.Type == ClaimTypes.Email).Value.EndsWith(requirement.Domain))
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
