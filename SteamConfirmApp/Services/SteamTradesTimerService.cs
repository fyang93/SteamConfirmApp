using System.Runtime.InteropServices;

namespace SteamConfirmApp.Services
{
    public class SteamTradesTimerService : IDisposable
    {
        private PeriodicTimer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        public event Func<Task>? OnTickAsync;


        public SteamTradesTimerService(TimeSpan interval)
        {
            _timer = new(interval);
            _cancellationTokenSource = new();
            StartTimer();
        }

        public void Dispose()
        {
            this.StopTimer();
        }

        private async void StartTimer()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
                {
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
