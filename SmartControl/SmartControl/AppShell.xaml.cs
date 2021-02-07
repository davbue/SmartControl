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
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewDevicePage), typeof(NewDevicePage));
            Routing.RegisterRoute(nameof(DeviceDetailPage), typeof(DeviceDetailPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
