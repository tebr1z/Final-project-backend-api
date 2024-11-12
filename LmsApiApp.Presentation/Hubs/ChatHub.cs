using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LmsApiApp.Presentation.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>(); // userId, userName

        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public ChatHub(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            Console.WriteLine($"OnConnectedAsync called. UserId: {userId}");

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user != null)
                {
             
                    await _userService.SetUserOnlineStatusAsync(userId, true);
                    OnlineUsers.TryAdd(userId, user.UserName);

              
                    await Clients.All.SendAsync("UserStatusChanged", user.UserName, true);
                    Console.WriteLine($"User {user.UserName} is now online.");

                 
                    await Clients.Caller.SendAsync("ReceiveUserList", OnlineUsers.Values);
                }
                else
                {
                    Console.WriteLine($"User with ID {userId} not found.");
                }
            }
            else
            {
                Console.WriteLine("UserId could not be retrieved from context.");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            Console.WriteLine($"OnDisconnectedAsync called. UserId: {userId}");

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user != null)
                {
                
                    await _userService.SetUserOnlineStatusAsync(userId, false);
                    OnlineUsers.TryRemove(userId, out _);

                   
                    await Clients.All.SendAsync("UserStatusChanged", user.UserName, false);
                    Console.WriteLine($"User {user.UserName} is now offline.");
                }
                else
                {
                    Console.WriteLine($"User with ID {userId} not found for disconnection.");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

   
        public async Task SendMessageAsync(string receiverId, string message)
        {
            var senderId = Context.User?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            Console.WriteLine($"SendMessageAsync called. SenderId: {senderId}, ReceiverId: {receiverId}, Message: {message}");

            if (!string.IsNullOrEmpty(receiverId) && !string.IsNullOrEmpty(senderId))
            {
                var sender = await _userService.GetUserByIdAsync(senderId);
                var receiver = await _userService.GetUserByIdAsync(receiverId);

                if (sender != null && receiver != null)
                {
                 
                    await Clients.User(receiverId).SendAsync("ReceiveMessage", sender.UserName, message);
                    Console.WriteLine($"Message successfully sent from {sender.UserName} to {receiver.UserName}");

                  
                    var newMessage = new Message
                    {
                        SenderId = senderId,
                        ReceiverId = receiverId,
                        Content = message,
                        SentAt = DateTime.Now,
                        IsRead = false
                    };

                    await _messageService.SaveMessageAsync(newMessage);
                    Console.WriteLine("Message saved to database.");
                }
                else
                {
                    Console.WriteLine($"SendMessageAsync: Sender or receiver not found. SenderId: {senderId}, ReceiverId: {receiverId}");
                }
            }
            else
            {
                Console.WriteLine($"SendMessageAsync: SenderId or ReceiverId is empty. SenderId: {senderId}, ReceiverId: {receiverId}");
            }
        }
    }
}
