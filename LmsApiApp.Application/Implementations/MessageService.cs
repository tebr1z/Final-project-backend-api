using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using System;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;
        private readonly ISignalRService _signalRService;
        private readonly LmsApiDbContext _context;

        public MessageService(IMessageRepository messageRepository, IUserService userService, ISignalRService signalRService, LmsApiDbContext context)
        {
            _messageRepository = messageRepository;
            _userService = userService;
            _signalRService = signalRService;
            _context = context;
        }

        public async Task SendMessageAsync(string senderId, string receiverId, string content)
        {
            var sender = await _userService.GetUserByIdAsync(senderId);
            var receiver = await _userService.GetUserByIdAsync(receiverId);

            if (sender == null || receiver == null)
                return;

            // Mesajı veritabanına kaydet
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                SentAt = DateTime.Now
            };

            await _messageRepository.SaveMessageAsync(message);

            // Eğer alıcı online ise, mesajı SignalR ile anlık olarak gönder
            if (receiver.IsOnline)
            {
                await _signalRService.SendMessageToUserAsync(receiverId, sender.FullName, content);
            }
            else
            {
                // Eğer alıcı offline ise, e-posta bildirimi gönder
                await _userService.SendOfflineNotificationAsync(receiver, message);
            }
        }

        // Mesajı kaydetme işlemi
        public async Task SaveMessageAsync(Message message)
        {
            _context.messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
