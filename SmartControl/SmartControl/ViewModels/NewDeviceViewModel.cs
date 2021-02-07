using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    public class NewDeviceViewModel : BaseViewModel
    {
        private string deviceName;
        private List<string> deviceTypeNames;
        private List<string> roomNames;
        private List<string> gatewayIds;
        private string selectedDeviceTypeName;
        private string selectedRoomName;
        private string selectedGatewayId;
        private bool gatewayPickerEnabled;
        public List<Room> Rooms { get; set; }
        public List<Gateway> Gateways { get; set; }
        public List<DeviceType> DeviceTypes { get; set; }

        public NewDeviceViewModel()
        {
            GatewayPickerEnabled = false;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            LoadRooms();
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public async void LoadRooms()
        {
            Rooms = (await SmartHubClient.GetRoomsAsync()).ToList();
            RoomNames = Rooms.Select(room => room.RoomName).ToList();
            DeviceTypes = (await SmartHubClient.GetDeviceTypesAsync()).ToList();
            DeviceTypeNames = DeviceTypes.Select(dt => dt.DeviceTypeName).ToList();
        }
        public async void LoadGateways()
        {
            Room room = Rooms.Single(r => r.RoomName == SelectedRoomName);
            Gateways = (await SmartHubClient.GetGatewaysAsync()).Where(gateway => gateway.RoomId == room.RoomId).ToList();
            GatewayIds = Gateways.Select(gateway => gateway.GatewayId.ToString()).ToList();
            GatewayPickerEnabled = true;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(deviceName)
                && !String.IsNullOrWhiteSpace(selectedDeviceTypeName)
                && !String.IsNullOrWhiteSpace(selectedRoomName)
                && !String.IsNullOrWhiteSpace(selectedGatewayId);

        }
        public bool GatewayPickerEnabled
        {
            get => gatewayPickerEnabled;
            set => SetProperty(ref gatewayPickerEnabled, value);
        }
        public string DeviceName
        {
            get => deviceName;
            set => SetProperty(ref deviceName, value);
        }

        public string SelectedDeviceTypeName
        {
            get => selectedDeviceTypeName;
            set => SetProperty(ref selectedDeviceTypeName, value);
        }
        public string SelectedRoomName
        {
            get => selectedRoomName;
            set { SetProperty(ref selectedRoomName, value); LoadGateways(); OnPropertyChanged(); }
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {

            Models.Device newDevice = new Models.Device()
            {
                DeviceId = Guid.NewGuid().ToString(),
                DeviceName = DeviceName,
                Enabled = false,
                GatewayId = SelectedGatewayId,
                DeviceTypeId = DeviceTypes.Single(dt => dt.DeviceTypeName == SelectedDeviceTypeName).DeviceTypeID
            };

            await SmartHubClient.CreateDeviceAsync(newDevice);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
