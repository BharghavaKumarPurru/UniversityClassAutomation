using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace UniversityClassAutomation.Schedulers
{
    public class Scheduler
    {
        public static async Task Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<AutomationJob>()
                                .WithIdentity("automationJob", "group1")
                                .Build();

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity("trigger1", "group1")
                                        .StartNow()
                                        .WithSimpleSchedule(x => x.WithIntervalInMinutes(15).RepeatForever())
                                        .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }

    public class AutomationJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var test = new Tests.ClassAutomationTest();
            test.SetUp();
            try
            {
                test.AutomateClassSearch();
            }
            finally
            {
                test.TearDown();
            }

            return Task.CompletedTask;
        }
    }
}
