using System.Threading;
using System.Threading.Tasks;

namespace LittleTask
{
    public interface ICronTask
    {
        // The cron expression for the task.
        string Cron { get; }

        // The implementation of the task.
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
