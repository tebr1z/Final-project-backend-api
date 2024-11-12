using LmsApiApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(int questionId);
        Task DeleteAsync(int questionId);
        Task UpdateAsync(Question question);
        // Diğer gerekli metodları ekleyin...
    }
}
