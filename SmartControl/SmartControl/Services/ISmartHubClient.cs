using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartControl.Services
{
    public interface ISmartHubClient
    {
        Task<IEnumerable<Device>> GetDevicesAsync();
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<IEnumerable<Gateway>> GetGatewaysAsync();
        Task<IEnumerable<DeviceType>> GetDeviceTypesAsync();
        Task<Device> GetDeviceByIdAsync(string deviceId);
        Task PostDataAsync(DataEntity dataEntity);
        Task CreateDeviceAsync(Device device);
        Task CreateRoomAsync(Room room);
        Task CreateGatewayAsync(Gateway gateway);
        Task<float> GetLastValue(string DeviceId);
        Task PutDeviceAsync(Device device);
        Task DeleteDeviceAsync(string deviceId);

        string ConnectionString { get; set; }
    }
}
