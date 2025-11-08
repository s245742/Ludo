using Ludo.Commands;
using Ludo.Models;
using Ludo.Services;
using Ludo.Stores;
using Ludo.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
namespace Ludo.ViewModels.PreGameViewModels
{
    public class CreateGameViewModel : GameValidationViewModel
    {

        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;
        public ICommand NavigateStartScreenCommand { get; }

        private ObservableCollection<Game> games = new ObservableCollection<Game>();

        public RelayCommand SaveCommand { get; }
        


        public CreateGameViewModel(NavigationStore navigationStore, GameService gameService, PlayerService playerService, GamePieceService gamePieceService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;

            //navigation
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore,
                () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>()); //navigate to startscreen (DI makes complex to get viewmodel)

            InitalizeNewGame();

            SaveCommand = new RelayCommand(Save);
        }

        
        //create game trykkes
        private void Save()
        {
            //CHECK VALIDATION ERRORS
            var errors = GetAllErrors().ToList();
            if (checkIfGameNameExists(CurrGame.Game_Name))
                errors.Add("Game Name already exists, try another");

            
            if (errors.Any())
            {
                string message = string.Join("\n", errors);
                System.Windows.MessageBox.Show(message, "Validation Errors");
                return;
            }

            // Save game
            _gameService.CreateGame(CurrGame);
            //add each player and piece to DB
            foreach (var player in GamePlayers)
            {
                player.Game_Name = CurrGame.Game_Name;
                _playerService.CreatePlayer(player);

                foreach (var piece in player.PlayerPieces) { 
                _gamePieceService.CreateGamePiece(piece);
                }
            
            }
            System.Windows.MessageBox.Show("Game succesfully created.");


        }
        


        private void InitalizeNewGame()
        {
            //new game
            CurrGame = new Game();

            //new player
            GamePlayers.Clear();
            GamePlayers.Add(new Player(PieceColor.Red));
            GamePlayers.Add(new Player(PieceColor.Green));
            GamePlayers.Add(new Player(PieceColor.Blue));
            GamePlayers.Add(new Player(PieceColor.Yellow));

            foreach (Player player in GamePlayers)
            {
                for (int i = 1; i < 5; i++)
                {
                    player.PlayerPieces.Add(new Piece(player.Color, i,0));
                }
            }

        }
        //helper method
        private bool checkIfGameNameExists(string name)
        {
            games = _gameService.getAll();
            foreach (var game in games)
            {
                if (game.Game_Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

    }
}

    

    
