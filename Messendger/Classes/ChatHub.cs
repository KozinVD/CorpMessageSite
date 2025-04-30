using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Messendger.Classes
{
    [Authorize]
    public class ChatHub : Hub
    {
        public MessendgerDb db {  get; set; }
        public ChatHub(MessendgerDb db)
        {
            this.db = db;
        }
        public async Task Send(string idChatS, string idSender, string message, string timeMessage)
        {
            try
            {
                int idChat = int.Parse(idChatS);
                Chat chat = db.Chats.Include(x => x.ChatParticipants).Where(x => x.Id == idChat).FirstOrDefault();
                UserInfo senderInfo = await db.UserInfos.FindAsync(idSender);
                string sender = senderInfo.Surname + " " + senderInfo.Name;
                //Сохранение сообщения в бд
                Message newMes = new Message() {Text = message, IdUser = idSender, IdChat = idChat, TimeSend = DateTime.Parse(timeMessage)};
                db.Messages.Add(newMes);
                await db.SaveChangesAsync();
                //Отправление сообщения другим участникам
                await Clients.Users(chat.ChatParticipants.Where(p => p.IdUser != idSender).Select(p => p.IdUser).ToList()).SendAsync("Receive", idChat, sender, message, timeMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка");
                Console.WriteLine($"{idChatS} {idSender} {message} {timeMessage}"); 
            }
        }
    }
}
