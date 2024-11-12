using LmsApiApp.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

public class TimerHub : Hub
{
    private readonly ITestService _testService;
    private static readonly Dictionary<int, System.Timers.Timer> _timers = new Dictionary<int, System.Timers.Timer>();

    public TimerHub(ITestService testService)
    {
        _testService = testService;
    }

    public async Task UpdateTestDuration(int testId, TimeSpan newDuration)
    {
        // Mevcut bir zamanlayıcı varsa durdur
        if (_timers.ContainsKey(testId))
        {
            _timers[testId].Stop();
            _timers[testId].Dispose();
        }

        // Yeni zamanlayıcıyı oluştur
        System.Timers.Timer timer = new System.Timers.Timer(1000); // 1 saniye aralıklarla güncelle
        timer.Elapsed += async (sender, e) => await SendTimeUpdate(testId);
        timer.Start();

        _timers[testId] = timer;

        // Veritabanını güncelle
        await _testService.UpdateTestDurationInDatabase(testId, newDuration);

        await Clients.Group(testId.ToString()).SendAsync("UpdateDuration", newDuration);
    }

    private async Task SendTimeUpdate(int testId)
    {
        // Kalan süreyi güncelleyin ve istemcilere bildirin
        if (_timers.ContainsKey(testId))
        {
            var timer = _timers[testId];
            // Kalan süreyi bir saniye düşür
            TimeSpan remainingTime = TimeSpan.FromSeconds(timer.Interval / 1000) - TimeSpan.FromSeconds(1);

            if (remainingTime.TotalSeconds <= 0)
            {
                // Süre dolmuşsa gerekli işlemleri yap
                await Clients.Group(testId.ToString()).SendAsync("TimeExpired", testId);
                timer.Stop();
                _timers.Remove(testId);
            }
            else
            {
                await Clients.Group(testId.ToString()).SendAsync("ReceiveTimeUpdate", remainingTime);
            }
        }
    }

    public async Task JoinTest(int testId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, testId.ToString());
    }

    public async Task LeaveTest(int testId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, testId.ToString());
    }

    public async Task CloseQuestion(int questionId)
    {
        var closedQuestion = await _testService.CloseQuestionAsync(questionId);
        await Clients.All.SendAsync("QuestionClosed", closedQuestion);
    }
}
