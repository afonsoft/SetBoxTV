using Hangfire.Server;

namespace SetBoxWebUI.Interfaces
{
    public interface IHangfireJob
    {
        void Initialize();
        void JobGetNewFiles(PerformContext context);
        void JobDeleteOldFiles(PerformContext context);
    }
}
