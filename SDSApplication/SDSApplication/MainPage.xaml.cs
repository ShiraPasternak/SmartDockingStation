﻿namespace SDSApplication;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient client = new();
    //private readonly String apiBaseUrl = "https://azurefunctions.azurewebsites.net"; // Update

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

    private async void Map_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MapPage());
    }

}

