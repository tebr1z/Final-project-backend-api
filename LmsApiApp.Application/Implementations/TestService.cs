using AutoMapper;
using LmsApiApp.Application.Dtos.TestDtos;
using LmsApiApp.Application.Interfaces;
using LmsApiApp.Core.Entities;
using LmsApiApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Implementations
{
    public class TestService : ITestService
    {
        private readonly LmsApiDbContext _context;
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;
        public TestService(ITestRepository testRepository, IMapper mapper, IQuestionRepository questionRepository, LmsApiDbContext context)
        {
            _testRepository = testRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _context = context;
        }

        public async Task<TestResponseDto> CreateTestAsync(TestDto testDto)
        {
            var test = _mapper.Map<Test>(testDto);
            test.CreatedAt = DateTime.Now;
            await _testRepository.AddAsync(test);
            return _mapper.Map<TestResponseDto>(test);
        }

        public async Task<TestResponseDto> UpdateTestAsync(int testId, TestDto testDto)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null) throw new KeyNotFoundException("Test not found");
            _mapper.Map(testDto, test);
            test.UpdatedAt = DateTime.Now;
            await _testRepository.UpdateAsync(test);
            return _mapper.Map<TestResponseDto>(test);
        }

        public async Task<TestResponseDto> GetTestAsync(int testId)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null) throw new KeyNotFoundException("Test not found");
            return _mapper.Map<TestResponseDto>(test);
        }

        public async Task<bool> DeleteTestAsync(int testId)
        {
            var test = await _testRepository.GetByIdAsync(testId);
            if (test == null) throw new KeyNotFoundException("Test not found");
            await _testRepository.DeleteAsync(test);
            return true;
        }

        public async Task<QuestionDto> AddQuestionToTestAsync(int testId, QuestionDto questionDto)
        {
            try
            {
                var test = await _testRepository.GetByIdAsync(testId);
                if (test == null) throw new KeyNotFoundException("Test not found.");

                var question = _mapper.Map<Question>(questionDto);
                question.TestId = testId; // Test ID'yi ayarla

                // Cevapları ilişkilendir
                question.Answers = questionDto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList();

                test.Questions.Add(question); // Soruyu teste ekle

                await _testRepository.UpdateAsync(test); // Testi güncelle

                return _mapper.Map<QuestionDto>(question); // Geri döndür
            }
            catch (DbUpdateException ex)
            {
                // Hata mesajını loglayın veya hata ile ilgili bilgiyi döndürün
                throw new Exception("Veritabanı güncelleme hatası: " + ex.InnerException?.Message);
            }
        }

        public async Task<QuestionDto> CloseQuestionAsync(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null) throw new KeyNotFoundException("Question not found");

            question.IsClosed = true; // Soruyu kapatma işlemi
            await _questionRepository.UpdateAsync(question); // Güncelleme işlemi

            return _mapper.Map<QuestionDto>(question); // Dönüş türünü sağla
        }

    
        public async Task ApproveTestResultAsync(int testResultId)
        {
            var testResult = await _testRepository.GetTestResultByIdAsync(testResultId);
            if (testResult == null)
            {
                throw new KeyNotFoundException("Test result not found.");
            }

            testResult.IsApprovedByTeacher = true; // Onay durumunu güncelle
            await _testRepository.UpdateTestResultAsync(testResult); // Güncellemeyi veritabanına kaydet
        }


        public async Task DeleteQuestionAsync(int questionId)
        {
            // Soru silme işlemini burada gerçekleştirin
            await _questionRepository.DeleteAsync(questionId); // Varsayılan repository metodunuz
        }
        public async Task<string> AnswerOpenQuestionAsync(int questionId, string answer)
        {
            // Cevabı saklama işlemi
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                throw new KeyNotFoundException("Question not found.");
            }

          
            return answer; // Cevabı döndürün
        }
        public async Task<IEnumerable<TestResponseDto>> GetTestsByCourseIdAsync(int courseId)
        {
            var tests = await _context.Tests
                .Include(t => t.Questions) // Soruları dahil et
                .ThenInclude(q => q.Answers) // Eğer soruların cevaplarını da almak istiyorsanız
                .Where(t => t.CourseId == courseId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TestResponseDto>>(tests);
        }

        public async Task<IEnumerable<TestResultDto>> GetTestResultsAsync(int testId)
        {
            var testResults = await _testRepository.GetTestResultsAsync(testId);
            return _mapper.Map<IEnumerable<TestResultDto>>(testResults);
        }


        public async Task<TestResult> SubmitTestResultAsync(int testId, string userId, TestResultDto testResultDto)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test == null) throw new KeyNotFoundException("Test not found");

            // Kullanıcının önceki sonuçlarını kontrol edin
            var existingResult = await _context.TestResults
                .FirstOrDefaultAsync(r => r.TestId == testId && r.UserId == userId);

            // Soruları işleme
            foreach (var questionResult in testResultDto.Questions)
            {
                // Burada her bir sorunun cevabını kontrol edin
                // Örneğin, veritabanında kaydedebilirsiniz
            }

            if (existingResult != null)
            {
                existingResult.Score = CalculateScore(testResultDto); // Mevcut sonucu güncelleyin
                existingResult.CompletedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return existingResult;
            }

            var newResult = new TestResult
            {
                TestId = testId,
                UserId = userId,
                Score = CalculateScore(testResultDto), // Yeni sonucu hesaplayın
                CompletedAt = DateTime.Now
            };

            await _context.TestResults.AddAsync(newResult);
            await _context.SaveChangesAsync();
            return newResult;
        }

        private int CalculateScore(TestResultDto testResultDto)
        {
            // Burada, cevapların doğruluğunu kontrol ederek toplam puanı hesaplayabilirsiniz
            int score = 0;

            foreach (var question in testResultDto.Questions)
            {
                // Cevapların doğruluğunu kontrol edin ve puan ekleyin
                var correctAnswer = _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefault(q => q.Id == question.QuestionId)
                    ?.Answers.FirstOrDefault(a => a.IsCorrect);

                if (correctAnswer != null && question.GivenAnswer == correctAnswer.Text)
                {
                    score += 1; // Doğru cevap için 1 puan ekleyin (ihtiyaca göre ayarlayın)
                }
            }

            return score;
        }

        public async Task UpdateTestDurationInDatabase(int testId, TimeSpan newDuration)
        {
            var test = await _context.Tests.FindAsync(testId);
            if (test != null)
            {
                test.TestDuration = newDuration; // Yeni süreyi ayarla
                await _context.SaveChangesAsync(); // Veritabanını güncelle
            }
        }
    }

}
