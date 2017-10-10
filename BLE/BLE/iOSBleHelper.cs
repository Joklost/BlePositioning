using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace BLE
{
    public class iOSBleHelper : IBleHelper
    {
        public IBluetoothLE Ble { get; }
        public IAdapter Adapter { get; }
        public List<string> FilterAddressList { get; set; }
        public bool IsScanning { get; }
        public bool Filter(IDevice device)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDevice>> ScanDevices(int limit)
        {
            throw new NotImplementedException();
        }

        public Task ContinousScan(EventHandler<DeviceEventArgs> callback, int seconds = 60)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelScan()
        {
            throw new NotImplementedException();
        }

        public string GetAddress(IDevice device)
        {
            throw new NotImplementedException();
        }
    }
}
