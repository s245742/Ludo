using Ludo;
using LudoClient.Commands;
//using LudoClient.Models;
using LudoClient.Services;
using LudoClient.Stores;
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.InGameViewModels;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Models;
using SharedModels.Models.DTO;
using SharedModels.TransferMsg;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace LudoClient.ViewModels.PreGameViewModels
{
    public class JoinGameViewModel : ViewModelBase
    {
        private readonly NetworkService _networkService;
        public ICommand NavigateStartScreenCommand { get; }
        public ICommand NavigateToGameCommand { get; }

        public RelayCommand<Game> DeleteGameCommand { get; }
        public RelayCommand<Game> JoinRedCommand { get; }
        public RelayCommand<Game> JoinBlueCommand { get; }
        public RelayCommand<Game> JoinGreenCommand { get; }
        public RelayCommand<Game> JoinYellowCommand { get; }


        private ObservableCollection<CreateGameDTO> gamesList;
        public ObservableCollection<CreateGameDTO> GamesList
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

        public JoinGameViewModel(NavigationStore navigationStore, CurrPlayersStore currPlayersStore, NetworkService networkService)
        {
            _currPlayersStore = currPlayersStore;
            _networkService = networkService;
            //navigation
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            NavigateToGameCommand = new NavigateCommand<GameViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<GameViewModel>());
            //commands
            DeleteGameCommand = new RelayCommand<Game>(DeleteGame);
            

            JoinRedCommand = new RelayCommand<Game>(g => JoinGame(g, PieceColor.Red ));
            JoinBlueCommand = new RelayCommand<Game>(g => JoinGame(g, PieceColor.Blue));
            JoinGreenCommand = new RelayCommand<Game>(g => JoinGame(g, PieceColor.Green));
            JoinYellowCommand = new RelayCommand<Game>(g => JoinGame(g, PieceColor.Yellow));

            //load games
            LoadData();
        }

        
        //display games
        private async void LoadData()
        {
            //Send
            try
            {
               
                await _networkService.ConnectAsync("127.0.0.1", 5000);

                var envelope = new MessageEnvelope
                {
                    MessageType = "GetAllGamesAndPlayers",
                    Payload = JsonSerializer.Serialize("empty")
                };
                var json = JsonSerializer.Serialize(envelope);
                //Send and receive json of games
                var res = await _networkService.SendMessageAsync(json);

                var gamesList = JsonSerializer.Deserialize<ObservableCollection<CreateGameDTO>>(res);
                //display
                GamesList = gamesList;

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
                    
                    await _networkService.ConnectAsync("127.0.0.1", 5000);

                    var envelope = new MessageEnvelope
                    {
                        MessageType = "DeleteGame",
                        Payload = JsonSerializer.Serialize(game)
                    };
                    var json = JsonSerializer.Serialize(envelope);
                    //Send json of game
                    var res = await _networkService.SendMessageAsync(json);
                    var resp = JsonSerializer.Deserialize<string>(res);
                    System.Windows.MessageBox.Show(resp);
                   

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Connection failed: {ex.Message}");
                }
                LoadData(); //reload displays
            }
        }
        //join this game as this color
        public async void JoinGame(Game game,PieceColor pieceColor)
        {
            try
            {
                
                await _networkService.ConnectAsync("127.0.0.1", 5000);

                JoinGameDTO joinGameDTO = new JoinGameDTO()
                {
                    Game = game,
                    PieceColor = pieceColor
                }; 

                var envelope = new MessageEnvelope
                {
                    MessageType = "JoinGame",
                    Payload = JsonSerializer.Serialize(joinGameDTO)
                };

                var json = JsonSerializer.Serialize(envelope);
                //Send json of game
                var res = await _networkService.SendMessageAsync(json);
                var dto = JsonSerializer.Deserialize<JoinGameResponse>(res);
                //ensure this player isnt already connected
                if (!dto.Success)
                {
                    System.Windows.MessageBox.Show(dto.Message);
                    return;
                }

                

                ObservableCollection<Player> GamePlayers = new ObservableCollection<Player>(dto.Players);
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
