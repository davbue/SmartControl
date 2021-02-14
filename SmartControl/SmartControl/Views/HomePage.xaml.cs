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
    public partial class HomePage : ContentPage
    {
        HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new HomeViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void ExpandBtn_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}