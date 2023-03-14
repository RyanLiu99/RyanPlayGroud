using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Medrio.CspReport
{
    /// <summary>
    /// De-dup and Push collected violation to storage
    /// </summary>
    /// <remarks>To use: services.AddHostedService&lt;CspPushService&gt;(); in hosted environment.</remarks>

    public class CspPushService : BackgroundService
    {
        private readonly ICspViolationCollector _collector;
        private readonly ICspReportPusher _pusher;

        private readonly ILogger<CspPushService> _logger;
        private int _startRequested = 0;

        public CspPushService(ICspViolationCollector collector, ICspReportPusher pusher, IHostApplicationLifetime lifeTime, ILogger<CspPushService> logger)
        {
            _collector = collector;
            _pusher = pusher;
            _logger = logger;

            lifeTime.ApplicationStopping.Register((logger) =>
            {
                IList<string>? violations = _collector.WindDown();
                (logger as ILogger).LogInformation("CspPushService winding down ..., push last {c} violations", violations.Count);
                Push(violations);
            }, _logger);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mainTask = Task.Factory.StartNew(state =>
                {
                    CancellationToken token = (CancellationToken)state;
                    token.ThrowIfCancellationRequested();

                    while (!token.IsCancellationRequested)
                    {
                        IList<string>? violations = _collector.BlockToHandOverData();
                        Push(violations);
                    }

                }, stoppingToken, stoppingToken, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            mainTask.ContinueWith(
                (task) => { _logger.LogError(task.Exception, "CspPushService faulted!"); },
                TaskContinuationOptions.OnlyOnFaulted);

            return mainTask;
        }

        private void Push(IList<string>? violations)
        {
            if (violations != null && violations.Any())
            {
                var distinct = violations.Distinct().ToArray();

                try
                {
                    _pusher.Push(distinct);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to InvalidateCache");
                }
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            var oldValue = Interlocked.CompareExchange(ref _startRequested, 1, 0);

            if (oldValue == 1) return Task.CompletedTask;

            _logger.LogInformation("Starting CspPushService...");
            return base.StartAsync(cancellationToken);
        }

        public bool IsHealthy(out Exception? unHealthyDetail)
        {
            if (ExecuteTask == null)
            {
                unHealthyDetail = new Exception("CspPushService never started!");
                return false;
            }

            var isHealthy = !ExecuteTask.IsFaulted;

            if (!isHealthy)
            {
                unHealthyDetail = ExecuteTask.Exception?.GetBaseException();
            }
            else
            {
                unHealthyDetail = null;
            }

            return isHealthy;
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default )
        {
            _logger.LogInformation("Stopping CspPushService...");
            await base.StopAsync(cancellationToken).ConfigureAwait(false);
            _startRequested = 0;
        }
    }
}
