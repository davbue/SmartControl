using SmartControl.Services;
using SmartControl.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartControl
{
    public partial class App : Application
    {

        public App()
        {
            Device.SetFlags(new string[] { "Brush_Experimental" });
            InitializeComponent();
            DependencyService.Register<SmartHubClient>();
            MainPage = new AppShell();
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
