using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LittleTask
{
    public class CronTaskRunner<TCronTask> : BackgroundService, IDisposable
        where TCronTask : ICronTask
    {
        protected ILogger Logger { get; }
        protected IServiceProvider Services { get; }

        public CronTaskRunner(IServiceProvider services, ILogger<CronTaskRunner<TCronTask>> logger)
        {
            this.Logger = logger;
            this.Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = this.Services.CreateScope())
                {
                    var bgCron = scope.ServiceProvider.GetRequiredService<TCronTask>();

                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    var cronExpression = CronExpression.Parse(bgCron.Cron);
                    var nextRun = cronExpression.GetNextOccurrence(DateTimeOffset.UtcNow, TimeZoneInfo.Utc);
                    var delay = (nextRun - DateTimeOffset.UtcNow).Value;
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, stoppingToken);
                    }

                    try
                    {
                        await bgCron.Execute(stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        this.Logger.LogTrace("Task cancelled.");
                        return;
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogCritical($"Unhandled exception from cron task {e}");
                    }
                }
            }
        }
    }
}