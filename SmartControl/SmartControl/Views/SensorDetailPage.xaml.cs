using SmartControl.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartControl.Views
{
    public partial class SensorDetailPage : ContentPage
    {
        SensorDetailViewModel model;
        public SensorDetailPage()
        {
            InitializeComponent();
            
            model = new SensorDetailViewModel();
            BindingContext = model;
        }
    }
}