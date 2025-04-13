using Messendger.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Messendger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MessendgerDb>(options => options.UseSqlServer(connection));
            builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MessendgerDb>();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapGet("/db", (MessendgerDb db) =>
            {
                return db.Users;
            });
            //string url = builder.Configuration.GetConnectionString("Url");
            //app.Urls.Add(url);
            app.Run();
        }
    }
}
