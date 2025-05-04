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
        public int IdCurChat { get; private set; } = 0;
        public UserInfo info { get; set; }
        public List<ViewModelChat> Chats { get; set; } = [];
        public List<itemMessage> Messages { get; set; } = [];

        public async Task OnGetAsync()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IdCurChat = Convert.ToInt32(RouteData.Values["idCurChat"]);
            if (IdCurChat != 0)
            {
                Messages = await Message_actions.GetMessage(IdCurChat, db, userId);
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
            ChatParticipant Participant = chat.ChatParticipants.Where(p => p.IdUser != curUserId).First();
            if (Participant.IdUserNavigation.IdPhotoNavigation != null)
            return $"/userImages/{Participant.IdUserNavigation.IdPhotoNavigation.Name}";
            return "/res/image/User.png";
        }
    }
}
