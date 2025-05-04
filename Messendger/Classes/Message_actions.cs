using Messendger.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messendger.Classes
{
    public static class Message_actions
    {
        public async static Task<List<itemMessage>> GetMessage(int id, MessendgerDb db, string user)
        {
            
            List<itemMessage> messages = await db.Messages.AsNoTracking().Include(x => x.IdChatNavigation).ThenInclude(x => x.ChatParticipants).Include(x => x.IdUserNavigation).ThenInclude(x => x.UserInfo)
                .Where(x => x.IdChat == id && x.IdChatNavigation.ChatParticipants.Select(p => p.IdUser).ToList().Contains(user))
                .Select(m => new itemMessage
                (
                    checkYou(m.IdUser, user),
                    m.IdUserNavigation.UserInfo.Surname +
                " " + m.IdUserNavigation.UserInfo.Name,
                    m.Text, m.TimeSend)).ToListAsync();
            return messages;
        }
        static bool checkYou(string id, string id2)
        {
            if (id == id2)
                return true;
            return false;
        }
    }
    public record itemMessage (bool isYou, string Sender, string Text, DateTime TimeSend);
}
