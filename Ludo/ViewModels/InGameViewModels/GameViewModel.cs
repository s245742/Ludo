using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Ludo;
using LudoClient.Services;
using LudoClient.Stores;
using LudoClient.Models;
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.PreGameViewModels;
using LudoClient.Commands;
using SharedModels.Models;

namespace LudoClient.ViewModels.InGameViewModels
{
    public class GameViewModel : ViewModelBase
    {
        public ObservableCollection<BoardCellViewModel> BoardCells { get; } = new(); // skal ændres til en liste fra modelen senere

        public int[] Path => GameBoardDefinition.Path;


        private ObservableCollection<Player> gamePlayers;

        public ICommand NavigateStartScreenCommand { get; }
        public RelayCommand SaveGameCommand { get; }
        private readonly GamePieceService _gamePieceService;

        public GameViewModel(NavigationStore navigationStore, CurrPlayersStore currPlayersStore, GamePieceService gamePieceService)
        {
            _gamePieceService = gamePieceService;
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            gamePlayers = currPlayersStore.GamePlayers;
            SaveGameCommand = new RelayCommand(Save);

            GenerateBoard();
            AddDemoPieces();
            
        }

        private void GenerateBoard()
        {
            for (int i = 0; i<225; i++)
            {
                var cell = new BoardCellViewModel();
                {
                    // Determine path cells
                    if (GameBoardDefinition.Path.Contains(i))
                    {
                        cell.Type = CellType.Fields;
                    }

                    // Determine start for each color
                    else if (GameBoardDefinition.RedStart.Contains(i))
                    {
                        cell.Type = CellType.Red;
                    }
                    else if (GameBoardDefinition.BlueStart.Contains(i))
                    {
                        cell.Type = CellType.Blue;
                    }
                    else if (GameBoardDefinition.GreenStart.Contains(i))
                    {
                        cell.Type = CellType.Green;
                    }
                    else if (GameBoardDefinition.YellowStart.Contains(i))
                    {
                        cell.Type = CellType.Yellow;
                    }

                    // Determine home paths for each color
                    else if (GameBoardDefinition.RedHome.Contains(i))
                    {
                        cell.Type = CellType.Red;
                    }
                    else if (GameBoardDefinition.BlueHome.Contains(i))
                    {
                        cell.Type = CellType.Blue;
                    }
                    else if (GameBoardDefinition.GreenHome.Contains(i))
                    {
                        cell.Type = CellType.Green;
                    }
                    else if (GameBoardDefinition.YellowHome.Contains(i))
                    {
                        cell.Type = CellType.Yellow;
                    }



                    // Default to empty cell
                    else
                    {
                        cell.Type = CellType.Empty;
                    }

                    BoardCells.Add(cell);
                }
                ;
                
            }
        }

        private void AddDemoPieces()
        {
            // Adding demo pieces for testing

            var r = new Piece(PieceColor.Red, 0, GameBoardDefinition.RedStart[0]);
            var r1 = new Piece(PieceColor.Red, 1, GameBoardDefinition.RedStart[1]);
            var r2 = new Piece(PieceColor.Red, 2, GameBoardDefinition.RedStart[2]);
            var g = new Piece(PieceColor.Green, 0, GameBoardDefinition.GreenStart[0]);
            var b = new Piece(PieceColor.Blue, 0, GameBoardDefinition.BlueStart[0]);
            var y = new Piece(PieceColor.Yellow, 0, GameBoardDefinition.YellowStart[0]);

            var rvm = new PieceViewModel(r, 40);
            var r1vm = new PieceViewModel(r1, 20);

            var gvm = new PieceViewModel(g, 40);

            var bvm = new PieceViewModel(b, 40);
            var yvm = new PieceViewModel(y, 40);
            BoardCells[r.SpaceIndex].Pieces.Add(rvm);
            BoardCells[r1.SpaceIndex].Pieces.Add(r1vm);
            BoardCells[g.SpaceIndex].Pieces.Add(gvm);
            BoardCells[b.SpaceIndex].Pieces.Add(bvm);
            BoardCells[y.SpaceIndex].Pieces.Add(yvm);

            MoveToPiece(rvm, 6);
            MoveToPiece(r1vm, 6);





        }


        // test methods to move pieces around the board
        private void MoveToPiece(PieceViewModel piece, int newSpaceIndex)
        {
            if (piece.SpaceIndex >= 0 && piece.SpaceIndex < BoardCells.Count)
            {
                BoardCells[piece.SpaceIndex].Pieces.Remove(piece);
            }
            piece.SpaceIndex = newSpaceIndex;
            BoardCells[newSpaceIndex].Pieces.Add(piece);
        }

        private void MoveSteps(PieceViewModel piece, int steps)
        {
            int currentIndex = piece.SpaceIndex;
            int newIndex = (currentIndex + steps) % BoardCells.Count;
            MoveToPiece(piece, newIndex);
        }

       private void Save()
        {
            foreach (Player player in gamePlayers)
            {
                foreach (Piece gamepiece in player.PlayerPieces)
                {
                    //we get current pieceID in db, from slotindex and playerid (these shouldnt change)
                    int pieceId = _gamePieceService.GetPieceIDFromPiece(gamepiece);
                    //update this piece with new spaceindex
                    _gamePieceService.UpdatePieceFromPieceID(gamepiece, pieceId);
                }

            }
            //after update we can now return to startscreen
            NavigateStartScreenCommand.Execute(null);
        }



    }
}
