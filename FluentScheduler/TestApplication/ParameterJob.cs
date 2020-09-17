namespace TestApplication
{
    using FluentScheduler;
    using System;

    public class ParameterJob : IJob
    {
        public string Parameter { get; set; }

        static ParameterJob()
        {
            Console.WriteLine("[parameter]", "Just executed with parameter \"{0}\".");
        }

        public void Execute()
        {
            Console.WriteLine("[parameter]", Parameter);
        }
    }
}
