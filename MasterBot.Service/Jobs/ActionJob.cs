using System;
using System.Threading.Tasks;
using Quartz;

namespace MasterBot.Service.Jobs
{
    public class ActionJob : IJob
    {
        public static JobBuilder Create(Action action)
        {
            return JobBuilder.Create<ActionJob>()
                             .SetJobData(new JobDataMap
                              {
                                  { "action", action }
                              });
        }

        public Task Execute(IJobExecutionContext context)
        {
            if (context.MergedJobDataMap["action"] is Action action)
            {
                action();
            }

            return Task.CompletedTask;
        }
    }
}
