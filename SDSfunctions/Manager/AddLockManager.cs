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
    public static class AddLockManager
    {
        public class LockData
        {
            public int StationId { get; set; }
            public string Name { get; set; }
            public string Mac { get; set; }
        }

        [FunctionName("AddLockManager")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "manage/locks/add")] HttpRequest req
        )
        {
            if (!AuthenticationManager.Authenticate(req))
            {
                return new BadRequestResult();
            }

            var requestBody = string.Empty;
            using (var streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            var lockData = JsonConvert.DeserializeObject<LockData>(requestBody);

            using var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString"));
            connection.Open();
            var query = $"EXEC AddLockManager {lockData.StationId}, '{lockData.Name}', '{lockData.Mac}';";
            using var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
            return new OkResult();
        }
    }
}
