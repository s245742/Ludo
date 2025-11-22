using Ludo;
using Ludo.ViewModels.InGameViewModels;
using LudoClient.Commands;
//using LudoClient.Models;
using LudoClient.Services;
using LudoClient.Stores;
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.PreGameViewModels;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Models;
using SharedModels.Models.Cells;
using SharedModels.TransferMsg;
using SharedModels.Models.DTO;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using SharedModels.GameLogic;

namespace LudoClient.ViewModels.InGameViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private readonly NetworkService _networkService;
        //public ObservableCollection<BoardCellViewModel> BoardCells { get; } = new(); // skal ændres til en liste fra modelen senere
        private readonly Game _game;
        public ObservableCollection<CellViewModel> BoardCells { get; } = new(); // skal ændres til en liste fra modelen senere
        //public ObservableCollection<PieceViewModel> Pieces { get; } = new();


        public int[] Path => GameViewBoardDefinition.Path;


        private ObservableCollection<Player> gamePlayers;

        public ICommand NavigateStartScreenCommand { get; }
        public RelayCommand ExitGameCommand { get; }


        private PieceViewModel? _selectedPieceVM;
        public PieceViewModel? SelectedPieceVM
        {
            get => _selectedPieceVM;
            set { _selectedPieceVM = value; OnPropertyChanged(nameof(SelectedPieceVM)); }
        }


        private int _moveSteps = 1;
        public int MoveSteps
        {
            get => _moveSteps;
            set { _moveSteps = value; OnPropertyChanged(nameof(MoveSteps)); }
        }



        public ICommand SelectPieceCommand { get; }
        //public ICommand MoveSelectedPieceCommand { get; }



        public GameViewModel(NavigationStore navigationStore, CurrPlayersStore currPlayersStore, NetworkService networkService)
        {
            
            _networkService = networkService;
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            gamePlayers = currPlayersStore.GamePlayers;

            //tilføjet for at initialisere spillet med bræt og spillere
            _game = new Game(BoardFactory.CreateBoard(), currPlayersStore.GamePlayers);

            GenerateBoard();
            if (_game.Players != null && _game.Players.Any())
                PlaceAllPieces();

            SelectPieceCommand = new RelayCommand<PieceViewModel>(SelectPiece);
            ExitGameCommand = new RelayCommand(ExitGame);

            PlaceAllPieces();


          
        }
        public async Task InitializeAsync()
        {
            //if (!_networkService.IsConnected)
              //  await _networkService.ConnectAsync("127.0.0.1", 5000);

            // Start listening on a background thread
            _ = Task.Run(() => _networkService.StartListeningAsync(OnMessage));
        }

        private async void ExitGame()
        {
            try
            {
                if (_networkService.IsConnected)
                    await _networkService.DisconnectAsync();

                NavigateStartScreenCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExitGame error: " + ex.Message);
            }
        }


        private void ReselectByModelPiece(Piece modelPiece)
        {
            // Ingen visuel selection længere:
            var found = BoardCells.SelectMany(c => c.Pieces)
                                  .FirstOrDefault(pv => ReferenceEquals(pv.ModelPiece, modelPiece));
            SelectedPieceVM = found; // (valgfrit: behold reference hvis du vil)

        }



        private async void SelectPiece(PieceViewModel? pvm)
        {
            if (pvm == null) return;

            // Flyt direkte med MoveSteps
            if (MoveSteps <= 0) return;
            var modelPiece = pvm.ModelPiece;
            Piece updatedPiece = _game.MovePiece(modelPiece, MoveSteps);
            PlaceAllPieces();
            ReselectByModelPiece(modelPiece);

            //send to ´server and broadcast to other clients
            await BroadcastMoveAsync(updatedPiece);

        }

        private async Task BroadcastMoveAsync(Piece piece)
        {
            if (!_networkService.IsConnected) return;

            var moveDto = new SharedModels.Models.DTO.MovePieceDto
            {
                Player_ID = piece.Player_ID,
                Color = piece.Color,
                SlotIndex = piece.SlotIndex,
                SpaceIndex = piece.SpaceIndex
            };

            var envelope = new MessageEnvelope
            {
                MessageType = "MovePiece",
                Payload = JsonSerializer.Serialize(moveDto)
            };

            string json = JsonSerializer.Serialize(envelope);
            await _networkService.SendAsync(json);
        }



        public void MovePiece(PieceViewModel pieceVM, int steps)
        {
            _game.MovePiece(pieceVM.ModelPiece, steps);
            PlaceAllPieces();
        }

        private void PlaceAllPieces()
        {
            // 1) Tøm alle cellers visuelle brikker
            foreach (var cell in BoardCells)
                cell.Pieces.Clear();

            // 2) Byg map: gridIndex -> liste af Piece (model)
            var cellBuckets = new Dictionary<int, List<Piece>>();
            foreach (var player in _game.Players)
            {
                foreach (var piece in player.PlayerPieces)
                {
                    int gridIndex = ToGridIndex(piece);
                    if (!cellBuckets.TryGetValue(gridIndex, out var list))
                    {
                        list = new List<Piece>();
                        cellBuckets[gridIndex] = list;
                    }
                    list.Add(piece);
                }
            }

            // 3) Fyld celler med PieceViewModels m. stacking pr. farve (ingen forskydning)
            foreach (var kvp in cellBuckets)
            {
                int gridIndex = kvp.Key;
                var modelPiecesInCell = kvp.Value;
                var targetCell = BoardCells[gridIndex];

                // Gruppér KUN pr. farve
                var colorGroups = modelPiecesInCell.GroupBy(mp => mp.Color);

                foreach (var colorGroup in colorGroups)
                {
                    var models = colorGroup.ToList();

                    // Størrelse pr. lag (justér efter smag)
                    double[] sizes = { 25.0, 20.0, 15.0, 10.0 };

                    // VIGTIGT: Tilføj i rækkefølge STØRST -> MINDEST
                    // Så de små tilføjes sidst og ligger øverst i Z-orden.
                    for (int i = 0; i < models.Count; i++)
                    {
                        var pv = new PieceViewModel(models[i])
                        {
                            VisualSize = sizes[Math.Min(i, sizes.Length - 1)]
                        };

                        targetCell.Pieces.Add(pv);
                    }
                }
            }
        }
        private void GenerateBoard()
        {
            int totalCells = 225; // 15x15 grid
            for (int i = 0; i < totalCells; i++)
            {
                Cell cell;
                PieceColor ownedBy = PieceColor.None;
                if (GameViewBoardDefinition.Path.Contains(i))
                {
                    PathType type = PathType.Normal;
                    
                    if (GameBoardDefinitions.Stars.Contains(i))
                    {
                        type = PathType.Star;
                    }
                    if (GameViewBoardDefinition.Globes.Contains(i))
                    {
                        type = PathType.Globe;
                        if (i == GameViewBoardDefinition.GreenHomeEntry)
                        {
                             ownedBy = PieceColor.Green;
                        }
                        else if (i == GameViewBoardDefinition.YellowHomeEntry)
                        {
                             ownedBy = PieceColor.Yellow;
                        }
                        else if (i == GameViewBoardDefinition.RedHomeEntry)
                        {
                             ownedBy = PieceColor.Red;
                        }
                        else if (i == GameViewBoardDefinition.BlueHomeEntry)
                        {
                             ownedBy = PieceColor.Blue;
                        }
                        else
                        {
                            ownedBy = PieceColor.None;
                        }
                    }
                    cell = new PathCell(i, Array.IndexOf(GameViewBoardDefinition.Path, i), type);
                    
                }
                else if (GameViewBoardDefinition.GreenStart.Contains(i))
                {
                    cell = new HomeCell(i, PieceColor.Green, Array.IndexOf(GameViewBoardDefinition.GreenStart, i));
                }
                else if (GameViewBoardDefinition.BlueStart.Contains(i))
                {
                    cell = new HomeCell(i, PieceColor.Blue, Array.IndexOf(GameViewBoardDefinition.BlueStart, i));
                }
                else if (GameViewBoardDefinition.YellowStart.Contains(i))
                {
                    cell = new HomeCell(i, PieceColor.Yellow, Array.IndexOf(GameViewBoardDefinition.YellowStart, i));
                }
                else if (GameViewBoardDefinition.RedStart.Contains(i))
                {
                    cell = new HomeCell(i, PieceColor.Red, Array.IndexOf(GameViewBoardDefinition.RedStart, i));
                }
                else if (GameViewBoardDefinition.GreenHome.Contains(i))
                {
                    cell = new GoalPathCell(i, PieceColor.Green, Array.IndexOf(GameViewBoardDefinition.GreenHome, i));
                }
                else if (GameViewBoardDefinition.YellowHome.Contains(i))
                {
                    cell = new GoalPathCell(i, PieceColor.Yellow, Array.IndexOf(GameViewBoardDefinition.YellowHome, i));
                }
                else if (GameViewBoardDefinition.RedHome.Contains(i))
                {
                    cell = new GoalPathCell(i, PieceColor.Red, Array.IndexOf(GameViewBoardDefinition.RedHome, i));
                }
                else if (GameViewBoardDefinition.BlueHome.Contains(i))
                {
                    cell = new GoalPathCell(i, PieceColor.Blue, Array.IndexOf(GameViewBoardDefinition.BlueHome, i));
                }
                else if (i == GameViewBoardDefinition.GreenGoal)
                {
                    cell = new GoalCell(i, PieceColor.Green);
                }
                else if (i == GameViewBoardDefinition.YellowGoal)
                {
                    cell = new GoalCell(i, PieceColor.Yellow);
                }
                else if (i == GameViewBoardDefinition.RedGoal)
                {
                    cell = new GoalCell(i, PieceColor.Red);
                }
                else if (i == GameViewBoardDefinition.BlueGoal)
                {
                    cell = new GoalCell(i, PieceColor.Blue);
                }
                else
                {
                    cell = new EmptyCell(i);
                }

                BoardCells.Add(new CellViewModel(cell, ownedBy));
            }
        }

        // Når client modtager message fra server
        private void OnMessage(string msg)
        {
            try
            {
                var envelope = JsonSerializer.Deserialize<MessageEnvelope>(msg);
                if (envelope == null) return;

                switch (envelope.MessageType)
                {
                    case "MovePiece": 
                        try
                        {
                            var moveDto = JsonSerializer.Deserialize<MovePieceDto>(envelope.Payload);
                            if (moveDto == null) return;

                            var piece = _game.Players
                                .SelectMany(p => p.PlayerPieces)
                                .FirstOrDefault(p => p.Player_ID == moveDto.Player_ID &&
                                                     p.Color == moveDto.Color);
                            if (piece == null)
                            {
                                Console.WriteLine("MovePiece: piece not found for Player_ID=" + moveDto.Player_ID);
                                return;
                            }

                            piece.SpaceIndex = moveDto.SpaceIndex;

                            Application.Current.Dispatcher.Invoke(PlaceAllPieces);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed parsing MovePiece payload: " + ex.Message + " Raw payload: " + envelope.Payload);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing message envelope: " + ex.Message + " Raw message: " + msg);
            }
        }

        // Helpers to convert Piece.SpaceIndex to Grid Index
        private int ToGridIndex(Piece piece)
        {
            // Hjemme: SpaceIndex < 0 -> vis i farvens startfelt efter SlotIndex (0..3)
            if (piece.SpaceIndex == PiecePositionCodec.Home)
            {
                return piece.Color switch
                {
                    PieceColor.Green => GameViewBoardDefinition.GreenStart[piece.SlotIndex],
                    PieceColor.Blue => GameViewBoardDefinition.BlueStart[piece.SlotIndex],
                    PieceColor.Yellow => GameViewBoardDefinition.YellowStart[piece.SlotIndex],
                    PieceColor.Red => GameViewBoardDefinition.RedStart[piece.SlotIndex],
                    _ => 0
                };
            }

            // i målzone:  SpaceIndex = GoalBase + 1..GoalBase + 5 -> vis i farvens målzone
            if (piece.SpaceIndex >= PiecePositionCodec.GoalPathStart && piece.SpaceIndex <= PiecePositionCodec.GoalPathStart +4) // I mål: SpaceIndex > Path.Length -> vis i farvens målzone
            {
                int homeIndex = piece.SpaceIndex - PiecePositionCodec.GoalPathStart; // 0..3
                return piece.Color switch
                {
                    PieceColor.Green => GameViewBoardDefinition.GreenHome[homeIndex],
                    PieceColor.Blue => GameViewBoardDefinition.BlueHome[homeIndex],
                    PieceColor.Yellow => GameViewBoardDefinition.YellowHome[homeIndex],
                    PieceColor.Red => GameViewBoardDefinition.RedHome[homeIndex],
                    _ => 0
                };
            }

            if(piece.SpaceIndex < 0) // ugyldig SpaceIndex
                return 0;

            // I mål: SpaceIndex == GoalValue -> vis i farvens mål
            if (piece.SpaceIndex == PiecePositionCodec.GoalValue)
            {
                return piece.Color switch
                {
                    PieceColor.Green => GameViewBoardDefinition.GreenGoal,
                    PieceColor.Blue => GameViewBoardDefinition.BlueGoal,
                    PieceColor.Yellow => GameViewBoardDefinition.YellowGoal,
                    PieceColor.Red => GameViewBoardDefinition.RedGoal,
                    _ => 0
                };
            }

            // Ude på stien: farvens lokale SpaceIndex + offset => globalt pathIndex
            int offset = GameBoardDefinitions.PathOffsets[piece.Color];
            int pathIndex = ((piece.SpaceIndex - 1) + offset) % GameViewBoardDefinition.Path.Length;

            // Slå UniformGrid-indekset op via den globale path-oversættelse
            return GameViewBoardDefinition.Path[pathIndex];
        }

        // test methods to move pieces around the board
        private void Tester_MoveToPiece(PieceViewModel piece, int newSpaceIndex)
        {
            if (piece.SpaceIndex >= 0 && piece.SpaceIndex < BoardCells.Count)
            {
                BoardCells[piece.SpaceIndex].Pieces.Remove(piece);
            }
            piece.SpaceIndex = newSpaceIndex;
            BoardCells[newSpaceIndex].Pieces.Add(piece);
        }

    }
}
