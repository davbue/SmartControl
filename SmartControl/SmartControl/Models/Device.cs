using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartControl.Models
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public bool Enabled { get; set; }
        public string GatewayId { get; set; }
        public long DeviceTypeId { get; set; }

        //Navigation Properties
        public Gateway Gateway { get; set; }
        public DeviceType DeviceType { get; set; }
        public Room Room { get; set; }
    }
}
