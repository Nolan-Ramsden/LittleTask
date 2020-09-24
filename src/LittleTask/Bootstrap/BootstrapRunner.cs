using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LittleTask
{
    class BootstrapRunner
    {
        protected ILogger Logger { get; }
        protected IEnumerable<IBootstrapTask> BootstrapTasks { get; }

        public BootstrapRunner(IEnumerable<IBootstrapTask> bootstrapTasks, ILogger<BootstrapRunner> logger)
        {
            this.Logger = logger;
            this.BootstrapTasks = bootstrapTasks;
        }

        public void Run()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            try
            {
                Task.WhenAll(
                    this.BootstrapTasks.Select(t => t.Bootstrap(tokenSource.Token))
                ).GetAwaiter().GetResult();
            }
            catch(Exception e)
            {
                this.Logger.LogCritical("Bootstrapping failed. {Exception}", e);
                throw;
            }
            finally
            {
                tokenSource.Cancel();
            }
        }
    }
}
