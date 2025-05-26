namespace WebApp_Tak4.Extensions
{
    public static class ErrorHandlerExtensions
    {
        public static void ErrorHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
