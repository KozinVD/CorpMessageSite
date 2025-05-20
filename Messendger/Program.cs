using Messendger.Classes;
using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Elfie.Extensions;
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
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 104857600; // 100 MB
            });
            var app = builder.Build();
            app.UseHsts();
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
            app.MapPost("/editprofile",[Authorize]  async (HttpRequest request, MessendgerDb db, HttpContext context) =>
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

            //Конечная точка для отправления заявки в друзья
            app.MapGet("api/addFr/{id}",[Authorize] async (string id, MessendgerDb db, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var checkFriend = db.Friends.Where(x => x.IdUser == userId && x.IdFriend == id).FirstOrDefault();
                Console.WriteLine(checkFriend);
                if (checkFriend != null)
                    return Results.Ok();
                Console.WriteLine("Здесь!");
                var userReq = await db.FriendRequests.Where(x => x.IdSender == userId).ToListAsync();
                foreach (var user in userReq)
                {
                    if (user.IdSender == userId && user.IdRecipient == id)
                        return Results.Ok();
                }
                Console.WriteLine("Здесь!");
                var req = db.FriendRequests.Where(x => x.IdSender == id && x.IdRecipient == userId).FirstOrDefault();
                if (req != null)
                {
                    Friend newFriend = new Friend();
                    newFriend.IdUser = id;
                    newFriend.IdFriend = userId;
                    await db.Friends.AddAsync(newFriend);
                    await db.SaveChangesAsync();
                    Friend newFriend2 = new Friend();
                    newFriend2.IdFriend = id;
                    newFriend2.IdUser = userId;
                    await db.Friends.AddAsync(newFriend2);
                    db.FriendRequests.Remove(req);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Здесь!");
                    return Results.Ok();
                }

                FriendRequest request = new FriendRequest()
                {
                    IdSender = userId,
                    IdRecipient = id,
                    DateSend = DateOnly.FromDateTime(DateTime.Now)
                };
                await db.FriendRequests.AddAsync(request);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            //Получения друзей
            app.MapGet("/api/getFriend", [Authorize] async (HttpContext context, MessendgerDb db) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userChats = await db.ChatParticipants.Include(x=> x.IdChatNavigation).ThenInclude(x =>x.ChatParticipants)
                .Where(x => x.IdUser == userId).Select(x => x.IdChatNavigation).Where(x => !x.IsGroup).ToListAsync();
                var userFriends = await db.Friends.Include(f => f.IdFriendNavigation).ThenInclude(x => x.UserInfo).ThenInclude(x => x.IdJobNavigation)
                .Include(f => f.IdFriendNavigation).ThenInclude(x => x.IdPhotoNavigation)
                .Where(x => x.IdUser == userId).Select(u => new
                {
                    Id = u.IdFriend,
                    Surname = u.IdFriendNavigation.UserInfo.Surname,
                    Name = u.IdFriendNavigation.UserInfo.Name,
                    Lastname = u.IdFriendNavigation.UserInfo.Lastname,
                    JobName = u.IdFriendNavigation.UserInfo.IdJobNavigation.Name,
                    PhotoPath = u.IdFriendNavigation.IdPhotoNavigation != null ? $"/userImages/{u.IdFriendNavigation.IdPhotoNavigation.Name}" : "/res/image/User.png",
                    IdChat = GetChatId(userChats, u.IdFriend)
                }).ToListAsync();
                if (userFriends.Count == 0)
                    return Results.NotFound();
                return Results.Ok(userFriends);
            });

            //Получение входящих заявок
            app.MapGet("/api/getreqv",[Authorize] async (HttpContext context, MessendgerDb db) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userChats = await db.ChatParticipants.Include(x => x.IdChatNavigation).ThenInclude(x => x.ChatParticipants).Where(x => x.IdUser == userId).Select(x => x.IdChatNavigation).Where(x => !x.IsGroup).ToListAsync();
                var userReqTo = await db.FriendRequests.Include(f => f.IdSenderNavigation).ThenInclude(x => x.UserInfo).ThenInclude(x => x.IdJobNavigation)
                .Include(f => f.IdSenderNavigation).ThenInclude(x => x.IdPhotoNavigation)
                .Where(x => x.IdRecipient == userId).Select(f => new
                {
                    Id = f.IdSender,
                    Surname = f.IdSenderNavigation.UserInfo.Surname,
                    Name = f.IdSenderNavigation.UserInfo.Name,
                    Lastname = f.IdSenderNavigation.UserInfo.Lastname,
                    JobName = f.IdSenderNavigation.UserInfo.IdJobNavigation.Name,
                    PhotoPath = f.IdSenderNavigation.IdPhotoNavigation != null?$"/userImages/{f.IdSenderNavigation.IdPhotoNavigation.Name}":"/res/image/User.png",
                    IdChat = GetChatId(userChats, f.IdSender)
                }).ToListAsync();
                if (userReqTo.Count == 0)
                    return Results.NotFound();
                return Results.Ok(userReqTo);
            });
            //Получение исходящих заявок
            app.MapGet("/api/getreqi", [Authorize] async (HttpContext context, MessendgerDb db) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userChats = await db.ChatParticipants.Include(x => x.IdChatNavigation).ThenInclude(x => x.ChatParticipants).Where(x => x.IdUser == userId).Select(x => x.IdChatNavigation).Where(x => !x.IsGroup).ToListAsync();
                var userReqTo = await db.FriendRequests.Include(f => f.IdRecipientNavigation).ThenInclude(x => x.UserInfo).ThenInclude(x => x.IdJobNavigation)
                .Include(f => f.IdRecipientNavigation).ThenInclude(x => x.IdPhotoNavigation)
                .Where(x => x.IdSender == userId).Select(f => new
                {
                    Id = f.IdRecipient,
                    Surname = f.IdRecipientNavigation.UserInfo.Surname,
                    Name = f.IdRecipientNavigation.UserInfo.Name,
                    Lastname = f.IdRecipientNavigation.UserInfo.Lastname,
                    JobName = f.IdRecipientNavigation.UserInfo.IdJobNavigation.Name,
                    PhotoPath = f.IdRecipientNavigation.IdPhotoNavigation != null ? $"/userImages/{f.IdRecipientNavigation.IdPhotoNavigation.Name}" : "/res/image/User.png",
                    IdChat = GetChatId(userChats, f.IdRecipient)
                }).ToListAsync();
                if (userReqTo.Count == 0)
                    return Results.NotFound();
                return Results.Ok(userReqTo);
            });

            //Конечная точка для удаления друзей
            app.MapGet("/api/delFriend/{id}",[Authorize] async (string id, MessendgerDb db, HttpContext context) =>
            {
                Console.WriteLine(id);
                Console.WriteLine();
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.Friends.Remove(await db.Friends.Where(x => x.IdUser == userId && x.IdFriend == id).FirstAsync());
                db.Friends.Remove(await db.Friends.Where(x => x.IdUser == id && x.IdFriend == userId).FirstAsync());
                await db.SaveChangesAsync();
                Results.Ok();
            });

            //Конечная точка для удаления входящего запроса
            app.MapGet("/api/delReqV/{id}", [Authorize] async (string id, MessendgerDb db, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.FriendRequests.Remove(await db.FriendRequests.Where(x => x.IdSender == id && x.IdRecipient == userId).FirstAsync());
                await db.SaveChangesAsync();
                Results.Ok();
            });

            //Конечная точка для удаления исходящего запроса
            app.MapGet("/api/delReqI/{id}", [Authorize] async (string id, MessendgerDb db, HttpContext context) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                db.FriendRequests.Remove(await db.FriendRequests.Where(x => x.IdSender == userId && x.IdRecipient == id).FirstAsync());
                await db.SaveChangesAsync();
                Results.Ok();
            });

            app.MapPost("/sendFiles/{chatId}", [Authorize] async (int chatId ,HttpRequest request, MessendgerDb db, HttpContext context, IHubContext<ChatHub> hubContext) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var form = await request.ReadFormAsync();

                UserInfo curUser = await db.UserInfos.Include(x => x.IdNavigation).ThenInclude(x => x.IdPhotoNavigation).Where(x => x.Id == userId).FirstAsync();

                var file = form.Files["fileSend"];
                var textMess = form["textMessage"];
                Console.WriteLine();
                Console.WriteLine("Вызов");
                Console.WriteLine(file);
                Console.WriteLine(textMess);
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "userFile");
                    Directory.CreateDirectory(path);
                    string guid = Guid.NewGuid().ToString();
                    var filepath = Path.Combine(path, guid + file.FileName);
                    using var stream = new FileStream(filepath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    string mimeType = file.ContentType;
                    string text = $"<a href=\"/userFile/{guid + file.FileName}\" class=\"link-warning link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover\">{file.FileName}</a><p>{textMess}</p>";
                    if (mimeType.StartsWith("image/"))
                        text = $"<img src=\"/userFile/{guid + file.FileName}\" class=\"p-1\" width=\"100%\"/><p>{textMess}</p>";
                    if (mimeType.StartsWith("video/"))
                        text = $"<video src=\"/userFile/{guid +file.FileName}\" width=\"100%\" controls class=\"p-1\" preload=\"metadata\"></video><p>{textMess}</p>";
                    if (mimeType.StartsWith("audio/"))
                        text = $"<div style=\"overflow: hidden;\"><audio src=\"/userFile/{ guid + file.FileName}\" class=\"mt-1\" controls preload=\"metadata\"></audio></div><p>{textMess}</p>";
                    

                    Message message = new Message() {
                    Text = text,
                    TimeSend = DateTime.Now,
                    IdUser = userId,
                    IdChat = chatId
                    };
                    await db.Messages.AddAsync(message);
                    await db.SaveChangesAsync();
                    Entities.File newFile = new Entities.File() {
                        Name = guid + file.Name,
                        IdMessage = message.Id
                    };
                    await db.Files.AddAsync(newFile);
                    await db.SaveChangesAsync();

                    UserInfo senderInfo = await db.UserInfos.FindAsync(userId);
                    string sender = senderInfo.Surname + " " + senderInfo.Name;
                    Chat chat = await db.Chats.Include(x => x.ChatParticipants).Where(x => x.Id == message.IdChat).FirstAsync();
                    await hubContext.Clients.Users(chat.ChatParticipants.Where(p => p.IdUser != userId).Select(p => p.IdUser).ToList()).SendAsync("Receive", chat.Id, sender, message.Text, message.TimeSend.Hour + ":" + message.TimeSend.Minute + " " + message.TimeSend.Date);
                    await hubContext.Clients.Users(userId).SendAsync("ReceiveFileY", chat.Id, sender, message.Text, message.TimeSend.Hour + ":" + message.TimeSend.Minute + " " + message.TimeSend.Date);

                    return Results.Ok();
                }
                return Results.NotFound();
            });

            //Конечная точка для получения участников чата
            app.MapGet("/chatPart/{id}",[Authorize] async (int id, MessendgerDb db) =>
            {
                var part = await db.ChatParticipants
                .Include(x => x.IdUserNavigation).ThenInclude(p => p.IdPhotoNavigation)
                .Include(x => x.IdUserNavigation).ThenInclude(p => p.UserInfo).ThenInclude(p => p.IdJobNavigation).Where(x => x.IdChat == id).Select(p => new
                {
                    Id = p.IdUserNavigation.UserInfo.Id,
                    Surname = p.IdUserNavigation.UserInfo.Surname,
                    Name = p.IdUserNavigation.UserInfo.Name,
                    Lastname = p.IdUserNavigation.UserInfo.Lastname,
                    PhotoPath = itemSearch.GetPath(p.IdUserNavigation.IdPhotoNavigation),
                    JobName = p.IdUserNavigation.UserInfo.IdJobNavigation.Name
                }).ToListAsync();
                if (part.Count == 0)
                    return Results.NotFound();
                return Results.Ok(part);
            });

            //Конечная точка для получения людей не присутствующих в чате
            app.MapGet("/newChatPart/{id}", [Authorize] async (int id, MessendgerDb db, HttpContext context) =>
            {
                Chat curChat = await db.Chats
            .Include(c => c.ChatParticipants).ThenInclude(p => p.IdUserNavigation).ThenInclude(p => p.UserInfo)
            .Where(c => c.Id == id).FirstAsync();
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<UserInfo> chatPart = curChat.ChatParticipants.Select(p => p.IdUserNavigation.UserInfo).ToList();
            List<UserInfo> friends = await db.Friends.Include(x => x.IdFriendNavigation).Select(f => f.IdFriendNavigation.UserInfo).ToListAsync();
            friends.RemoveAll(f => chatPart.Contains(f));
            if (friends.Count == 0)
                return Results.NotFound();
                List<UserInfo> list = new List<UserInfo>();
            foreach (var f in friends)
            {
                    list.Add(await db.UserInfos.Include(u => u.IdNavigation).ThenInclude(u => u.IdPhotoNavigation)
                        .Include(u => u.IdJobNavigation)
                        .Where(x => x.Id == f.Id).FirstAsync());
            }
            
                return Results.Ok(list.Select(f => new
                {
                    Id = f.Id,
                    FullName = f.FullName,
                    Path = itemSearch.GetPath(f.IdNavigation.IdPhotoNavigation),
                    JobName = f.IdJobNavigation.Name
                }).ToList().Distinct());
            });

            //Конечная точка для Чата
            app.MapHub<ChatHub>("/chat");


            string url = builder.Configuration.GetConnectionString("Url");
            app.Urls.Add(url);
            app.Run();
            //while (true)
            //{
            //    Console.Write(">");
            //    var input = Console.ReadLine();
            //}
        }
        public static int GetChatId(List<Chat> chats, string id)
        {
            foreach (var chat in chats)
            {
                if (chat.ChatParticipants.Select(x => x.IdUser).Contains(id))
                    return chat.Id; 
            }
            return 0;
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