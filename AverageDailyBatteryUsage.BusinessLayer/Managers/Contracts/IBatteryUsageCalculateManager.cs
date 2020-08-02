using System;
using System.Collections.Generic;

namespace AverageDailyBatteryUsage.BusinessLayer.Managers.Contracts
{
    public interface IBatteryUsageCalculateManager
    {
        public List<Tuple<string,string>> CalculateAverageDailyBatteryUsage();
    }
}
