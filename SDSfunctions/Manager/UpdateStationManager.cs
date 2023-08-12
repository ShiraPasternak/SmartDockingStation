using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace SDS.Function
{
    public static class UpdateStationManager
    {
        public class StationUpdate
        {
            public int StationId { get; set; }
            public decimal HourlyRate { get; set; }
        }

        [FunctionName("UpdateStationManager")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "manage/stations/update")] HttpRequest req
        )
        {
            var requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var stationUpdate = JsonConvert.DeserializeObject<StationUpdate>(requestBody);

            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString"));
            connection.Open();
            var query = $"EXEC UpdateStationManager {stationUpdate.StationId}, {stationUpdate.HourlyRate};";
            using var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
            return new OkResult();
        }
    }
}
