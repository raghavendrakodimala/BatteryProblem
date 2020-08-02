using AverageDailyBatteryUsage.DataLayer.Repositories.Contracts;
using System;
using System.IO;

namespace AverageDailyBatteryUsage.DataLayer.Repositories
{
    /// <summary>
    /// Repository which 
    /// </summary>
    public class BatteryUsageCalculatorRepository : IBatteryUsageCalculatorRepository
    {
        /// <summary>
        /// Reads the json string from json file
        /// </summary>
        /// <param name="filePath">JSON file path that copied to output directory</param>
        /// <returns>JSON string</returns>
        public string ReadJsonFromFile(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    var json = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(json)) return json;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
