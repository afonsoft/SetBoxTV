using Hangfire.Server;

namespace SetBoxWebUI.Interfaces
{
    public interface IHangfireJob
    {
        void Initialize();
        void JobGetNewFiles(PerformContext context);
        void JobDeleteFilesNotExist(PerformContext context);
    }
}
