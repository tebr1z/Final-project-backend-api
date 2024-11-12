using LmsApiApp.Core.Entities;

namespace LmsApiApp.Application.Interfaces
{
    public interface ITestRepository
    {
        Task<IEnumerable<Test>> GetTestsByCourseIdAsync(int courseId);
        Task<Test> GetByIdAsync(int id);
        Task AddAsync(Test test);
        Task UpdateAsync(Test test);
        Task DeleteAsync(Test test);
        Task<Question> GetQuestionByIdAsync(int questionId);
        Task AddTestResultAsync(TestResult testResult);
        Task<TestResult> GetTestResultByIdAsync(int testResultId);
        Task UpdateQuestionAsync(Question question); // Question nesnesi güncellemesi için
        Task UpdateTestResultAsync(TestResult testResult); // TestResult nesnesi güncellemesi için
        Task<IEnumerable<TestResult>> GetTestResultsAsync(int testId); // Yeni metod

    }

}
