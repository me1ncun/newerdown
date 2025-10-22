using Microsoft.AspNetCore.SignalR;

namespace NewerDown.Hubs;

public class AlertHub : Hub
{
    public async Task SendAlert(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveAlert", message);
    }
}