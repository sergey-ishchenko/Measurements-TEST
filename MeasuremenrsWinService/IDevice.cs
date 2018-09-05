using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeasuremenrsWinService.Models;

namespace MeasuremenrsWinService
{
    interface IDevice
    {
        int DeviceId { get; }
        IEnumerable<DeviceData> GetData();
        void SaveLastValueToConfig();
    }
}
