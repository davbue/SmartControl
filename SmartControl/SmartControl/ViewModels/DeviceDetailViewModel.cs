using SmartControl.Models;
using SmartControl.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public class DeviceDetailViewModel : BaseViewModel
    {
        private bool enabled;
        private bool disabled;
        private string deviceId;
        private string deviceName;
        private string roomName;
        private string iconSource;
        private float deviceValue;
        private bool sliderVisibility;
        public Models.Device Device { get; set; }
        public string Id { get; set; }
        public Command OnOffCommand { get; }
        public Command EditDeviceCommand { get; }
        public DeviceDetailViewModel()
        {
            OnOffCommand = new Command(OnDeviceToggled);
            EditDeviceCommand = new Command(OnEditClicked);
        }
        public string DeviceName
        {
            get => deviceName;
            set => SetProperty(ref deviceName, value);
        }
        public string RoomName
        {
            get => roomName;
            set => SetProperty(ref roomName, value);
        }

        public string IconSource
        {
            get => iconSource;
            set => SetProperty(ref iconSource, value);
        }

        public bool SliderVisibility
        {
            get => sliderVisibility;
            set => SetProperty(ref sliderVisibility, value);
        }
        public bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }
        public bool Disabled
        {
            get => disabled;
            set => SetProperty(ref disabled, value);
        }

        public float DeviceValue
        {
            get => deviceValue;
            set => SetProperty(ref deviceValue, value);
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
        public void OnDeviceToggled()
        {
            bool enabled = !Device.Enabled;
            if (Device.Enabled)
            {
                IconSource = Device.DeviceType.Icon;
            }
            else
            {
                IconSource = System.IO.Path.GetFileNameWithoutExtension(Device.DeviceType.Icon) + "_white.png";
            }
            Enabled = enabled;
            Disabled = !enabled;
            Device.Enabled = enabled;
            SmartHubClient.PutDeviceAsync(Device);
            
        }

        public void ValueChanged(float value)
        {
            DataEntity dataEntity = new DataEntity
            {
                DeviceId = Device.DeviceId,
                Enabled = Device.Enabled,
                DeviceTypeId = Device.DeviceTypeId,
                Value = value
            };

            SmartHubClient.PostDataAsync(dataEntity);
        }

        public async void LoadDeviceId(string deviceId)
        {
            try
            {
                Device = await SmartHubClient.GetDeviceByIdAsync(deviceId);
                Id = Device.DeviceId;
                DeviceName = Device.DeviceName;
                RoomName = Device.Room.RoomName;
                IconSource = Device.DeviceType.Icon;
                if (Device.Enabled)
                {
                    IconSource = System.IO.Path.GetFileNameWithoutExtension(Device.DeviceType.Icon) + "_white.png";
                }
                else
                {
                    IconSource = Device.DeviceType.Icon;
                }
                if(Device.DeviceType.DeviceTypeName == "Relais" || Device.DeviceType.Sensor )
                {
                    SliderVisibility = false;
                }
                else
                {
                    SliderVisibility = true;
                }
                Enabled = Device.Enabled;
                Disabled = !Device.Enabled;
                DeviceValue = await SmartHubClient.GetLastValue(Device.DeviceId);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async void OnEditClicked()
        {
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(EditDevicePage)}?{nameof(EditDeviceViewModel.DeviceId)}={Device.DeviceId}");
        }
    }
}
