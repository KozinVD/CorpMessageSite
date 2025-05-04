using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Messendger.Classes
{
    [Authorize]
    public class ChatHub : Hub
    {
        public MessendgerDb db {get; set; }
        public ChatHub(MessendgerDb db)
        {
            this.db = db;
        }
        public async Task Send(string idChatS, string message, string timeMessage)
        {
            try
            {
                string idSender = Context.UserIdentifier;
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
                Console.WriteLine($"{idChatS} {message} {timeMessage}"); 
            }
        }
        public async Task CreateNewChat(string[] UsersId)
        {
            string idSender = Context.UserIdentifier;
            string path = "/res/images/User.png";
            if (UsersId.Length == 1)
            {
                User user = db.Users.Include(u => u.IdPhotoNavigation).Where(u => u.Id == idSender).First();
                if (user.IdPhotoNavigation != null)
                    path = $"/userImages/{user.IdPhotoNavigation.Name}";
            }
            Chat newChat = new Chat();
            newChat.IsGroup = false;
            if (UsersId.Length > 1)
            {
                newChat.IsGroup = true;
            }
            await db.Chats.AddAsync(newChat);
            await db.SaveChangesAsync();
            foreach (var user in UsersId)
            {
                ChatParticipant participant = new ChatParticipant()
                { 
                  IdChat = newChat.Id,
                  IdUser = user
                };
                await db.ChatParticipants.AddAsync(participant);
            }
            ChatParticipant part = new ChatParticipant()
            {
                IdChat = newChat.Id,
                IdUser = idSender
            };
            await db.ChatParticipants.AddAsync(part);
            await db.SaveChangesAsync();
            List<ChatParticipant> participants = db.Chats.Include(x => x.ChatParticipants)
                .ThenInclude(x => x.IdUserNavigation)
                .ThenInclude(x => x.UserInfo)
                .Where(x => x.Id == newChat.Id).First().ChatParticipants.ToList();
            string name = newChat.Name;
            List<string> Users = UsersId.Where(x => x != idSender).ToList();
            foreach (var user in Users)
            {
                if (newChat.Name == null)
                name = string.Join(' ', participants.Where(p => p.IdUser != user).Select(p => p.IdUserNavigation.UserInfo.Surname + " " + p.IdUserNavigation.UserInfo.Name));
                await Clients.Users(user).SendAsync("createChat", newChat.Id, name, newChat.IsGroup, path);
            }
                await Clients.Users(idSender).SendAsync("createChatO", newChat.Id);
        }
    }
}
