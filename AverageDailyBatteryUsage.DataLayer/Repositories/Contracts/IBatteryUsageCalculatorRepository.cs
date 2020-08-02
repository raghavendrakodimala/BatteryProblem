namespace AverageDailyBatteryUsage.DataLayer.Repositories.Contracts
{
    /// <summary>
    /// Interface for the implementation of repository which operates on reading/writing data.
    /// </summary>
    public interface IBatteryUsageCalculatorRepository
    {
        public string ReadJsonFromFile(string path);
    }
}
