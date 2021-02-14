using SmartControl.Models;
using SmartControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartControl.Views
{
    public partial class NewRoomPage : ContentPage
    {
        public Models.Room Room { get; set; }

        public NewRoomPage()
        {
            InitializeComponent();
            BindingContext = new NewDeviceViewModel();
        }
    }
}