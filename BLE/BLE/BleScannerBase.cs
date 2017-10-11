using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace BLE
{
    public abstract class BleScannerBase
    {
        protected BleScannerBase()
        {
            Ble = CrossBluetoothLE.Current;
            Adapter = Ble.Adapter;
        }

        protected IBluetoothLE Ble { get; }
        protected IAdapter Adapter { get; }

        public abstract Task<List<BleDevice>> ScanDevices(int limit = 10, int scanTimeout = 10000);

        public class BleDevice
        {
            public string Address { get; set; }
            public int RSSI { get; set; }
        }
    }
}