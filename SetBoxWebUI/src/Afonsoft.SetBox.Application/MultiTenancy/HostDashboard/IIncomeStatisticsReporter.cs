using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Afonsoft.SetBox.MultiTenancy.HostDashboard.Dto;

namespace Afonsoft.SetBox.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}