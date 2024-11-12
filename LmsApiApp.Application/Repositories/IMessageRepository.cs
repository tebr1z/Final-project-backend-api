using Google;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly LmsApiDbContext _context;

        public MessageRepository(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(Message message)
        {
            _context.messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessagesAsync(string userId, string contactId)
        {
            return await _context.messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == contactId) || (m.SenderId == contactId && m.ReceiverId == userId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
