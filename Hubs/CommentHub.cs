using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Hubs
{
    public class CommentHub : Hub
    {
        public async Task SendMessage(string UserName, string UserId, int NewsID, string Message)
        {
            await Clients.All.SendAsync("ReceiveMessage", UserName, Message);
            //Repository repository = new Repository(new MessageDbContext());
            //repository.Add(new Message { Text = message, User = user });

            //ApplicationContext context = new ApplicationContext();
        }
    }
}
