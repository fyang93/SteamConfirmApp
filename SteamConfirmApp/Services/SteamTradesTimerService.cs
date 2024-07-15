using System.Runtime.InteropServices;

namespace SteamConfirmApp.Services
{
    public class SteamTradesTimerService
    {
        private PeriodicTimer _timer;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _enabled;
        private TimeSpan _interval;

        public event Func<Task>? OnTickAsync;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    RestartTimer(this.Interval);
                } else {
                    StopTimer();
                }
            }
        }

        public TimeSpan Interval { get; set; }

        public SteamTradesTimerService(TimeSpan interval)
        {
            _timer = new(interval);
            this.Interval = interval;
            _cancellationTokenSource = new();
            StartTimer();
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
            this.Interval = newInterval;
            _cancellationTokenSource = new();
            StartTimer();
        }
    }
}
