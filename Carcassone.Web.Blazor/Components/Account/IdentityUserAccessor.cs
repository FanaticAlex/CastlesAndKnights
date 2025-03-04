using Carcassone.Web.Blazor.Data;
using Microsoft.AspNetCore.Identity;

namespace Carcassone.Web.Blazor.Components.Account
{
    internal sealed class IdentityUserAccessor(UserManager<CarcassoneUser> userManager, IdentityRedirectManager redirectManager)
    {
        public async Task<CarcassoneUser> GetRequiredUserAsync(HttpContext context)
        {
            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
            {
                redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
            }

            return user;
        }
    }
}
