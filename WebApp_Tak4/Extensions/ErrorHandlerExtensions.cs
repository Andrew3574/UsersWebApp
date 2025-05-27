using WebApp_Tak4.Extensions;

namespace WebApp_Task4.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static void ErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
