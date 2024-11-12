using LmsApiApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IMessageRepository
    {
        Task SaveMessageAsync(Message message);
        Task<List<Message>> GetMessagesAsync(string userId, string contactId);
    }
}
