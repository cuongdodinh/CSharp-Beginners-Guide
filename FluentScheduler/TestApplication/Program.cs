using FluentScheduler;
using System;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            ListenForStart();
            ListenForEnd();
            ListenForException();
            Initialize();
            Sleep();
        }


        private static void ListenForStart()
        {
            Console.WriteLine("[job start]", "{0} has started.");
            JobManager.JobStart += (info) => Console.WriteLine("[job start]", info.Name);
        }

        private static void ListenForEnd()
        {
            Console.WriteLine("[job end]", "{0} has ended{1}.");

            JobManager.JobEnd += (info) =>
                Console.WriteLine("[job end]", info.Name,
                    info.Duration > TimeSpan.FromSeconds(1) ? " with duration of " + info.Duration : string.Empty);
        }

        private static void ListenForException()
        {
            Console.WriteLine("[job exception]", "An error just happened:" + Environment.NewLine + "{0}");
            JobManager.JobException += (info) => Console.WriteLine("[job exception]", info.Exception);
        }

        private static void Initialize()
        {
            JobManager.Initialize(new MyRegistry());
            JobManager.RemoveJob("[removed]");

            Console.WriteLine("[late]");
            JobManager.AddJob(() => Console.WriteLine("[late]", "This was added after the initialize call."),
                s => s.WithName("[late]").ToRunNow());
        }

        private static void Sleep()
        {
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
