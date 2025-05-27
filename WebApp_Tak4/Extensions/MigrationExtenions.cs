using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WebApp_Tak4.Data;

namespace WebApp_Task4.Extensions
{
    public static class MigrationExtenions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using Task4DbContext dbContext = scope.ServiceProvider.GetRequiredService<Task4DbContext>();
            dbContext.Database.Migrate();
        }
    }
}
