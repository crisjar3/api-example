using Ardalis.Result;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace NetForemost.Core.Services.Reports;

public static class FileCSVHelper
{
    public static Result<byte[]> GetCSVBytes<T>(IEnumerable<T> record)
    {
        try
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvWriter.WriteRecords(record);
                writer.Flush();
                var csvBytes = memoryStream.ToArray();
                return csvBytes;
            }
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public static void DeleteFile(string filePath)
    {
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                Console.WriteLine($"The file {filePath} has been successfully deleted.");
            }
            else
            {
                Console.WriteLine($"The file {filePath} does not exist in the directory.");
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error trying to delete file: {ex.Message}");
        }
    }
}
