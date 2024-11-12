using LmsApiApp.Application.Dtos.TestDtos;
using LmsApiApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface ITestService
    {

        Task<TestResponseDto> CreateTestAsync(TestDto testDto);
        Task<TestResponseDto> UpdateTestAsync(int id, TestDto testDto);
        Task<TestResponseDto> GetTestAsync(int id);
        Task<QuestionDto> AddQuestionToTestAsync(int testId, QuestionDto questionDto);
        Task<QuestionDto> CloseQuestionAsync(int questionId);
        Task DeleteQuestionAsync(int questionId); // Yeni metod
        
        Task ApproveTestResultAsync(int testResultId);
        Task<string> AnswerOpenQuestionAsync(int questionId, string answer); // Yeni metod
        Task<IEnumerable<TestResponseDto>> GetTestsByCourseIdAsync(int courseId);
        Task<IEnumerable<TestResultDto>> GetTestResultsAsync(int testId);
        Task<TestResult> SubmitTestResultAsync(int testId, string userId, TestResultDto testResultDto);
        Task UpdateTestDurationInDatabase(int testId, TimeSpan newDuration);
    }

}
