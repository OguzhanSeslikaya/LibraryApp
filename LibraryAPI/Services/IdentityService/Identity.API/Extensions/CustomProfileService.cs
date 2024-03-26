using Identity.API.Entities.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.API.Extensions
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<AppUser> userManager;
        public CustomProfileService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context.Subject.Identity.Name == null)
                return;
            var user = await userManager.FindByNameAsync(context.Subject.Identity.Name);
            string role = (await userManager.GetRolesAsync(user)).FirstOrDefault();
            if (user != null)
            {
                role = (role=="Admin" ? "admin" : "uye");
                var claims = new List<Claim>
                {
                    new Claim("role", role),
                    new Claim("userName",user.UserName)
                };
                context.AddRequestedClaims(claims);
                context.IssuedClaims = claims;
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
