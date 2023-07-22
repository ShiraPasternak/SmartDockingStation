using SDSApplication.Control;
using SDSApplication.ViewModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Linq;

using System.Net.NetworkInformation;


namespace SDSApplication.Views
{
    public partial class MapPage : ContentPage
    {
        private List<CustomPin> pins;
        private Dictionary<CustomPin, string> pinStatusDictionary;
        private MapViewModel mapViewModel;

        public MapPage(MapViewModel mapViewModel)
        {
            InitializeComponent();

            this.mapViewModel = mapViewModel;
            BindingContext = mapViewModel;

            _ = InitializeMapAsync();
        }

        private async Task InitializeMapAsync()
        {
            var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(30)
            });
            double latitude = 32.136823861209905;
            double longitude = 34.83708761019526;

            pins = new List<CustomPin>();

            location ??= await Geolocation.Default.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(30)
            });

            for (int i = 0; i < 10; i++)
            {
                var customPin = new CustomPin
                {
                    
                    Address = $"{i + 1}/10 locks in use",
                    Label = $"Station {i + 1}",
                    Location = new Location(latitude + (i * 0.01), longitude + (i * 0.01)),
                    Type = PinType.Place,
                    Distance = Location.CalculateDistance(location.Latitude, location.Longitude, latitude + (i * 0.01), longitude + (i * 0.01), DistanceUnits.Kilometers),
                    Status = ((i + 1) == 10)
                };
                pins.Add(customPin);
            }

            pins = pins.OrderBy(pin =>
            {
                return Location.CalculateDistance(location.Latitude, location.Longitude, pin.Location.Latitude, pin.Location.Longitude, DistanceUnits.Kilometers);
            }).ToList();

            foreach (var pin in pins)
            {
            if (pin.Status)
                {
                    pin.ImageSource = ImageSource.FromFile("red_pin.png");
                }
                else if (!pin.Status)
                {
                    pin.ImageSource = ImageSource.FromFile("green_pin.png");
                }
                mappy.Pins.Add(pin);
            }

            pinList.ItemsSource = pins;

            if (location != null)
            {
                var mapSpan = new MapSpan(new Location(location.Latitude, location.Longitude), 0.01, 0.01);
                mappy.MoveToRegion(mapSpan);
            }

            pinList.ItemsSource = pins;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        private void PinTapped(object sender, PinClickedEventArgs e)
        {
            if (sender is Pin pin)
            {
                mappy.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Location, Distance.FromKilometers(0.5)));
            }
        }

        private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Pin pin)
            {
                mappy.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Location, Distance.FromKilometers(0.5)));
            }

            if (sender is ListView listView)
            {
                listView.SelectedItem = null;
            }
        }

        private void OnMarkerClicked(object sender, PinClickedEventArgs e)
        {
            e.HideInfoWindow = true;
        }
    }
}
