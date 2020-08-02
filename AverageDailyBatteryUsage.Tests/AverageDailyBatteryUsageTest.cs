using AverageDailyBatteryUsage.BusinessLayer.Managers;
using AverageDailyBatteryUsage.BusinessLayer.Managers.Contracts;
using AverageDailyBatteryUsage.DataLayer.Repositories;
using NUnit.Framework;

namespace AverageDailyBatteryUsage.Tests
{
    /// <summary>
    /// Test script to execute test on average daily usage of device battery.
    /// </summary>
    [TestFixture]
    public class AverageDailyUsageBatteryTests
    {
        private IBatteryUsageCalculateManager manager;
        /// <summary>
        /// Initial setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            manager = new BatteryUsageCalculateManager(new BatteryUsageCalculatorRepository());
        }
        /// <summary>
        /// Test method to verify the result.
        /// </summary>
        [Test]
        public void VerifyAverageDailyUsageBatteryCalculated()
        {
            var readings = manager.CalculateAverageDailyBatteryUsage();
            Assert.IsTrue(readings.Count > 0);
            Assert.IsTrue(readings.TrueForAll(reading => !string.IsNullOrEmpty(reading.Item1) && !string.IsNullOrEmpty(reading.Item2)));
        }
    }
}