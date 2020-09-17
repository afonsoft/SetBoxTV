using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using SetBoxWebUI.Interfaces;
using System;

namespace SetBoxWebUI.Jobs
{
    public class HangfireJob : IHangfireJob
    {
        public void Initialize()
        {
            RecurringJob.AddOrUpdate<IHangfireJob>("Delete Files In Db", x => x.JobDeleteOldFiles(null), "*/15 * * * *", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireJob>("Create Files In Db", x => x.JobGetNewFiles(null), "*/5 * * * *", TimeZoneInfo.Local);
        }

        public void JobDeleteOldFiles(PerformContext context)
        {
            try
            {
                context.WriteLine("Job Inicializado");
                //Do
                context.WriteLine("Job Finalizado");
            }
            catch (Exception ex)
            {
                context.WriteLine($"Erro no Job : {ex}");
            }
        }

        public void JobGetNewFiles(PerformContext context)
        {
            try
            {
                context.WriteLine("Job Inicializado");
                //Do
                context.WriteLine("Job Finalizado");
            }
            catch (Exception ex)
            {
                context.WriteLine($"Erro no Job : {ex}");
            }
        }
    }
}
