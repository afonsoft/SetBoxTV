using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Afonsoft.SetBox.EntityFrameworkCore;

namespace Afonsoft.SetBox.HealthChecks
{
    public class SetBoxDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public SetBoxDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("SetBoxDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("SetBoxDbContext could not connect to database"));
        }
    }
}
