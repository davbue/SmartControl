using SmartControl.Models;
using SmartControl.ViewModels;
using SmartControl.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartControl.Views
{
    public partial class DevicesPage : ContentPage
    {
        DevicesViewModel _viewModel;

        public DevicesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new DevicesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void DevicePower_Toggled(object sender, ToggledEventArgs e)
        {
            if ((sender as Switch).IsToggled)
            {
                (sender as Switch).ThumbColor = Color.FromHex("#2E6661");
            }
            else
            {
                (sender as Switch).ThumbColor = Color.LightGray;
            }
            
        }
    }
}