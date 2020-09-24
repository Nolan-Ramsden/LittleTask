using System.Threading;
using System.Threading.Tasks;

namespace LittleTask
{
    public interface IBootstrapTask
    {
        Task Bootstrap(CancellationToken stoppingToken);
    }
}
