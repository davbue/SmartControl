using SmartControl.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartControl.Views
{
    public partial class DeviceDetailPage : ContentPage
    {
        DeviceDetailViewModel model;
        public DeviceDetailPage()
        {
            InitializeComponent();
            
            model = new DeviceDetailViewModel();
            BindingContext = model;
        }

        private void DynamicValue_DragCompleted(object sender, System.EventArgs e)
        {
            model.ValueChanged((float)(sender as Slider).Value);
        }
    }
}