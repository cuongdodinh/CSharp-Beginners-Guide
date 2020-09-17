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

            //Ctrl + C 또는 SIGTERM에 의해서 셧다운이 트리거 될때까지 호출측 스레드를 블럭한다
            await host.WaitForShutdownAsync();
            //host.WaitForShutdown();
			
			Console.WriteLine("Hello World! ~~");
        }
    }
}
