using Ludo;
using Ludo.Models;
using Ludo.ViewModels.InGameViewModels;
using LudoClient.Commands;
using LudoClient.Models;
using LudoClient.Services;
using LudoClient.Stores;
using LudoClient.ViewModels.Base;
using LudoClient.ViewModels.PreGameViewModels;
using Microsoft.Extensions.DependencyInjection;
using SharedModels.Models;
using SharedModels.Models.Cells;
using SharedModels.TransferMsg;
using System.Collections.ObjectModel;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

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
        public RelayCommand SaveGameCommand { get; }

        public ICommand PingCommand { get; }

        private string _pingText;
        public string PingText
        {
            get => _pingText;
            set { _pingText = value; OnPropertyChanged(nameof(PingText)); }
        }


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
        public ICommand MoveSelectedPieceCommand { get; }



        public GameViewModel(NavigationStore navigationStore, CurrPlayersStore currPlayersStore, NetworkService networkService)
        {
            _networkService = networkService;
            NavigateStartScreenCommand = new NavigateCommand<StartScreenViewModel>(navigationStore, () => App.ServiceProvider.GetRequiredService<StartScreenViewModel>());
            gamePlayers = currPlayersStore.GamePlayers;
            PingCommand = new RelayCommand(SendPing);


            //tilføjet for at initialisere spillet med bræt og spillere
            _game = new Game(BoardFactory.CreateBoard(), currPlayersStore.GamePlayers);

            GenerateBoard();
            if (_game.Players != null && _game.Players.Any())
                PlaceAllPieces();
            //GeneratePieces();


            SelectPieceCommand = new RelayCommand<PieceViewModel>(SelectPiece);
            MoveSelectedPieceCommand = new RelayCommand(MoveSelectedPiece);

            PlaceAllPieces();


          
        }
        public async Task InitializeAsync()
        {
            if (!_networkService.IsConnected)
                await _networkService.ConnectAsync("127.0.0.1", 5000);

            // Start listening on a background thread
            _ = Task.Run(() => _networkService.StartListeningAsync(OnMessage));
        }


        private void ReselectByModelPiece(Piece modelPiece)
        {
            var found = BoardCells.SelectMany(c => c.Pieces)
                                  .FirstOrDefault(pv => ReferenceEquals(pv.ModelPiece, modelPiece));
            foreach (var cell in BoardCells)
                foreach (var pv in cell.Pieces)
                    pv.IsSelected = false;

            if (found != null)
            {
                found.IsSelected = true;
                SelectedPieceVM = found;
            }
            else
            {
                SelectedPieceVM = null;
            }
        }




        private void MoveSelectedPiece()
        {
            if (SelectedPieceVM == null || MoveSteps == 0) return;

            var modelPiece = SelectedPieceVM.ModelPiece;
            _game.MovePiece(modelPiece, MoveSteps);

            PlaceAllPieces();
            ReselectByModelPiece(modelPiece);
        }



        // === NYT: vælg en brik ved klik ===

        private void SelectPiece(PieceViewModel? pvm)
        {
            // Nulstil tidligere selection
            foreach (var cell in BoardCells)
                foreach (var pv in cell.Pieces)
                    pv.IsSelected = false;

            SelectedPieceVM = pvm;
            if (SelectedPieceVM != null)
                SelectedPieceVM.IsSelected = true;
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

            // 2) Tilføj brikker til korrekt celle
            foreach (var player in _game.Players)
            {
                foreach (var piece in player.PlayerPieces)
                {
                    int gridIndex = ToGridIndex(piece);
                    var targetCell = BoardCells[gridIndex];
                    targetCell.Pieces.Add(new PieceViewModel(piece));
                }
            }

            // 3) (Valgfrit) Stacking: gør flere brikker af samme farve i samme celle mindre lag-på-lag
            foreach (var cell in BoardCells)
            {
                if (cell.Pieces.Count <= 1) continue;

                var groups = cell.Pieces.GroupBy(p => p.ModelPiece.Color);
                foreach (var g in groups)
                {
                    double[] sizes = new[] { 40.0, 30.0, 20.0, 10.0 };
                    int i = 0;
                    foreach (var pv in g)
                    {
                        pv.VisualSize = sizes[Math.Min(i, sizes.Length - 1)];
                        i++;
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

                if (GameViewBoardDefinition.Path.Contains(i))
                {
                    cell = new PathCell(i, i, PathType.Normal);
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

                BoardCells.Add(new CellViewModel(cell));
            }
        }

        //private void GeneratePieces()
        //{
        //    foreach (var player in _game.Players)
        //        foreach (var piece in player.PlayerPieces)
        //            Pieces.Add(new PieceViewModel(piece));
        //}



        private string _pingLabelText;
        public string PingLabelText
        {
            get => _pingLabelText;
            set
            {
                _pingLabelText = value;
                OnPropertyChanged(nameof(PingLabelText));
            }
        }
        private void OnMessage(string msg)
        {
            try
            {
                var envelope = JsonSerializer.Deserialize<MessageEnvelope>(msg);
                if (envelope?.MessageType == "PingResponse")
                {
                    string text = JsonSerializer.Deserialize<string>(envelope.Payload);
                    Application.Current.Dispatcher.Invoke(() => PingText = text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing message: " + ex.Message);
            }
        }

        private async void SendPing()
        {
            if (!_networkService.IsConnected) return;

            var envelope = new MessageEnvelope
            {
                MessageType = "Ping",
                Payload = JsonSerializer.Serialize("Hello from client!")
            };

            string json = JsonSerializer.Serialize(envelope);
            await _networkService.SendAsync(json);
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

        // GameViewModel.cs
        private int ToGridIndex(Piece piece)
        {
            // Hjemme: SpaceIndex < 0 -> vis i farvens startfelt efter SlotIndex (0..3)
            if (piece.SpaceIndex == 0)
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

            if (piece.SpaceIndex > GameViewBoardDefinition.Path.Length) // I mål: SpaceIndex > Path.Length -> vis i farvens målzone
            {
                int homeIndex = piece.SpaceIndex - GameViewBoardDefinition.Path.Length - 1; // 0..3
                return piece.Color switch
                {
                    PieceColor.Green => GameViewBoardDefinition.GreenHome[homeIndex],
                    PieceColor.Blue => GameViewBoardDefinition.BlueHome[homeIndex],
                    PieceColor.Yellow => GameViewBoardDefinition.YellowHome[homeIndex],
                    PieceColor.Red => GameViewBoardDefinition.RedHome[homeIndex],
                    _ => 0
                };
            }

            if(piece.SpaceIndex < 1) // ugyldig SpaceIndex
                return 0;
            if (piece.SpaceIndex > GameViewBoardDefinition.Path.Length + GameViewBoardDefinition.GreenHome.Length)
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




    }
}
