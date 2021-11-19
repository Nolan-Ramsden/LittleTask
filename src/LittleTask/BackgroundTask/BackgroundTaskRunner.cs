using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LittleTask
{
    public class BackgroundTaskRunner<TBackgroundTask> : BackgroundService, IDisposable
        where TBackgroundTask : IBackgroundTask
    {
        protected ILogger Logger { get; }
        protected IServiceProvider Services { get; }

        public BackgroundTaskRunner(IServiceProvider services, ILogger<BackgroundTaskRunner<TBackgroundTask>> logger)
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
                    var bgTask = scope.ServiceProvider.GetRequiredService<TBackgroundTask>();
                    try
                    {
                        await Task.Delay(bgTask.Delay, stoppingToken);
                        await bgTask.Execute(stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        this.Logger.LogTrace("Task cancelled.");
                        return;
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogCritical($"Unhandled exception from background task {e}");
                    }
                }
            }
        }
    }
}