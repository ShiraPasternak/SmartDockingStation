﻿using AndroidX.Lifecycle;
using SDSApplication.MSALClient;
using SDSApplication.ViewModel;
using SDSApplication.ViewModels;

namespace SDSApplication;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient client = new();
    //private readonly String apiBaseUrl = "https://azurefunctions.azurewebsites.net"; // Update
    private MapViewModel mapViewModel;

    public MainPage()
	{
		InitializeComponent();
	}

    private async void Scan_Lock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StartParkingPage());
    }

    private async void Scan_Barcode_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BarcodeScanningPage());
    }

    private async void Scan_Bluetooth_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new HomePage(homeViewModel));
        await Shell.Current.GoToAsync("//HomePage", true);
    }

    private async void Map_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.MapPage(mapViewModel));
    }
    private async void LogOutButton_Clicked(object sender, EventArgs e)
    {
        await PublicClientSingleton.Instance.SignOutAsync().ContinueWith((t) =>
        {
            return Task.CompletedTask;
        });

        await Shell.Current.GoToAsync("mainview");
    }

    private async void CurrentParkingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CurrentParkingsPage());
    }

    private async void ParkingHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ParkingsHistoryPage());
    }

}

