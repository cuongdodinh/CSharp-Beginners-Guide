namespace TestApplication
{
    using FluentScheduler;
    using System;

    public class DisposableJob : IJob, IDisposable
    {
        static DisposableJob()
        {
            Console.WriteLine("[disposable]");
        }

        public void Execute()
        {
            Console.WriteLine("[disposable]", "Just executed.");
        }

        public void Dispose()
        {
            Console.WriteLine("[disposable]", "Disposed properly.");
        }
    }
}
