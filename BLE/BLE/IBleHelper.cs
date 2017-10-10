using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace BLE
{
    public interface IBleHelper
    {
        IBluetoothLE Ble { get; }
        IAdapter Adapter { get; }
        List<string> FilterAddressList { get; set; }
        bool IsScanning { get; }
        bool Filter(IDevice device);
        Task<List<IDevice>> ScanDevices(int limit = 10);
        Task ContinousScan(EventHandler<DeviceEventArgs> callback, int seconds = 60);
        Task<bool> CancelScan();
        string GetAddress(IDevice device);
    }
}
