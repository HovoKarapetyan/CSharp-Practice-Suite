using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AsyncReportGenerator
{
    public interface IReportService
    {
        Task GenerateReportAsync(string filePath, IEnumerable<string> data);
    }

    public class LocalFileReportService : IReportService
    {
        public async Task GenerateReportAsync(string filePath, IEnumerable<string> data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    foreach (var line in data)
                    {
                        await writer.WriteLineAsync(
                            $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC - {line}");
                    }
                }

                Console.WriteLine(
                    $"[SUCCESS] Report generated successfully at: {Path.GetFullPath(filePath)}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {

            List<string> sampleData = new List<string>
            {
                "System startup initialized successfully.",
                "Database connection established.",
                "Batch job #402 processed 150 items.",
                "Warning: CPU usage spiked above 80% for 3 seconds.",
                "System health check: PASSED."
            };

            IReportService reportService = new LocalFileReportService();
            string outputFilePath = "system_activity_report.txt";

            Console.WriteLine("Generating report asynchronously...");
            await reportService.GenerateReportAsync(outputFilePath, sampleData);

            Console.WriteLine("\nTask finished. Press any key to exit...");
            Console.ReadKey();
        }
    }
}