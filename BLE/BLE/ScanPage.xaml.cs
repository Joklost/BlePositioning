using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BLE
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private readonly StackLayout _layout;
        private readonly Label _scanProgress;
        private readonly IBleHelper _ble;

        public List<Button> ScanResultList { get; private set; }

        public ScanPage(IBleHelper bleHelper)
        {
            InitializeComponent();
            _ble = bleHelper;
            ScanResultList = new List<Button>();

            _scanProgress = new Label
            {
                HorizontalOptions = LayoutOptions.Center
            };
            var scanButton = new Button
            {
                Text = "Start Scan",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("77D065"),
                HorizontalOptions = LayoutOptions.Center
            };
            scanButton.Clicked += OnScanButtonClicked;

            _layout = new StackLayout
            {
                //VerticalOptions = LayoutOptions.Center,
                Children = {scanButton, _scanProgress}
            };

            Title = "Scan for Devices";
            Content = _layout;

        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            btn.IsEnabled = false;
            await ScanDevices();
            btn.IsEnabled = true;
        }

        private async Task ScanDevices()
        {
            _scanProgress.Text = "Scanning for devices...";
            ResetScanResults();
            var devs = await _ble.ScanDevices();
            _scanProgress.Text = "Scan completed!";

            foreach (var dev in devs)
            {
                var btn = new Button
                {
                    Text = _ble.GetAddress(dev) + ";" + dev.Rssi
                };
                btn.Clicked += OnScanResultButtonClicked;
                _layout.Children.Add(btn);
                ScanResultList.Add(btn);
            }
        }

        private void ResetScanResults()
        {
            foreach (var btn in ScanResultList)
            {
                _layout.Children.Remove(btn);
            }
            ScanResultList = new List<Button>();
            _ble.FilterAddressList.Clear();
        }

        private void OnScanResultButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            var btAddr = btn.Text.Split(';')[0].ToUpper();

            _ble.FilterAddressList.Add(btAddr);
            btn.IsEnabled = false;
        }
    }
}