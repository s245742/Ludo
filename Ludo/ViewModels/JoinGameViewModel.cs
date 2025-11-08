using Ludo.Commands;
using Ludo.Models;
using Ludo.Services;
using Ludo.Stores;
using Ludo.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Ludo.ViewModels
{
    public class JoinGameViewModel : ViewModelBase
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;

        public ICommand NavigateStartScreenCommand { get; }

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


        public JoinGameViewModel(NavigationStore navigationStore, GameService gameService, PlayerService playerService, GamePieceService gamePieceService)
        {
            _gameService = new GameService();
            _playerService = new PlayerService();
            _gamePieceService = new GamePieceService();
            
            //navigation
            this.NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            //commands
            deleteGameCommand = new RelayCommand<Game>(DeleteGame);
            joinGameCommand = new RelayCommand<Game>(JoinGame);

            LoadData();
        }

        
        //helpermethod display games
        private void LoadData()
        {
            GamesList = _gameService.getAll();
        }
        

        //param game is passed with command.parameter (note we need a generic relaycommand here sicne delete taks a param)
        public void DeleteGame(Game game)
        {
            var result = MessageBox.Show($"Are you sure you want to delete game \"{game.Game_Name}\"?",
                         "Confirmation",
                         MessageBoxButton.YesNo,
                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //call gameservices (Should be transaction, but w/e)
                _gamePieceService.deleteGamePiecesfromGame(game);
                _playerService.deletePlayersfromGame(game);
                _gameService.delete(game);
                LoadData(); //reload displays
            }
        }

        public void JoinGame(Game game)
        {
            //Get player list
            ObservableCollection<Player> GamePlayers = _playerService.getAllPlayersFromGame(game);
            //
            foreach (Player player in GamePlayers)
            {
                ObservableCollection<GamePiece> gp = new ObservableCollection<GamePiece>();
                   gp = _gamePieceService.getAllGamePieceFromPlayer(player);
                foreach (GamePiece gamepiece in gp)
                {
                    player.PlayerPieces.Add(gamepiece);
                }
               
            }
           // we now have loaded the player and playerpieces which we can serialize to make game :)

        }



    }
}
