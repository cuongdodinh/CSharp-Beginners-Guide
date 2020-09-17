using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Basic
{
    class Program
    {        
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var host = new HostBuilder()
                .Build();

            await host.RunAsync();

            Console.WriteLine("Hello World! ~~");
        }
    }
}
