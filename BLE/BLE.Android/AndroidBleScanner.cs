using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Bluetooth;

namespace BLE.Droid
{
    class AndroidBleScanner : BleScannerBase
    {
        private bool _isScanning;

        public override async Task<List<BleDevice>> ScanDevices(int limit = 10, int scanTimeout = 10000)
        {
            if (_isScanning) return null;
            _isScanning = true;

            Adapter.ScanTimeout = scanTimeout; // scan for 10 seconds
            var deviceList = new List<BleDevice>();
            var i = 0;

            Adapter.DeviceDiscovered += (s, a) =>
            {
                var dev = a.Device.NativeDevice as BluetoothDevice;
                if (i++ < limit)
                {
                    deviceList.Add(new BleDevice
                    {
                        RSSI = a.Device.Rssi,
                        Address = dev.Address,
                    });
                }
                else
                {
                    Adapter.StopScanningForDevicesAsync();
                }
            };

            await Adapter.StartScanningForDevicesAsync();
            _isScanning = false;
            return deviceList;
        }
    }
}