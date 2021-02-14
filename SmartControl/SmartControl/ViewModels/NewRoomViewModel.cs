using SmartControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartControl.ViewModels
{
    public class NewRoomViewModel : BaseViewModel
    {
        private string roomName;

        public NewRoomViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(roomName);

        }

        public string RoomName
        {
            get => roomName;
            set => SetProperty(ref roomName, value);
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

            Models.Room newRoom = new Models.Room()
            {
                RoomId = Guid.NewGuid().ToString(),
                RoomName = RoomName
            };

            await SmartHubClient.CreateRoomAsync(newRoom);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
