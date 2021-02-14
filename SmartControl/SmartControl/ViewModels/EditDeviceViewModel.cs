using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public class EditDeviceViewModel : BaseViewModel
    {
        private string deviceId;
        private string deviceName;
        private string pin;
        private List<string> deviceTypeNames;
        private List<string> roomNames;
        private List<string> gatewayIds;
        private string selectedDeviceTypeName;
        private string selectedRoomName;
        private string selectedGatewayId;
        public Models.Device Device { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Gateway> Gateways { get; set; }
        public List<DeviceType> DeviceTypes { get; set; }
        public string Id { get; set; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command DeleteCommand { get; }
        public EditDeviceViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            DeleteCommand = new Command(OnDelete);
            LoadRooms();
            LoadAllGateways();
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public string DeviceName
        {
            get => deviceName;
            set => SetProperty(ref deviceName, value);
        }

        public string Pin
        {
            get => pin;
            set => SetProperty(ref pin, value);
        }

        public string SelectedDeviceTypeName
        {
            get => selectedDeviceTypeName;
            set => SetProperty(ref selectedDeviceTypeName, value);
        }
        public string SelectedRoomName
        {
            get => selectedRoomName;
            set 
            { 
                SetProperty(ref selectedRoomName, value);
                LoadGateways();
                OnPropertyChanged(); 
            }
        }
        public string SelectedGatewayId
        {
            get => selectedGatewayId;
            set => SetProperty(ref selectedGatewayId, value);
        }

        public List<string> DeviceTypeNames
        {
            get => deviceTypeNames;
            set => SetProperty(ref deviceTypeNames, value);
        }
        public List<string> RoomNames
        {
            get => roomNames;
            set => SetProperty(ref roomNames, value);
        }

        public List<string> GatewayIds
        {
            get => gatewayIds;
            set => SetProperty(ref gatewayIds, value);
        }

        public string DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
                LoadDeviceId(value);
            }
        }


        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDelete()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
            await SmartHubClient.DeleteDeviceAsync(Device.DeviceId);

        }

        public async void LoadRooms()
        {
            var test = await SmartHubClient.GetRoomsAsync();
            Rooms = (await SmartHubClient.GetRoomsAsync()).ToList();
            RoomNames = Rooms.Select(room => room.RoomName).ToList();
            DeviceTypes = (await SmartHubClient.GetDeviceTypesAsync()).ToList();
            DeviceTypeNames = DeviceTypes.Select(dt => dt.DeviceTypeName).ToList();
        }
        public async void LoadGateways()
        {
            Room room = Rooms.Single(r => r.RoomName == SelectedRoomName);
            Gateways = (await SmartHubClient.GetGatewaysAsync()).Where(gateway => gateway.RoomId == room.RoomId).ToList();
            GatewayIds = Gateways.Select(gateway => gateway.GatewayId).ToList();
            SelectedGatewayId = Device.GatewayId;
        }

        public async void LoadAllGateways()
        {
            Gateways = (await SmartHubClient.GetGatewaysAsync()).ToList();
            GatewayIds = Gateways.Select(gateway => gateway.GatewayId).ToList();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(deviceName)
                && !String.IsNullOrWhiteSpace(selectedDeviceTypeName)
                && !String.IsNullOrWhiteSpace(selectedRoomName)
                && !String.IsNullOrWhiteSpace(selectedGatewayId)
                && !String.IsNullOrWhiteSpace(pin);

        }

        private async void OnSave()
        {

            Models.Device newDevice = new Models.Device()
            {
                DeviceId = Guid.NewGuid().ToString(),
                DeviceName = DeviceName,
                Enabled = false,
                GatewayId = SelectedGatewayId,
                DeviceTypeId = DeviceTypes.Single(dt => dt.DeviceTypeName == SelectedDeviceTypeName).DeviceTypeID,
                Pins = Pin
            };

            await SmartHubClient.CreateDeviceAsync(newDevice);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadDeviceId(string deviceId)
        {
            try
            {
                Device = await SmartHubClient.GetDeviceByIdAsync(deviceId);
                Id = Device.DeviceId;
                DeviceName = Device.DeviceName;
                SelectedRoomName = Device.Room.RoomName;
                SelectedGatewayId = Device.GatewayId;
                Pin = Device.Pins;
                SelectedDeviceTypeName = Device.DeviceType.DeviceTypeName;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
