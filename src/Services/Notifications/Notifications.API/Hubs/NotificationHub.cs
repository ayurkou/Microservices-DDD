using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Notifications.API.Hubs
{
    public class NotificationHub: Hub
    {
        public async Task Notify(Guid userId, string message)
        {
            await Clients.All.SendAsync("Completed", userId, message);
        }
    }
}