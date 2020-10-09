using Hangfire.Server;
using System.Threading.Tasks;

namespace SetBoxWebUI.Interfaces
{
    public interface IHangfireJob
    {
        void Initialize();
        Task JobGetNewFiles(PerformContext context);
        Task JobDeleteFilesNotExist(PerformContext context);
    }
}
