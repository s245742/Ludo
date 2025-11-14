using Ludo;
using LudoClient.Commands;
using LudoClient.Models;
using LudoClient.Services;
using LudoClient.Stores;
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.InGameViewModels;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Models;
using SharedModels.TransferMsg;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LudoClient.ViewModels.PreGameViewModels
{
    public class JoinGameViewModel : ViewModelBase
    {
        public ICommand NavigateStartScreenCommand { get; }
        public ICommand NavigateToGameCommand { get; }

        private RelayCommand<Game> deleteGameCommand;
        public RelayCommand<Game> DeleteGameCommand
        {
            get { return deleteGameCommand; }
        }

        private ObservableCollection<Game> gamesList;
        public ObservableCollection<Game> GamesList
        {
            get { return gamesList; }
            set { gamesList = value; OnPropertyChanged("GamesList"); }
        }

        private RelayCommand<Game> joinGameCommand;
        public RelayCommand<Game> JoinGameCommand
        {
            get { return joinGameCommand; }
        }

        private CurrPlayersStore _currPlayersStore;

        public JoinGameViewModel(NavigationStore navigationStore, CurrPlayersStore currPlayersStore)
        {
            _currPlayersStore = currPlayersStore;

            //navigation
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            NavigateToGameCommand = new NavigateCommand<GameViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<GameViewModel>());
            //commands
            deleteGameCommand = new RelayCommand<Game>(DeleteGame);
            joinGameCommand = new RelayCommand<Game>(JoinGame);
            //load games
            LoadData();
        }

        
        //display games
        private async void LoadData()
        {
            //Send
            try
            {
                var network = new NetworkService();
                await network.ConnectAsync("127.0.0.1", 5000);

                var envelope = new MessageEnvelope
                {
                    MessageType = "GetAllGames",
                    Payload = System.Text.Json.JsonSerializer.Serialize("empty")
                };
                var json = System.Text.Json.JsonSerializer.Serialize(envelope);
                //Send and receive json of games
                var res = await network.SendMessageAsync(json);

                var gamesList = System.Text.Json.JsonSerializer.Deserialize<List<Game>>(res);
                //display
                GamesList = new ObservableCollection<Game>(gamesList);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Connection failed: {ex.Message}");
            }
            
        }
        

        //param game is passed with command.parameter (note we need a generic relaycommand here sicne delete taks a param)
        public async void DeleteGame(Game game)
        {
            var result = MessageBox.Show($"Are you sure you want to delete game \"{game.Game_Name}\"?",
                         "Confirmation",
                         MessageBoxButton.YesNo,
                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                
                try
                {
                    var network = new NetworkService();
                    await network.ConnectAsync("127.0.0.1", 5000);

                    var envelope = new MessageEnvelope
                    {
                        MessageType = "DeleteGame",
                        Payload = System.Text.Json.JsonSerializer.Serialize(game)
                    };
                    var json = System.Text.Json.JsonSerializer.Serialize(envelope);
                    //Send json of game
                    await network.SendMessageAsync(json);

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Connection failed: {ex.Message}");
                }
                LoadData(); //reload displays
            }
        }

        public async void JoinGame(Game game)
        {
            try
            {
                var network = new NetworkService();
                await network.ConnectAsync("127.0.0.1", 5000);

                var envelope = new MessageEnvelope
                {
                    MessageType = "JoinGame",
                    Payload = System.Text.Json.JsonSerializer.Serialize(game)
                };
                var json = System.Text.Json.JsonSerializer.Serialize(envelope);
                //Send json of game
                var res = await network.SendMessageAsync(json);
                var gamePlayersList = System.Text.Json.JsonSerializer.Deserialize<List<Player>>(res);
                ObservableCollection<Player> GamePlayers = new ObservableCollection<Player>(gamePlayersList);
                _currPlayersStore.SetPlayers(GamePlayers);
                NavigateToGameCommand.Execute(null);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Connection failed: {ex.Message}");
            }

        }



    }
}
