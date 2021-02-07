using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartControl.Models
{
    public class DeviceType
    {
        public long DeviceTypeID { get; set; }
        public string DeviceTypeName { get; set; }
        public string Icon { get; set; }
        public string Unit { get; set; }

        //Navigation Properties
        public ICollection<Device> Devices { get; set; }
    }
}
