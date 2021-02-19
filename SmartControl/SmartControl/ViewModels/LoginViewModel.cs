using SmartControl.Services;
using SmartControl.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string connectionString;

        public LoginViewModel()
        {
            try
            {
                ConnectionString = Application.Current.Properties["ConnectionString"].ToString();
            }
            catch
            {
                Application.Current.Properties["ConnectionString"] = "temp";
            }
            
            SmartHubClient.ConnectionString = ConnectionString;
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(connectionString);
        }

        public string ConnectionString
        {
            get => connectionString;
            set => SetProperty(ref connectionString, value);
        }

        public Command SaveCommand { get; }

        private async void OnSave()
        {
            Application.Current.Properties["ConnectionString"] = ConnectionString;
            SmartHubClient.ConnectionString = ConnectionString;
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
