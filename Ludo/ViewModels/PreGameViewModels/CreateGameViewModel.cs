using Ludo;
using SharedModels.Models;
using SharedModels.TransferMsg;
using LudoClient.ViewModels.Base;
using LudoClient;
using LudoClient.Commands;
using LudoClient.Models;
using LudoClient.Services;
using LudoClient.Stores;
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
using SharedModels.Models.DTO;
namespace LudoClient.ViewModels.PreGameViewModels
{
    public class CreateGameViewModel : GameValidationViewModel
    {

        public ICommand NavigateStartScreenCommand { get; }
        public RelayCommand SaveCommand { get; }
        private readonly NetworkService _networkService;
        public CreateGameViewModel(NavigationStore navigationStore, NetworkService networkService)
        {
           
            //navigation
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore,
                () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>()); //navigate to startscreen (DI makes complex to get viewmodel)

            InitalizeNewGame();
            _networkService = networkService;
            SaveCommand = new RelayCommand(Save);

        }

        
        //create game trykkes
        private async void Save()
        {
            //CHECK inital VALIDATION ERRORS
            var errors = GetAllErrors().ToList();
            if (errors.Any())
            {
                string message = string.Join("\n", errors);
                System.Windows.MessageBox.Show(message, "Validation Errors");
                return;
            }

            //create dto
            foreach (var player in GamePlayers)
            {
                player.Game_Name = CurrGame.Game_Name; //make them match before send over to dto
            }

            CreateGameDTO DTO = new CreateGameDTO
            {
                Game = CurrGame,
                Players = GamePlayers.ToList()
            };

            //Send
            try
            {
                
                await _networkService.ConnectAsync("127.0.0.1", 5000);

                var envelope = new MessageEnvelope
                {
                    MessageType = "CreateNewGame",
                    Payload = System.Text.Json.JsonSerializer.Serialize(DTO)
                };
                var json = System.Text.Json.JsonSerializer.Serialize(envelope);
                //Send JSON to server
                var res = await _networkService.SendMessageAsync(json);

                //error
                if (res.Equals("error, GameName already exists"))
                {
                    errors.Add("Game Name already exists, try another");
                    string message = string.Join("\n", errors);
                    System.Windows.MessageBox.Show(message, "Validation Errors");
                    return;
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Connection failed: {ex.Message}");
            }
            System.Windows.MessageBox.Show("Game succesfully created.");


        }

        private void InitalizeNewGame()
        {
            CurrGame = new Game();

            GamePlayers.Clear();
            GamePlayers.Add(new Player(PieceColor.Red));
            GamePlayers.Add(new Player(PieceColor.Green));
            GamePlayers.Add(new Player(PieceColor.Blue));
            GamePlayers.Add(new Player(PieceColor.Yellow));

            foreach (Player player in GamePlayers)
            {
                for (int i = 0; i < 4; i++)
                {
                    // SpaceIndex = -1 betyder "i hjemmet"
                    player.PlayerPieces.Add(new Piece(player.Color, i, 0));
                }
            }
        }

        //private void InitalizeNewGame()
        //{
        //    //new game
        //    CurrGame = new Game();

        //    //new player
        //    GamePlayers.Clear();
        //    GamePlayers.Add(new Player(PieceColor.Red));
        //    GamePlayers.Add(new Player(PieceColor.Green));
        //    GamePlayers.Add(new Player(PieceColor.Blue));
        //    GamePlayers.Add(new Player(PieceColor.Yellow));

        //    foreach (Player player in GamePlayers)
        //    {
        //        for (int i = 1; i < 5; i++)
        //        {
        //            player.PlayerPieces.Add(new Piece(player.Color, i,-1));
        //        }
        //    }

        //}
        //helper method
        //private bool checkIfGameNameExists(string name)
        //{
        //    games = _gameService.getAll();
        //    foreach (var game in games)
        //    {
        //        if (game.Game_Name.Equals(name))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

    }
}

    

    
