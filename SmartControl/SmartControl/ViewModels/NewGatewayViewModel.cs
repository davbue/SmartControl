using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    public class NewGatewayViewModel : BaseViewModel
    {
        private string gatewayId;
        private string selectedRoomName;

        public NewGatewayViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(selectedRoomName);

        }

        public string GatewayId
        {
            get => gatewayId;
            set => SetProperty(ref gatewayId, value);
        }

        public string SelectedRoomName
        {
            get => selectedRoomName;
            set => SetProperty(ref selectedRoomName, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {

            Models.Gateway newGateway = new Models.Gateway()
            {
                GatewayId = GatewayId,
                RoomId = Guid.NewGuid().ToString()
            };

            await SmartHubClient.CreateGatewayAsync(newGateway);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
