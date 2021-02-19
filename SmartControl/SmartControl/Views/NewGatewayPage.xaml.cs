using SmartControl.Models;
using SmartControl.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartControl.Views
{
    public partial class NewGatewayPage : ContentPage
    {
        public Models.Device Device { get; set; }

        public NewGatewayPage()
        {
            InitializeComponent();
            BindingContext = new NewGatewayViewModel();
        }
    }
}