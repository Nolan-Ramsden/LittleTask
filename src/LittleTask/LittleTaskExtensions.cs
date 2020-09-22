using Microsoft.Extensions.DependencyInjection;

namespace LittleTask
{
    public static class LittleTaskExtensions
    {
        public static IServiceCollection AddBackgroundTask<TBackgroundTask>(this IServiceCollection services)
            where TBackgroundTask : class, IBackgroundTask
        {
            return services.AddHostedService<BackgroundTaskRunner<TBackgroundTask>>();
        }

        public static IServiceCollection AddCronTask<TCronTask>(this IServiceCollection services)
            where TCronTask : class, ICronTask
        {
            return services.AddHostedService<CronTaskRunner<TCronTask>>();
        }
    }
}
