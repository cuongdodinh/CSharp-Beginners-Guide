<p align="center">
    <a href="#fluentscheduler">
        <img alt="logo" src="Logo/logo-200x200.png">
    </a>
</p>

# FluentScheduler
Automated job scheduler with fluent interface.

* [Usage](#usage)
* [Stopping the scheduler](#stopping-the-scheduler)
* [Unexpected exceptions](#unexpected-exceptions)
* [Concurrent jobs](#concurrent-jobs)
* [Daylight Saving Time](#daylight-saving-time)
* [Milliseconds Accuracy](#milliseconds-accuracy)
* [Weekly jobs](#weekly-jobs)
* [Dependency Injection](#dependency-injection)
* [Contributing](#contributing)
  
  
## Usage

The job configuration is handled in a [Registry] class. A job is either an [Action] or a class that inherits [IJob]:

```cs
using FluentScheduler;

public class MyRegistry : Registry
{
    public MyRegistry()
    {
        // Schedule an IJob to run at an interval
        Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();

        // Schedule an IJob to run once, delayed by a specific time interval
        Schedule<MyJob>().ToRunOnceIn(5).Seconds();

        // Schedule a simple job to run at a specific time
        Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

        // Schedule a more complex action to run immediately and on an monthly interval
        Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

        // Schedule a job using a factory method and pass parameters to the constructor.
        Schedule(() => new MyComplexJob("Foo", DateTime.Now)).ToRunNow().AndEvery(2).Seconds();

        // Schedule multiple jobs to be run in a single schedule
        Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
    }
}
```

You can also use the [Registry] class directly (instead of inheriting it):

```cs
var registry = new Registry();
registry.Schedule<MyJob>().ToRunNow().AndEvery(2).Seconds();
registry.Schedule<MyJob>().ToRunOnceIn(5).Seconds();
registry.Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);
registry.Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);
registry.Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
```

With the registry ready you then need to initialize the [JobManager]. This is usually done as soon as your application is loaded (in the [Application_Start] method of a web application for example):

```cs
protected void Application_Start()
{
    JobManager.Initialize(new MyRegistry());
} 
```

It's also possible to schedule jobs after initialization:

```cs
JobManager.AddJob(() => Console.WriteLine("Late job!"), (s) => s.ToRunEvery(5).Seconds());
```

[JobManager]: Library/JobManager.cs
[Registry]:          Library/Registry.cs
[IJob]:              Library/IJob.cs
[Action]:            https://msdn.microsoft.com/library/System.Action
[Application_Start]: https://msdn.microsoft.com/library/ms178473

## Stopping the Scheduler

To stop the scheduler:

```cs
JobManager.Stop();
```

To both stop and wait for the running jobs to finish:

```cs
JobManager.StopAndBlock();
```

## Unexpected exceptions

To observe unhandled exceptions from your scheduled jobs listen for the JobException event on [JobManager]:

```cs
JobManager.JobException += info => Log.Fatal("An error just happened with a scheduled job: " + info.Exception);
```

## Concurrent jobs

By default, the library allows a schedule to run in parallel with a previously triggered execution of the
same schedule.

If you don't want such behaviour you can set a specific schedule as non-reentrant:

```cs
public class MyRegistry : Registry
{
    Schedule<MyJob>().NonReentrant().ToRunEvery(2).Seconds();
}
```

Or you can set it to all schedules of the registry at once:

```cs
public class MyRegistry : Registry
{
    NonReentrantAsDefault();
    Schedule<MyJob>().ToRunEvery(2).Seconds();
}
```

## Daylight Saving Time

Unfortunately, not unlike many schedulers, there is no daylight saving time support yet.

If you are worried about your jobs not running or running twice due to that, the suggestion is to either avoid troublesome time ranges or to force the use of UTC:

```cs
JobManager.UseUtcTime();
```

## Milliseconds Accuracy

The aim of the library is ease of use and flexibility, and not millisecond precision.  While the library has a millisecond unit, you should avoid relying on really small intervals (less than 100ms).

## Weekly jobs

Let's suppose it's 10:00 of a Monday morning and you want to start a job that runs every Monday at 14:00.
Should the first run of your job be today or only on the next week Monday?

If you want the former (not waiting for a whole week):

```cs
// Every "zero" weeks
Schedule<MyJob>().ToRunEvery(0).Weeks().On(DayOfWeek.Monday).At(14, 0);
```

Or if you want the latter (making sure that at least one week has passed):

```cs
// Every "one" weeks
Schedule<MyJob>().ToRunEvery(1).Weeks().On(DayOfWeek.Monday).At(14, 0);
```

## Dependency Injection

Currently, the library supports dependency injection of jobs (via IJobFactory). However, you shouldn't use it, it's bad idea on its way to be deprecated.
  
  