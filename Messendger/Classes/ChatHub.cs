using Microsoft.AspNetCore.SignalR;

namespace Messendger.Classes
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
