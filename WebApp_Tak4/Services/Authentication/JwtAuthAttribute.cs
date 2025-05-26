using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp_Tak4.Models;

namespace WebApp_Tak4.Services.Auth
{
    public class JwtAuthAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var token = request.Cookies["jwt"];

            if (token != null)
            {
                var claims = JwtAuthenticationService.ValidateToken(token);
                ValidateClaims(context, claims);                           
            }
            else
            {                
                context.Result = new RedirectToActionResult("Login","Account", null);
            }

            base.OnActionExecuted(context);
        }
        
        private static void ValidateClaims(ActionExecutedContext context, ClaimsPrincipal claims)
        {
            if (string.IsNullOrEmpty(claims.Claims.First(claim => claim.Type == "jku").Value))
            {
                context.Result = new UnauthorizedObjectResult("You need to authorize to access this page");
            }
            string userState = claims.Claims.First(claim => claim.Type == "UserState").Value;
            if (userState != null && userState == "blocked")
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }

    
}
