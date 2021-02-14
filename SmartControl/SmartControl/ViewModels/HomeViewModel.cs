using SmartControl.Models;
using SmartControl.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

namespace SmartControl.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private Models.Device _selectedDevice;
        public ObservableCollection<Room> Rooms { get; }
        public Command LoadRoomsCommand { get; }
        public Command AddRoomCommand { get; }
        public Command<Models.Device> DeviceTapped { get; }
        public Command RoomTapped { get; }

        public HomeViewModel()
        {
            Title = "Home";
            Rooms = new ObservableCollection<Room>();
            LoadRoomsCommand = new Command(async () => await ExecuteLoadRoomsCommand());

            DeviceTapped = new Command<Models.Device>(OnDeviceSelected);
            RoomTapped = new Command(OnRoomTapped);

            AddRoomCommand = new Command(OnAddRoom);
        }

        async Task ExecuteLoadRoomsCommand()
        {
            IsBusy = true;

            try
            {
                Rooms.Clear();
                var rooms = await SmartHubClient.GetRoomsAsync();
                var devices = await SmartHubClient.GetDevicesAsync();
                foreach (var room in rooms)
                {
                    var roomDevices = devices.Where(d => d.Gateway.RoomId == room.RoomId);
                    room.Controls = roomDevices.Where(d => d.DeviceType.Sensor == false).ToList();
                    room.Sensors = roomDevices.Where(d => d.DeviceType.Sensor == true).ToList();
                    if (room.Controls.Any())
                    {
                        room.ControlsVisible = true;
                    }
                    else
                    {
                        room.ControlsVisible = false;
                    }

                    if (room.Sensors.Any())
                    {
                        room.SensorsVisible = true;
                    }
                    else
                    {
                        room.SensorsVisible = false;
                    }
                    foreach (Models.Device device in room.Sensors)
                    {
                        device.LastValue = await SmartHubClient.GetLastValue(device.DeviceId);
                        device.DeviceType.Icon = $"{Path.GetFileNameWithoutExtension(device.DeviceType.Icon)}_white.png";
                    }
                    Rooms.Add(room);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedDevice = null;
        }

        public Models.Device SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                SetProperty(ref _selectedDevice, value);
                OnDeviceSelected(value);
            }
        }

        private async void OnAddRoom(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewRoomPage));
        }

        async void OnRoomTapped()
        {
            
        }
        async void OnDeviceSelected(Models.Device device)
        {
            if (device == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(DeviceDetailPage)}?{nameof(DeviceDetailViewModel.DeviceId)}={device.DeviceId}");
            var check = $"{nameof(DeviceDetailPage)}?{nameof(DeviceDetailViewModel.DeviceId)}={device.DeviceId}";
        }
    }
}