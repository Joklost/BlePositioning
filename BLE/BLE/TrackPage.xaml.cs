using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackPage : ContentPage
    {
        private readonly IBleHelper _ble;
        private readonly StackLayout _layout;
        private readonly Label _scanProgress;
        private readonly Entry _scanEntry;
        private readonly Dictionary<string, Label> _labelDictionary;


        public TrackPage(IBleHelper bleHelper)
        {
            InitializeComponent();

            _ble = bleHelper;
            _labelDictionary = new Dictionary<string, Label>();

            _scanProgress = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };

            _scanEntry = new Entry
            {
                Placeholder = "Seconds",
                Keyboard = Keyboard.Numeric,
                Text = "60"
            };
            var scanButton = new Button
            {
                Text = "Start Scan",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("77D065"),
                HorizontalOptions = LayoutOptions.Center
            };
            scanButton.Clicked += OnScanButtonClicked;

            var scanStackLayout = new StackLayout
            {
                Children = { _scanEntry, scanButton},
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            _layout = new StackLayout
            {
                //VerticalOptions = LayoutOptions.Center,
                Children = {scanStackLayout, _scanProgress}
            };

            Title = "Track Selected Devices";
            Content = _layout;

            foreach (var btAddr in _ble.FilterAddressList)
            {
                var label = new Label
                {
                    Text = btAddr,
                    HorizontalOptions = LayoutOptions.Center
                };
                _labelDictionary.Add(btAddr, label);
                _layout.Children.Add(label);
            }
        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            btn.IsEnabled = false;
            _scanProgress.Text = "Scanning devices...";
            await _ble.ContinousScan((s, a) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var btAddr = _ble.GetAddress(a.Device);
                    _labelDictionary[btAddr].Text = btAddr + " " + a.Device.Rssi;
                });
            }, int.Parse(_scanEntry.Text));
            _scanProgress.Text = "Scan completed";
            btn.IsEnabled = true;
        }
    }
}