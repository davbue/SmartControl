using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace SmartControl.Services
{
    class SmartHubClient : ISmartHubClient
    {
        private RestClient client;
        public SmartHubClient()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            client = new RestClient("http://192.168.43.173:45455/api/");
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            //Get Devices
            RestRequest request = new RestRequest("Devices", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            IEnumerable<Device> devices = JsonConvert.DeserializeObject<List<Device>>(response.Content);
            foreach(Device device in devices)
            {
                Device tmpDevice = await FillDevicePropertiesAsync(device);
                device.Room = tmpDevice.Room;
                device.Gateway = tmpDevice.Gateway;
                device.DeviceType = tmpDevice.DeviceType;
            }
            return devices;
        }

        public async Task<Device> GetDeviceByIdAsync(string deviceId)
        {
            //Get Device
            RestRequest request = new RestRequest($"Devices/{deviceId}", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            Device device = JsonConvert.DeserializeObject<Device>(response.Content);
            device = await FillDevicePropertiesAsync(device);

            return device;
        }

        private async Task<Device> FillDevicePropertiesAsync(Device device)
        {
            //Get Gateway
            RestRequest request = new RestRequest($"Gateways/{device.GatewayId}", Method.GET);
            IRestResponse response = await client.ExecuteAsync(request);
            device.Gateway = JsonConvert.DeserializeObject<Gateway>(response.Content);

            //Get Room
            request = new RestRequest($"Rooms/{device.Gateway.RoomId}", Method.GET);
            response = await client.ExecuteAsync(request);
            device.Room = JsonConvert.DeserializeObject<Room>(response.Content);

            //Get DeviceType
            request = new RestRequest($"DeviceTypes/{device.DeviceTypeId}", Method.GET);
            response = await client.ExecuteAsync(request);
            device.DeviceType = JsonConvert.DeserializeObject<DeviceType>(response.Content);

            return device;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            //Get Rooms
            RestRequest request = new RestRequest("Rooms", Method.GET);
            var response = await client.ExecuteAsync(request);
            List<Room> rooms = JsonConvert.DeserializeObject<List<Room>>(response.Content);
            return rooms;
        }

        public async Task<IEnumerable<Gateway>> GetGatewaysAsync()
        {
            //Get Rooms
            RestRequest request = new RestRequest("Gateways", Method.GET);
            var response = await client.ExecuteAsync(request);
            List<Gateway> gateways = JsonConvert.DeserializeObject<List<Gateway>>(response.Content);
            return gateways;
        }

        public async Task<IEnumerable<DeviceType>> GetDeviceTypesAsync()
        {
            //Get Rooms
            RestRequest request = new RestRequest("DeviceTypes", Method.GET);
            var response = await client.ExecuteAsync(request);
            List<DeviceType> deviceTypes = JsonConvert.DeserializeObject<List<DeviceType>>(response.Content);
            return deviceTypes;
        }

        public async Task<float> GetLastValue(string DeviceId)
        {
            RestRequest request = new RestRequest($"GetLastValue/{DeviceId}", Method.GET);
            var response = await client.ExecuteAsync(request);
            float value = (float)Convert.ToDouble(response.Content);
            return value;
        }

        public async Task CreateDeviceAsync(Device device)
        {
            RestRequest request = new RestRequest("Devices", Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(device);
            var response = await client.ExecuteAsync(request);
        }

        public async Task PutDeviceAsync(Device device)
        {
            RestRequest request = new RestRequest($"Devices/{device.DeviceId}", Method.PUT);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(device);
            var response = await client.ExecuteAsync(request);
        }

        public async Task PostDataAsync(DataEntity dataEntity)
        {
            RestRequest request = new RestRequest("PostDataEntity", Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(dataEntity);
            var response = await client.ExecuteAsync(request);
        }
    }
}
