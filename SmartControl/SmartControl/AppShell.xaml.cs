using SmartControl.ViewModels;
using SmartControl.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SmartControl
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewDevicePage), typeof(NewDevicePage));
            Routing.RegisterRoute(nameof(NewRoomPage), typeof(NewRoomPage));
            Routing.RegisterRoute(nameof(EditDevicePage), typeof(EditDevicePage));
            Routing.RegisterRoute(nameof(DeviceDetailPage), typeof(DeviceDetailPage));
            Routing.RegisterRoute(nameof(SensorDetailPage), typeof(SensorDetailPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
