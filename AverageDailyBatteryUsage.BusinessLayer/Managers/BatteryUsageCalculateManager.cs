using AverageDailyBatteryUsage.BusinessLayer.Managers.Contracts;
using AverageDailyBatteryUsage.DataLayer.Repositories.Contracts;
using AverageDailyBatteryUsage.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AverageDailyBatteryUsage.BusinessLayer.Managers
{
    /// <summary>
    /// Manager class to execute business logic by reading JSON string from data layer
    /// </summary>
    public class BatteryUsageCalculateManager : IBatteryUsageCalculateManager
    {
        private readonly IBatteryUsageCalculatorRepository repository;
        public BatteryUsageCalculateManager(IBatteryUsageCalculatorRepository repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Calculates the battery average usage based on json file, by grouping by serial code and sorting by timestamp.
        /// Delegates the calcuation of average daily battery usage to the private method to get the calculated average
        /// </summary>
        /// <returns>List of Tuple which stores Device serial code and average value calculated</returns>
        public List<Tuple<string, string>> CalculateAverageDailyBatteryUsage()
        {
            try
            {
                var assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                assemblyPath = assemblyPath.Replace(string.Concat(assemblyName, ".dll"), "");
                dynamic average = 0;
                List<Tuple<string, string>> track = new List<Tuple<string, string>>();
                var json = this.repository.ReadJsonFromFile(string.Concat(assemblyPath, "Resources\\", "battery.json"));
                if (!string.IsNullOrEmpty(json))
                {
                    List<AverageDailyBatteryUsageRecordingEnitity> averageBatteryUsage = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AverageDailyBatteryUsageRecordingEnitity>>(json);
                    if (averageBatteryUsage.Count > 0)
                    {
                        var deviceList = averageBatteryUsage.GroupBy(b => b.SerialNumber); //Grouping by device serial code.
                        foreach (var device in deviceList)
                        {
                            average = 0;
                            var deviceChargingReadings = device.OrderBy(s => s.TimeStamp).ToArray(); //Sorting based on timestamp in ascending order
                            //Converting Battery levels from decimals to integral parts
                            foreach (var reading in deviceChargingReadings)
                            {
                                reading.BatteryLevel = reading.BatteryLevel * 100;
                            }
                            var readingByDate = deviceChargingReadings.GroupBy(r => r.TimeStamp.Date);//Grouping readings by date wise for each device
                            foreach (var reading in readingByDate)
                            {
                                average = CalculateAverageUsage(ref average, reading); //calculating average value for the day.
                                track.Add(new Tuple<string, string>(device.Key, Convert.ToString(average)));
                            }
                        }
                    }
                }
                return track;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Calculates the daily average usage of battery until the period it has been recorded in the json file.
        /// This private method engaged by the public method after grouping by device serial and sorting by timestamp.
        /// </summary>
        /// <param name="average">Dynamic argument to hold the calculated average value or unknown string in case of single record in the day</param>
        /// <param name="reading">Group of readings sorted by timestamp recorded per day.</param>
        /// <returns>Dynamic value is returned either calculated average or Unknown string</returns>
        private static dynamic CalculateAverageUsage(ref dynamic average, IGrouping<DateTime, AverageDailyBatteryUsageRecordingEnitity> reading)
        {
            var batteryLevelsOrder = reading.OrderBy(x => x.TimeStamp).ToArray(); //Sorts reading by timestamp in ascending order
            int counter = 0;
            List<AverageDailyBatteryUsageRecordingEnitity> perDayBatteryLevels = new List<AverageDailyBatteryUsageRecordingEnitity>();
            //Checking if all the sorted readings battery levels are in descending order to verify battery discharging continuously without intermediate charge.
            //Ignoring values if battery is charged in between readings.
            for (int i = 0; i < batteryLevelsOrder.Length; i++)
            {
                if (i < batteryLevelsOrder.Length - 1 && batteryLevelsOrder[i].BatteryLevel >= batteryLevelsOrder[i + 1].BatteryLevel)
                    perDayBatteryLevels.Add(batteryLevelsOrder[i]);
                else if (i == batteryLevelsOrder.Length - 1) perDayBatteryLevels.Add(batteryLevelsOrder[i]);
            }
            //If there are multiple readings per day, calculate avere
            if (perDayBatteryLevels.Count > 1)
            {
                float timediff = Math.Abs(perDayBatteryLevels[0].TimeStamp.Hour - perDayBatteryLevels[perDayBatteryLevels.Count - 1].TimeStamp.Hour);
                if (timediff == 0)
                {
                    timediff = Math.Abs(perDayBatteryLevels[0].TimeStamp.Minute - perDayBatteryLevels[perDayBatteryLevels.Count - 1].TimeStamp.Minute);
                    timediff = timediff / 60;
                }
                if (float.TryParse(Convert.ToString(average), out float num))
                {
                    if (float.Parse(Convert.ToString(average)) == 0)
                    {
                        var diff = 24 / timediff;
                        average = Math.Abs(perDayBatteryLevels[0].BatteryLevel - perDayBatteryLevels[perDayBatteryLevels.Count - 1].BatteryLevel) * diff;
                    }
                    else
                    {
                        average = Convert.ToSingle(average) * 24 / (24 * (++counter) + timediff);
                    }
                }
            }
            else //else show it as unknown value
            {
                average = "Unknown";
            }
            return average;
        }
    }
}
