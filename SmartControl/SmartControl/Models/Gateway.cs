using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartControl.Models
{
    public class Gateway
    {
        public string GatewayId { get; set; }
        public string RoomId { get; set; }

        //Navigation Properties
        public ICollection<Device> Devices { get; set; }
        public Room Room { get; set; }
    }
}
