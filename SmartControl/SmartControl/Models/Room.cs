using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartControl.Models
{
    public class Room
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }

        //Navigation Properties
        public ICollection<Gateway> Gateways { get; set; }
        public ICollection<Device> Controls { get; set; }
        public ICollection<Device> Sensors { get; set; }
        public bool ControlsVisible { get; set; }
        public bool SensorsVisible { get; set; }
    }
}
