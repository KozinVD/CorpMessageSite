using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        public UserInfo info { get; set; }
        public List<ViewModelChat> Chats { get; set; } = [];

        public async Task OnGetAsync()
        {
            info = await db.UserInfos.Where(x => x.IdNavigation.UserName == User.Identity.Name).FirstOrDefaultAsync();
            Chats = await db.ChatParticipants.Include(y => y.IdChatNavigation)
            .Where(x => x.IdUser == info.Id)
            .Select(chat =>
                new ViewModelChat
                {
                    Id = chat.IdChat,
                    NameChat = chat.IdChatNavigation.Name,
                    Participants = chat.IdChatNavigation.ChatParticipants.Where(x => x.IdUser != info.Id).Select(p => p.IdUserNavigation.UserInfo.Surname + " " + p.IdUserNavigation.UserInfo.Name).ToList()
                }).ToListAsync();
        }
    }
    public class ViewModelChat
    {
        public int Id { get; set; }
        public string NameChat { get; set; }
        public List<string> Participants { get; set; }
    }
}
