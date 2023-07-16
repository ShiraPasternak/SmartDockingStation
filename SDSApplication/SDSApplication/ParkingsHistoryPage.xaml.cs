using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Android.Content;
using System.Net.Http.Json;

namespace SDSApplication
{
    public partial class ParkingsHistoryPage : ContentPage
    {
        private static readonly HttpClient client = new();
        private readonly string apiBaseUrl = "https://azurefunctions.azurewebsites.net"; // Update
        private readonly Guid userId = new Guid("11111111-1111-1111-1111-111111111111");

        public ParkingsHistoryPage()
        {
            InitializeComponent();
            LoadUserParkingHistory();
        }

        private async void LoadUserParkingHistory()
        {
            var parkingHistory = await GetUserCurrentParkingProcessesMock();
            listParkingHistory.ItemsSource = parkingHistory;
        }   

        private async Task<List<ParkingHistoryProcessItem>> GetUserCurrentParkingProcesses()
        {
            var url = apiBaseUrl + "/api/GetUserCurrentParkingProcesses/";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Content = JsonContent.Create(new { user_id = userId }),
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var parkingHistory = JsonConvert.DeserializeObject<List<ParkingHistoryProcessItem>>(json);
            return parkingHistory;
        }

        private async Task<List<ParkingHistoryProcessItem>> GetUserCurrentParkingProcessesMock()
        {
            var mockResponseContent = JsonConvert.SerializeObject(new List<ParkingHistoryProcessItem>
            {
                new ParkingHistoryProcessItem { lockId = new Guid("a6d8b7e2-834c-4f0e-9c48-13a3bcb1568f") , startTime = new DateTime(2023, 7, 10, 10, 30, 0), endTime = new DateTime(2023, 7, 12, 10, 30, 0),
                                        duration = 48, cost =  100 },
                new ParkingHistoryProcessItem { lockId = new Guid("b0a9c5d3-132a-48f1-8d65-39c206c3656e"), startTime = new DateTime(2023, 7, 10, 15, 0, 0), endTime = new DateTime(2023, 7, 12, 15, 30, 0),
                                        duration = 1, cost =  10 },
                new ParkingHistoryProcessItem { lockId = new Guid("e7d984f9-5d09-4165-a14c-650b4b9ae7db"), startTime = new DateTime(2023, 7, 10, 8, 15, 0), endTime = new DateTime(2023, 7, 12, 10, 15, 0),
                                        duration = 2, cost =  20 }
            }) ;
            var parkingHistory = JsonConvert.DeserializeObject<List<ParkingHistoryProcessItem>>(mockResponseContent);
            return parkingHistory;
        }
    }
}
