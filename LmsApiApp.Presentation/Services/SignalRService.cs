using LmsApiApp.Application.Interfaces;
using LmsApiApp.Presentation.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LmsApiApp.Presentation.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendMessageToUserAsync(string userId, string senderName, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", senderName, message);
        }
    }
}
