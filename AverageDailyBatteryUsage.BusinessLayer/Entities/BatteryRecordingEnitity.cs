using System;

namespace AverageDailyBatteryUsage.Main.Models
{
    internal class AverageDailyBatteryUsageRecordingEnitity
    {
        public int AcademyId { get; set; }
        public float BatteryLevel { get; set; }
        public string EmployeeId { get; set; }
        public string SerialNumber { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
