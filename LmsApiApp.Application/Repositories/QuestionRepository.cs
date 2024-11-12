using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly LmsApiDbContext _context;

        public QuestionRepository(LmsApiDbContext context)
        {
            _context = context;
        }

        public async Task<Question> GetByIdAsync(int questionId)
        {
            return await _context.Questions.FindAsync(questionId);
        }

        public async Task DeleteAsync(int questionId)
        {
            var question = await GetByIdAsync(questionId);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }
    }

}
