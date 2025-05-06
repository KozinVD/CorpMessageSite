using Messendger.Classes;
using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace Messendger.Pages
{
    [Authorize]
    public class ChatMenuModel : PageModel
    {
        public MessendgerDb db;
        public ChatMenuModel(MessendgerDb db)
        {
            this.db = db;
        }
        public Chat? curChat { get; set; } = null;
        public int IdCurChat { get; private set; } = 0;
        public UserInfo info { get; set; }
        public List<ViewModelChat> Chats { get; set; } = [];
        public List<itemMessage> Messages { get; set; } = [];
        public List<ViewModelFriend> Friends { get; set; } = [];

        public async Task OnGetAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (RouteData.Values["idCurChat"] != null && int.TryParse(RouteData.Values["idCurChat"].ToString(), out int idChat))
            {
                IdCurChat = idChat;
            }
            else
            {
                IdCurChat = 0;
            }
            if (IdCurChat != 0)
            {
                Messages = await Message_actions.GetMessage(IdCurChat, db, userId);
                if (await db.Chats.FindAsync(IdCurChat) != null)
                curChat = await db.Chats.Include(x => x.ChatParticipants).ThenInclude(x =>x.IdUserNavigation).ThenInclude(x => x.UserInfo).Where(x => x.Id == IdCurChat).FirstAsync();
            }
            info = await db.UserInfos.Include(u => u.IdNavigation).ThenInclude(u => u.IdPhotoNavigation).Where(x => x.IdNavigation.UserName == User.Identity.Name).FirstOrDefaultAsync();
            Chats = await db.ChatParticipants.Include(y => y.IdChatNavigation).ThenInclude(c => c.ChatParticipants).ThenInclude(c => c.IdUserNavigation)
                .ThenInclude(c => c.IdPhotoNavigation)
                .Include(x => x.IdUserNavigation).ThenInclude(x => x.IdPhotoNavigation)
            .Where(x => x.IdUser == info.Id)
            .Select(chat =>
                new ViewModelChat
                {
                    Id = chat.IdChat,
                    NameChat = chat.IdChatNavigation.Name,
                    Participants = chat.IdChatNavigation.ChatParticipants.Where(x => x.IdUser != info.Id).Select(p => p.IdUserNavigation.UserInfo.Surname + " " + p.IdUserNavigation.UserInfo.Name).ToList(),
                    isGroup = chat.IdChatNavigation.IsGroup,
                    PhotoPath = ViewModelChat.GetPath(chat.IdChatNavigation, userId)
                }).ToListAsync();
            Friends = await db.Friends.Include(f => f.IdFriendNavigation).ThenInclude(u => u.UserInfo).ThenInclude(u => u.IdJobNavigation)
    .Include(f => f.IdFriendNavigation).ThenInclude(u => u.IdPhotoNavigation)
    .Include(f => f.IdFriendNavigation).ThenInclude(u => u.ChatParticipants)
    .Where(f => f.IdUser == userId).Select(f => new ViewModelFriend
    {
        Id = f.IdFriend,
        Name = f.IdFriendNavigation.UserInfo.Name,
        Surname = f.IdFriendNavigation.UserInfo.Surname,
        Lastname = f.IdFriendNavigation.UserInfo.Lastname,
        JobName = f.IdFriendNavigation.UserInfo.IdJobNavigation.Name,
        PathPhoto = ViewModelFriend.GetPath(f.IdFriendNavigation.IdPhotoNavigation),
        IdChat = 0
    }).ToListAsync();
            foreach (var fr in Friends)
            {
                int id = await ViewModelFriend.GetChatId(userId, fr.Id, db);
                fr.IdChat = id;
            }
        }
    }
    public class ViewModelChat
    {
        public int Id { get; set; }
        public string NameChat { get; set; }
        public List<string> Participants { get; set; }
        public bool isGroup { get; set; }
        public string PhotoPath { get; set; } = "/res/image/User.png";

        public static string GetPath(Chat chat, string curUserId)
        {
            if (chat.IsGroup)
                return "";
            ChatParticipant Participant = chat.ChatParticipants.Where(p => p.IdUser != curUserId).FirstOrDefault();
            if (Participant == null)
                return "/res/image/User.png";
            if (Participant.IdUserNavigation.IdPhotoNavigation != null)
            return $"/userImages/{Participant.IdUserNavigation.IdPhotoNavigation.Name}";
            return "/res/image/User.png";
        }
    }
    public class ViewModelFriend
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string JobName { get; set; }
        public string PathPhoto { get; set; }
        public int IdChat { get; set; }
        public static string GetPath(UserImage image)
        {
            if (image == null)
                return "/res/image/User.png";
            return $"/userImages/{image.Name}";
        }
        public static async Task<int> GetChatId(string curId, string idFriend, MessendgerDb db)
        {
            List<Chat> chatsUser = await db.ChatParticipants.Include(x => x.IdChatNavigation).Include(x => x.IdChatNavigation.ChatParticipants).Where(x => x.IdUser == curId).Select(x => x.IdChatNavigation).ToListAsync();
            foreach (var chat in chatsUser)
            {
                ChatParticipant chatF = chat.ChatParticipants.Where(x => x.IdUser == idFriend).FirstOrDefault();
                if (chatF != null)
                    return chatF.IdChat;
            }
            return 0;
        }
    }

}
