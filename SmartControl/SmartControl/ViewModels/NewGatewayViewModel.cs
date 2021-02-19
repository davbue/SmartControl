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
        private List<string> roomNames;

        public List<Room> Rooms { get; set; }

        public NewGatewayViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            LoadRooms();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(selectedRoomName)
                && !String.IsNullOrWhiteSpace(gatewayId);

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
        public List<string> RoomNames
        {
            get => roomNames;
            set => SetProperty(ref roomNames, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadRooms()
        {
            Rooms = (await SmartHubClient.GetRoomsAsync()).ToList();
            RoomNames = Rooms.Select(room => room.RoomName).ToList();
        }

        private async void OnSave()
        {

            Models.Gateway newGateway = new Models.Gateway()
            {
                GatewayId = GatewayId,
                RoomId = Rooms.Single(r => r.RoomName == selectedRoomName).RoomId
            };

            await SmartHubClient.CreateGatewayAsync(newGateway);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
