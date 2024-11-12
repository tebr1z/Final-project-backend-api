using Google;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

public class TestRepository : ITestRepository
{
    private readonly LmsApiDbContext _context;

    public TestRepository(LmsApiDbContext context)
    {
        _context = context;
    }

    public async Task<Test> GetByIdAsync(int id)
    {
        return await _context.Tests.Include(t => t.Questions).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(Test test)
    {
        await _context.Tests.AddAsync(test);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Test test)
    {
        _context.Tests.Update(test);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Test test)
    {
        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();
    }

    public async Task<Question> GetQuestionByIdAsync(int questionId)
    {
        return await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _context.Questions.Update(question);
        await _context.SaveChangesAsync();
    }

    public async Task AddTestResultAsync(TestResult testResult)
    {
        await _context.TestResults.AddAsync(testResult);
        await _context.SaveChangesAsync();
    }

    public async Task<TestResult> GetTestResultByIdAsync(int testResultId)
    {
        return await _context.TestResults.FirstOrDefaultAsync(tr => tr.Id == testResultId);
    }

    public async Task UpdateTestResultAsync(TestResult testResult)
    {
        _context.TestResults.Update(testResult);
        await _context.SaveChangesAsync();
    }
      public async Task<IEnumerable<Test>> GetTestsByCourseIdAsync(int courseId)
        {
            return await _context.Tests
                .Where(t => t.CourseId == courseId)
                .ToListAsync();
    }
    public async Task<IEnumerable<TestResult>> GetTestResultsAsync(int testId)
    {
        return await _context.TestResults
            .Where(tr => tr.TestId == testId)
            .Include(tr => tr.User) // Kullanıcı bilgilerini içermek için
            .ToListAsync();
    }

}
