using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp_Task4.Repositories;

namespace WebApp_Tak4.Services.Auth
{
    public class JwtAuthAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            await ValidateClaims(token, context);            

            await next();
        }

        private async Task ValidateClaims(string token, ActionExecutingContext context)
        {
            var claims = JwtAuthenticationService.ValidateToken(token);

            if (claims == null || !claims.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
            var email = claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                context.Result = new UnauthorizedObjectResult("Invalid token: Email not found.");
                return;
            }
            await ValidateUser(email,context);
            
        }

        private async Task ValidateUser(string email,ActionExecutingContext context)
        {
            var userRepository = context.HttpContext.RequestServices.GetService<UserRepository>();
            if (userRepository == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }
            var user = await userRepository.GetByEmail(email);
            if (user == null)
            {
                context.Result = new UnauthorizedObjectResult("User not found.");
                return;
            }
            if (user.UserState == Models.UserState.blocked)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }
        }
    }
}
