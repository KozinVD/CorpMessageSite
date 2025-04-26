using Messendger.Classes;
using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
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
            builder.Services.AddSignalR();
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
            
            app.MapGet("/newRole/{nameRole}", [Authorize] async (string nameRole, RoleManager<IdentityRole> roleManager) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(nameRole))
                        return "Ты тупой?";
                    await roleManager.CreateAsync(new IdentityRole() { Name = nameRole, NormalizedName = nameRole });
                    return "Готово!";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка!");
                    Console.WriteLine($"{ex.Message}");
                    return "ГГ!";
                }
            });
            app.MapHub<ChatHub>("/chat");
            //string url = builder.Configuration.GetConnectionString("Url");
            //app.Urls.Add(url);
            app.Run();
        }
    }
}