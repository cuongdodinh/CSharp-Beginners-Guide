namespace TestApplication
{
    using FluentScheduler;
    using System;
    using System.Threading;

    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            Welcome();

            NonReentrant();
            Reentrant();
            Disable();

            Faulty();
            Removed();
            Parameter();
            Disposable();

            FiveHundredMilliseconds();
            FiveMinutes();
            TenMinutes();
            Hour();
            Day();
            Weekday();
            Week();
        }

        private void Welcome()
        {
            Schedule(() => Console.Write("3, "))
                .WithName("[welcome]")
                .AndThen(() => Console.Write("2, "))
                .AndThen(() => Console.Write("1, "))
                .AndThen(() => Console.WriteLine("Live!"));
        }

        private void NonReentrant()
        {
            Console.WriteLine("[non reentrant]");

            Schedule(() =>
            {
                Console.WriteLine("[non reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }).NonReentrant().WithName("[non reentrant]").ToRunEvery(1).Seconds();
        }

        private void Reentrant()
        {
            Console.WriteLine("[reentrant]");

            Schedule(() =>
            {
                Console.WriteLine("[reentrant]", "Sleeping a minute...");
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }).WithName("[reentrant]").ToRunNow().AndEvery(1).Minutes();
        }

        private void Disable()
        {
            Console.WriteLine("[disable]");

            Schedule(() =>
            {
                JobManager.RemoveJob("[reentrant]");
                JobManager.RemoveJob("[non reentrant]");
                Console.WriteLine("[disable]", "Disabled the reentrant and non reentrant jobs.");
            }).WithName("[disable]").ToRunOnceIn(200).Seconds();
        }

        private void Faulty()
        {
            Console.WriteLine("[faulty]");

            Schedule(() =>
            {
                Console.WriteLine("[faulty]", "I'm going to raise an exception!");
                throw new Exception("I warned you.");
            }).WithName("[faulty]").ToRunEvery(20).Minutes();
        }

        private void Removed()
        {
            Console.WriteLine("[removed]");

            Schedule(() =>
            {
                Console.WriteLine("[removed]", "SOMETHING WENT WRONG.");
            }).WithName("[removed]").ToRunOnceIn(2).Minutes();
        }

        private void Parameter()
        {
            Schedule(new ParameterJob { Parameter = "Foo" }).WithName("[parameter]").ToRunOnceIn(10).Seconds();
        }

        private void Disposable()
        {
            Schedule<DisposableJob>().WithName("[disposable]").ToRunOnceIn(10).Seconds();
        }

        private void FiveHundredMilliseconds()
        {
            Console.WriteLine("[half a second]");

            Schedule(() => Console.WriteLine("[half a second]", "Half a second has passed."))
                .WithName("[half a second]").ToRunOnceIn(500).Milliseconds();
        }

        private void FiveMinutes()
        {
            Console.WriteLine("[five minutes]");

            Schedule(() => Console.WriteLine("[five minutes]", "Five minutes has passed."))
                .WithName("[five minutes]").ToRunOnceAt(DateTime.Now.AddMinutes(5)).AndEvery(5).Minutes();
        }

        private void TenMinutes()
        {
            Console.WriteLine("[ten minutes]");

            Schedule(() => Console.WriteLine("[ten minutes]", "Ten minutes has passed."))
                .WithName("[ten minutes]").ToRunEvery(10).Minutes();
        }

        private void Hour()
        {
            Console.WriteLine("[hour]");

            Schedule(() => Console.WriteLine("[hour]", "A hour has passed."))
                .WithName("[hour]").ToRunEvery(1).Hours();
        }

        private void Day()
        {
            Console.WriteLine("[day]");

            Schedule(() => Console.WriteLine("[day]", "A day has passed."))
                .WithName("[day]").ToRunEvery(1).Days();
        }

        private void Weekday()
        {
            Console.WriteLine("[weekday]");

            Schedule(() => Console.WriteLine("[weekday]", "A new weekday has started."))
                .WithName("[weekday]").ToRunEvery(1).Weekdays();
        }

        private void Week()
        {
            Console.WriteLine("[week]");

            Schedule(() => Console.WriteLine("[week]", "A new week has started."))
                .WithName("[week]").ToRunEvery(1).Weeks();
        }
    }
}
