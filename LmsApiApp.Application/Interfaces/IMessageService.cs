using LmsApiApp.Core.Entities;

namespace LmsApiApp.Application.Interfaces
{
    public interface IMessageService
    {
        Task SaveMessageAsync(Message message); // Mesaj kaydetme metodu
        Task SendMessageAsync(string senderId, string receiverId, string content); // Mesaj gönderme metodu
    }
}
