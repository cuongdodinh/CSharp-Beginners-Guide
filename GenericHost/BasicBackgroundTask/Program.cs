using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MyService1>();
                    services.AddHostedService<MyService2>();
                    services.AddHostedService<MyService3>();
                })
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }

    class MyService1 : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1 * 1000, stoppingToken);
                Console.WriteLine("xxx");
            }
        }
    }
    class MyService2 : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5 * 1000, stoppingToken);
                Console.WriteLine("yyy");
            }
        }
    }
    class MyService3 : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10 * 1000, stoppingToken);
                Console.WriteLine("zzz");
            }
        }
    }
}
