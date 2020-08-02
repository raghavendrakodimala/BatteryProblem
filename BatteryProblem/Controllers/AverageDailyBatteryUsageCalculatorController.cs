using AverageDailyBatteryUsage.BusinessLayer.Managers.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AverageDailyBatteryUsage.Main.Controllers
{
    /// <summary>
    /// Default controller to calculate the average daily usage of device battery
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AverageDailyBatteryUsageCalculatorController : ControllerBase
    {
        private readonly IBatteryUsageCalculateManager manager;
        public AverageDailyBatteryUsageCalculatorController(IBatteryUsageCalculateManager manager)
        {
            this.manager = manager;
        }
        /// <summary>
        /// Description: Default route to calculate the average daily usage of device battery.
        /// HTTP Verb : GET
        /// </summary>
        /// <returns>Ok status result with the list of calculated average values per device</returns>
        [Route("")]
        [HttpGet]
        public IActionResult GetAverageDailyUsageOfBattery()
        {
            try
            {
                var averageDailyUsageReadings = this.manager.CalculateAverageDailyBatteryUsage();
                return Ok(averageDailyUsageReadings);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
