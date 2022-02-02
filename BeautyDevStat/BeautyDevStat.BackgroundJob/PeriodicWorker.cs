using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeautyDevStat.BackgroundJob
{
    public class PeriodicWorker :IDisposable
    {
        private readonly Func<CancellationToken, Task> _action;
        private readonly TimeSpan _period;
        private Task _task;
        private CancellationTokenSource _cancellationSource;

        public PeriodicWorker(Func<CancellationToken, Task> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken());
        }

        public void Start(TimeSpan interval, CancellationToken cancellationToken)
        {
            _cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(_cancellationSource.Token, cancellationToken);
            _task = PeriodicAsync(interval, _cancellationSource.Token);
        }

        public void Stop()
        {
            _cancellationSource.Cancel();
        }
        
        private async Task PeriodicAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                
                await _action(cancellationToken);
                await Task.Delay(interval, cancellationToken);
            }
        }
        
        public void Dispose()
        {
            Stop();
        }
    }
}