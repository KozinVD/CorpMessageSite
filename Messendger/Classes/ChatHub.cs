using Messendger.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System;
using System.IO;
using System.Linq;

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
                Entities.Message newMes = new Entities.Message() {Text = message, IdUser = idSender, IdChat = idChat, TimeSend = DateTime.Parse(timeMessage)};
                db.Messages.Add(newMes);
                await db.SaveChangesAsync();
                //Отправление сообщения другим участникам
                await Clients.Users(chat.ChatParticipants.Where(p => p.IdUser != idSender).Select(p => p.IdUser).ToList()).SendAsync("Receive", idChat, sender, message, timeMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.InnerException.GetType().FullName);
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.InnerException.Message);
                Console.WriteLine("Ошибка");
                Console.WriteLine($"{idChatS} {message} {timeMessage}");

            }
        }
        public async Task CreateNewChat(string[] UsersId, string chatName)
        {
            string idSender = Context.UserIdentifier;

            string path = "/res/image/User.png";
            if (UsersId.Length == 1)
            {
                if (await ChatExsist(UsersId, idSender, db))
                    { return; }
                User user = db.Users.Include(u => u.IdPhotoNavigation).Where(u => u.Id == idSender).First();
                if (user.IdPhotoNavigation != null)
                    path = $"/userImages/{user.IdPhotoNavigation.Name}";
            }
            Chat newChat = new Chat();
            newChat.IsGroup = false;
            if (UsersId.Length > 1)
            {
                newChat.IsGroup = true;
                if (chatName != "" && !string.IsNullOrWhiteSpace(chatName))
                    newChat.Name = chatName;
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
        
        public static async Task<bool> ChatExsist(string[] ids, string idSender, MessendgerDb db)
        {
            var targetUserIds = ids.Append(idSender).Distinct().OrderBy(id => id).ToList();
            List<Chat> chatUser = await db.ChatParticipants.Include(x => x.IdChatNavigation).ThenInclude(x => x.ChatParticipants).Where(x => x.IdUser == idSender).Select(x => x.IdChatNavigation).ToListAsync();

            foreach (var chat in chatUser)
            {
                var chatUserIds = chat.ChatParticipants
            .Select(cp => cp.IdUser)
            .Distinct()
            .OrderBy(id => id)
            .ToList();
                if (chatUserIds.SequenceEqual(targetUserIds))
                {
                    // Такой чат уже существует
                    return true;
                }
            }
            return false;
        }
        public async Task DeleteChat(string idChatS)
        {
            int idChat = int.Parse(idChatS);
            List<string> particants = await db.ChatParticipants.Where(p => p.IdChat == idChat).Select(p => p.IdUser).ToListAsync(); 
            db.Chats.Remove(await db.Chats.FindAsync(idChat));
            await db.SaveChangesAsync();
            await Clients.Users(particants).SendAsync("DeleteChat", idChat);
        }
        public async Task DeleteChatPart(string idChatS, string IdUserP)
        {
            ChatParticipant participant = await db.ChatParticipants.Where(p => p.IdChat == int.Parse(idChatS) && p.IdUser == IdUserP).FirstAsync();
            db.ChatParticipants.Remove(participant);
            await db.SaveChangesAsync();
            await Clients.Users(IdUserP).SendAsync("DeleteChat", int.Parse(idChatS));
        }
        public async Task ChangeNameChat(string idChatS, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;
            Chat chat = await db.Chats.Include(c => c.ChatParticipants).Where(x => x.Id == int.Parse(idChatS)).FirstAsync();
            chat.Name = Name;
            await db.SaveChangesAsync();
            await Clients.Users(chat.ChatParticipants.Select(p => p.IdUser)).SendAsync("ChangeNameChat", chat.Id, Name);
        }
        public async Task AddUserChat(string idChatS, string IdUser)
        {
            string idSender = Context.UserIdentifier;
            Chat curChat = await db.Chats.Include(x => x.ChatParticipants).Where(x => x.Id == int.Parse(idChatS)).FirstAsync();
            ChatParticipant newParticipant = new ChatParticipant() {IdChat = int.Parse(idChatS), IdUser =IdUser };
            await db.ChatParticipants.AddAsync(newParticipant);
            await db.SaveChangesAsync();
            string name = curChat.Name;
                if (curChat.Name == null)
                    name = string.Join(' ', curChat.ChatParticipants.Where(p => p.IdUser != idSender).Select(p => p.IdUserNavigation.UserInfo.Surname + " " + p.IdUserNavigation.UserInfo.Name));
                await Clients.Users(IdUser).SendAsync("createChat", curChat.Id, name, true, "/res/image/User.png");
        }
    }
}
