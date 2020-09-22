using System;
using System.Threading;
using System.Threading.Tasks;

namespace LittleTask
{
    public interface IBackgroundTask
    {
        // How long to wait between task invocations.
        TimeSpan Delay { get; }

        // The implementation of the task.
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
