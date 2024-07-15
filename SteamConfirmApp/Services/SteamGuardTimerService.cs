using SteamAuth;

namespace SteamConfirmApp.Services
{
    public class SteamGuardTimerService
    {
        private PeriodicTimer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        public event Func<Task>? OnTickAsync;

        public long SteamTime { get; private set; } = 0;

        public int SecondsUntilChange { get; private set; } = 0;

        public string PercentageUntilChange { get; private set; } = string.Empty;

        public SteamGuardTimerService(TimeSpan interval)
        {
            _timer = new(interval);
            _cancellationTokenSource = new();
            StartTimer();
        }

        private async Task RunPeriodicTaskAsync()
        {
            if (SteamTime == 0)
            {
                SteamTime = await TimeAligner.GetSteamTimeAsync();
            } else
            {
                SteamTime += 1;
            }
            SecondsUntilChange = 30 - (int)(SteamTime - (SteamTime / 30L * 30L));
            PercentageUntilChange = $"{(int)Math.Round(((double)SecondsUntilChange / 30) * 100)}%";
        }

        private async void StartTimer()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
                {
                    await RunPeriodicTaskAsync();
                    OnTickAsync?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
                // Timer was canceled
            }
        }

        public void StopTimer()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void RestartTimer(TimeSpan newInterval)
        {
            _cancellationTokenSource?.Cancel();
            _timer = new(newInterval);
            _cancellationTokenSource = new();
            StartTimer();
        }
    }
}
