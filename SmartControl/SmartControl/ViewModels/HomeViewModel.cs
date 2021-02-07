using SmartControl.Models;
using SmartControl.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private Models.Device _selectedDevice;

        public ObservableCollection<Models.Device> Devices { get; }
        public Command LoadDevicesCommand { get; }
        public Command AddDeviceCommand { get; }
        public Command<Models.Device> DeviceTapped { get; }

        public HomeViewModel()
        {
            Title = "Devices";
            Devices = new ObservableCollection<Models.Device>();
            LoadDevicesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            DeviceTapped = new Command<Models.Device>(OnDeviceSelected);

            AddDeviceCommand = new Command(OnAddDevice);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Devices.Clear();
                var devices = await SmartHubClient.GetDevicesAsync();
                foreach (var device in devices)
                {
                    Devices.Add(device);
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

        private async void OnAddDevice(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewDevicePage));
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