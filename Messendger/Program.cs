using Messendger.Classes;
using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            
            //Поиск сообщений по Id чата и отправка клиенту
            app.MapGet("/api/message/{id}",[Authorize] async (int id, MessendgerDb db, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Results.Ok(await Message_actions.GetMessage(id, db, userId));
            });

            //Поиск людей по ФИО
            app.MapGet("api/users/{search}",[Authorize] async (string search, MessendgerDb db, HttpContext context) =>
            {
                var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<Chat> userChats = await db.ChatParticipants.Include(y => y.IdChatNavigation).ThenInclude(x => x.ChatParticipants).Where(x => x.IdUser == id).Select(p => p.IdChatNavigation).Where(c => c!=null &&!c.IsGroup).ToListAsync();
                Console.WriteLine(userChats.Count);
                foreach (var item in userChats)
                {
                    Console.WriteLine($"{item.Id},{item.Name}, {userChats.Count}");
                }
                List<itemSearch> searchList = await db.UserInfos.Include(x => x.IdJobNavigation).Include(x => x.IdNavigation).ThenInclude(x => x.IdPhotoNavigation).Where(u => u.Id != id).Where(x => x.Surname.ToLower().Contains(search.ToLower())
                || x.Name.ToLower().Contains(search.ToLower())
                || x.Lastname.ToLower().Contains(search.ToLower())).Select(u => new itemSearch {Id = u.Id ,FullName = u.FullName,
                    JobName = u.IdJobNavigation.Name,
                    IdChat = 0, IsFriend = false, 
                    Path = itemSearch.GetPath(u.IdNavigation.IdPhotoNavigation)
                }) .ToListAsync();
                if (searchList.Count == 0)
                    return Results.NotFound();

                foreach (var item in userChats)
                {
                    foreach (var chat in item.ChatParticipants)
                    {
                        foreach (var itemsearch in searchList)
                        {
                            if (itemsearch.Id == chat.IdUser)
                                itemsearch.IdChat = chat.IdChat;
                        }
                    }
                }
                return Results.Ok(searchList);
            });
            app.MapGet("/test", () =>
            {
                int a = 4;
                a += 4;
                return a;
            });
            //Добавление роли
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

            //Конечная точна для изменения профиля
            app.MapPost("/editprofile", async (HttpRequest request, MessendgerDb db, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var form = await request.ReadFormAsync();

                UserInfo curUser = await db.UserInfos.Include(x => x.IdNavigation).ThenInclude(x => x.IdPhotoNavigation).Where(x => x.Id == userId).FirstAsync();

                var file = form.Files["formFile"];
                var surname = form["surname"];
                var name = form["name"];
                var lastname = form["lastname"];
                curUser.Surname = surname;
                curUser.Name = name;
                curUser.Lastname = lastname;
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userImages");
                    Directory.CreateDirectory(path);
                    var filepath = Path.Combine(path, file.FileName);
                    using var stream = new FileStream(filepath, FileMode.Create);

                    await file.CopyToAsync(stream);
                    bool isNew = false;
                    UserImage image = curUser.IdNavigation.IdPhotoNavigation;
                    if (image == null)
                    {
                        image = new UserImage();
                        isNew = true;
                    }
                    image.Name = file.FileName;
                    if (isNew)
                    {
                        await db.AddAsync(image);
                        await db.SaveChangesAsync();
                    }
                    curUser.IdNavigation.IdPhoto = image.Id;
                    await db.SaveChangesAsync();
                }
                await db.SaveChangesAsync();
                return Results.Redirect("/ChatMenu");
            });

            //Конечная точка для Чата
            app.MapHub<ChatHub>("/chat");


            //string url = builder.Configuration.GetConnectionString("Url");
            //app.Urls.Add(url);
            app.Run();
        }
    }
    public class itemSearch
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string JobName { get; set; }
        public int IdChat { get; set; }
        public bool IsFriend { get; set; }
        public string Path {get; set; }
        public static string GetPath(UserImage image)
        {
            if (image == null)
                return "/res/image/User.png";
            return $"/userImages/{image.Name}";
        }
    }
}