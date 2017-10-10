using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Bluetooth;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace BLE
{
    public class AndroidBleHelper : IBleHelper
    {
        public IBluetoothLE Ble { get; }
        public IAdapter Adapter { get; }
        public List<string> FilterAddressList { get; set; }
        public bool IsScanning { get; private set; }

        public AndroidBleHelper()
        {
            Ble = CrossBluetoothLE.Current;
            Adapter = Ble.Adapter;
            FilterAddressList = new List<string>();
            IsScanning = false;
        }


        public bool Filter(IDevice device)
        {
            var btDev = (BluetoothDevice) device.NativeDevice;
            return FilterAddressList.Contains(btDev?.Address.ToUpper());
        }

        public async Task<List<IDevice>> ScanDevices(int limit = 10)
        {
            if (IsScanning) return null;
            IsScanning = true;

            Adapter.ScanTimeout = 10000; // scan for 10 seconds
            var deviceList = new List<IDevice>();
            var i = 0;

            Adapter.DeviceDiscovered += (s, a) =>
            {
                if (i < limit)
                {
                    deviceList.Add(a.Device);
                }
                i++;
            };

            await Adapter.StartScanningForDevicesAsync();
            IsScanning = false;
            return deviceList;
        }

        public async Task ContinousScan(EventHandler<DeviceEventArgs> callback, int seconds = 60)
        {
            if (IsScanning) return;
            IsScanning = true;

            Adapter.ScanTimeout = seconds * 1000;
            Adapter.DeviceAdvertised += callback;

            await Adapter.StartScanningForDevicesAsync(deviceFilter: Filter);
            IsScanning = false;
        }

        public async Task<bool> CancelScan()
        {
            if (!IsScanning) return false;

            await Adapter.StopScanningForDevicesAsync();
            return true;
        }

        public string GetAddress(IDevice device)
        {
            var btDevice = (BluetoothDevice) device.NativeDevice;
            return btDevice.Address;
        }
    }
}