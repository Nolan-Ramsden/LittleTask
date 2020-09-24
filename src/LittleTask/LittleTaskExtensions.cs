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


        public static IServiceCollection AddBootstrapTask<TBootstrapTask>(this IServiceCollection services)
            where TBootstrapTask : class, IBootstrapTask
        {
            return services.AddTransient<IBootstrapTask, TBootstrapTask>();
        }

        public static void RunBootstrap(this IServiceCollection services)
        {
            services.AddSingleton<BootstrapRunner>();
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<BootstrapRunner>();
                runner.Run();
            }
        }
    }
}
