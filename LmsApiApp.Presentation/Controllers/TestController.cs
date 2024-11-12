using LmsApiApp.Application.Dtos.TestDtos;
using LmsApiApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _testService;
        private readonly IHubContext<TimerHub> _hubContext; // SignalR Hub'u ekleyin

        public TestController(ITestService testService, IHubContext<TimerHub> hubContext)
        {
            _testService = testService;
            _hubContext = hubContext;
        }

        // Tüm testleri alma (Kursa göre filtreleme)
        [HttpGet]
        public async Task<IActionResult> GetAllTests(int courseId)
        {
            var tests = await _testService.GetTestsByCourseIdAsync(courseId);
            return Ok(tests);
        }

        // Test oluşturma
        [HttpPost]
        public async Task<IActionResult> CreateTest(
            string title,
            string content,
            string mediaUrl,
            int courseId,
            string userId,
            int testDuration,
            bool isTimed)
        {
            var testDto = new TestDto
            {
                Title = title,
                Content = content,
                MediaUrl = mediaUrl,
                CourseId = courseId,
                UserId = userId,
                TestDuration = TimeSpan.FromMinutes(testDuration),
                IsTimed = isTimed
            };

            var createdTest = await _testService.CreateTestAsync(testDto);
            return CreatedAtAction(nameof(GetTest), new { id = createdTest.Id }, createdTest);
        }

        // Belirli bir testi alma
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTest(int id)
        {
            var test = await _testService.GetTestAsync(id);
            // Testteki açık soruları gizleme
            foreach (var question in test.Questions)
            {
                if (question.Type == "open-ended")
                {
                    question.Answers = null; // Açık uçlu soruların cevaplarını gizle
                }
            }
            return Ok(test);
        }

        // Teste soru ekleme
        [HttpPost("{testId}/questions")]
        public async Task<IActionResult> AddQuestionToTest(int testId, QuestionDto questionDto)
        {
            var question = await _testService.AddQuestionToTestAsync(testId, questionDto);
            return Ok(question);
        }

        // Soruyu kapatma
        [HttpPut("questions/{questionId}/close")]
        public async Task<IActionResult> CloseQuestion(int questionId)
        {
            var question = await _testService.CloseQuestionAsync(questionId);
            return Ok(question);
        }

        // Soruyu silme
        [HttpDelete("questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            await _testService.DeleteQuestionAsync(questionId);
            return NoContent(); // 204 No Content
        }

        // Öğretmenin açık sorulara cevap vermesi
        [HttpPut("questions/{questionId}/answer")]
        public async Task<IActionResult> AnswerOpenQuestion(int questionId, [FromBody] string answer)
        {
            var result = await _testService.AnswerOpenQuestionAsync(questionId, answer);
            return Ok(result);
        }
        [HttpPost("{testId}/submit")]
        public async Task<IActionResult> SubmitTestResult(int testId, [FromBody] TestResultDto testResultDto)
        {
            // Kullanıcının test sonucunu kaydet
            var result = await _testService.SubmitTestResultAsync(testId, testResultDto.UserId, testResultDto);

            // Test sonuçlarını almak için gerekli bir metod yazın
            var results = await _testService.GetTestResultsAsync(testId);

            // Kullanıcıya verilen puan ve diğer bilgileri döndür
            var response = new
            {
                UserScore = results.FirstOrDefault(r => r.UserId == testResultDto.UserId)?.Score, // Kullanıcının puanı
                TotalScore = results.Sum(r => r.Score) // Toplam puan
            };

            return Ok(response);
        }


        // Testi tamamlayan kullanıcıların puanlarını alma
        [HttpGet("{testId}/results")]
        public async Task<IActionResult> GetTestResults(int testId)
        {
            var results = await _testService.GetTestResultsAsync(testId);
            return Ok(results);
        }


    }
}
