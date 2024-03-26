using Identity.API.Entities.Models;
using Identity.API.Entities.ViewModels;
using Identity.API.Extensions;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Identity.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _store;
        private readonly IEventService _events;
        private readonly SignInManager<AppUser> _signInManager;
        
        public AccountController(IIdentityServerInteractionService interaction, IClientStore store, IEventService events, SignInManager<AppUser> signInManager)
        {
            _interaction = interaction;
            _store = store;
            _events = events;
            _signInManager = signInManager;
        }

        public IActionResult register([FromQuery] string ReturnUrl)
        {
            return View(new ReturnUrlVM() { ReturnUrl = ReturnUrl});
        }

        public IActionResult login([FromQuery]string ReturnUrl)
        {
            return View(new ReturnUrlVM() { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> register([FromForm] AccountInfoVM account)
        {
            await _signInManager.UserManager.CreateAsync(new AppUser()
            {
                UserName = account.username
            },account.password);

            var context = await _interaction.GetAuthorizationContextAsync(account.returnUrl);
            if (context == null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                return View();
            }
            var client = await _store.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client == null)
                return View();
            var user = await _signInManager.UserManager.FindByNameAsync(account.username);
            if (user != null)
            {
                if ((await _signInManager.CheckPasswordSignInAsync(user, account.password, true)).Succeeded)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                    var isuser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName,
                    };
                    await HttpContext.SignInAsync(isuser);
                    if (context != null)
                    {
                        return Redirect(account.returnUrl);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> login([FromForm] AccountInfoVM account)
        {
            var context = await _interaction.GetAuthorizationContextAsync(account.returnUrl);
            if (context == null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                return View();
            }
            var client = await _store.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client == null) 
                return View();
            var user = await _signInManager.UserManager.FindByNameAsync(account.username);
            if (user != null)
            {
                if ((await _signInManager.CheckPasswordSignInAsync(user, account.password, true)).Succeeded)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                    var isuser = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.UserName,
                };
                    await HttpContext.SignInAsync(isuser);
                    if (context != null)
                    {
                        return Redirect(account.returnUrl);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> logout([FromQuery] string logoutId)
        {
            string externalAuthenticationScheme = null;
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            var client = await _store.FindEnabledClientByIdAsync(context.ClientId);

            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignout)
                    {
                        externalAuthenticationScheme = idp;
                    }
                }
            }
            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            return SignOut(new AuthenticationProperties { RedirectUri = client.RedirectUris.First() }, externalAuthenticationScheme);
        }
    }
}
