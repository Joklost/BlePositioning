using System;
using System.Collections.Generic;
using System.Linq;
using Rosenbjerg.DepMan;
using Xamarin.Forms;

namespace BLE
{
    public partial class App
    {
        public IBleHelper BleHelper { get; }

        public App()
        {
            InitializeComponent();
            var scanner = DependencyManager.Get<BleScannerBase>();
#if __ANDROID__
            BleHelper = new AndroidBleHelper();
#endif
#if __IOS__
            BleHelper = new iOSBleHelper();
#endif
            var scanButton = CreateGreenButton("Scan Nearby Devices");
            scanButton.Clicked += OnScanButtonClicked;
            var trackButton = CreateGreenButton("Track Selected Devices");
            trackButton.Clicked += OnTrackButtonClicked;
            var calcButton = CreateGreenButton("Calculate Values");
            calcButton.Clicked += OnCalcButtonClicked;

            var layout = new StackLayout
            {
                //Spacing = 20,
                //Padding = 50,
                //VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    scanButton,
                    trackButton,
                    calcButton
                }
            };
            MainPage = new NavigationPage(new MainPage
            {
                Title = "Bluetooth Low Energy Scanner",
                Content = layout
            });

        }

        private async void OnScanButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            await btn.Navigation.PushAsync(new ScanPage(BleHelper));
        }

        private async void OnTrackButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            await btn.Navigation.PushAsync(new TrackPage(BleHelper));
        }

        private async void OnCalcButtonClicked(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            await btn.Navigation.PushAsync(new CalcPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private bool IsEffective(int sampleRssi, IReadOnlyList<int> sampleList, double threshold)
        {
            // sampleList.Count should be larger than or equal to 2
            var deltaList = new List<int>();
            for (var i = 0; i < sampleList.Count - 2; i++)
            {
                var delta = sampleList[i + 1] - sampleList[i];
                deltaList.Add(delta);
            }

            var averageDelta = deltaList.Average();
            var currentDelta = sampleRssi - sampleList.Last();
            var deltaRatio = Math.Abs(currentDelta / averageDelta);

            return deltaRatio < threshold;
        }

        private static Button CreateGreenButton(string text)
        {
            return new Button
            {
                Text = text,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("77D065"),
                HorizontalOptions = LayoutOptions.Center
            };
        }
    }
}