using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Ludo.Models;

namespace Ludo.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private readonly Cell _model;

        public CellViewModel(Cell model)
        {
            _model = model;
            Type = model.CellType;

            if (model is PathCell pathCell)
                PathType = pathCell.Type;

            // Tilføj eksisterende brikker fra modellen
            foreach (var piece in _model.Pieces)
            {
                Pieces.Add(new PieceViewModel(piece, 40));
            }

            // Lyt til ændringer i Pieces
            _model.Pieces.CollectionChanged += Pieces_CollectionChanged;

            // Juster størrelser
            RecalculatePieceSizes();
        }

        public CellType Type { get; }
        public PathType PathType { get; private set; } = PathType.Normal;

        public bool IsStar => PathType == PathType.Star;
        public bool IsGlobe => PathType == PathType.Globe;

        public ObservableCollection<PieceViewModel> Pieces { get; } = new();


        public void UpdatePathType(PathType newType)
        {
            PathType = newType;
            OnPropertyChanged(nameof(PathType));
            OnPropertyChanged(nameof(IsStar));
            OnPropertyChanged(nameof(IsGlobe));
        }


        private void Pieces_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Piece newPiece in e.NewItems!)
                {
                    Pieces.Add(new PieceViewModel(newPiece, 40));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Piece oldPiece in e.OldItems!)
                {
                    var vmToRemove = Pieces.FirstOrDefault(p => p.SlotIndex == oldPiece.SlotIndex);
                    if (vmToRemove != null)
                        Pieces.Remove(vmToRemove);
                }
            }

            RecalculatePieceSizes();
        }

        public Brush CellColor => Type switch
        {
            CellType.Empty => Brushes.LightSteelBlue,
            CellType.Path => Brushes.White,
            CellType.GoalPath => GetGoalPathColor(),
            CellType.Home => GetHomeColor(),
            CellType.Goal => GetGoalColor(),
            _ => Brushes.White
        };




        private Brush GetGoalPathColor()
        {
            if (_model is GoalPathCell goalPathCell)
            {
                return goalPathCell.PathColor switch
                {
                    PlayerColor.Red => Brushes.IndianRed,
                    PlayerColor.Blue => Brushes.LightBlue,
                    PlayerColor.Green => Brushes.LightGreen,
                    PlayerColor.Yellow => Brushes.LightYellow,
                    _ => Brushes.White
                };
            }
            return Brushes.White;
        }

        private Brush GetHomeColor()
        {
            if (_model is HomeCell homeCell)
            {
                return homeCell.Color switch
                {
                    PlayerColor.Red => Brushes.IndianRed,
                    PlayerColor.Blue => Brushes.LightBlue,
                    PlayerColor.Green => Brushes.LightGreen,
                    PlayerColor.Yellow => Brushes.LightYellow,
                    _ => Brushes.White
                };
            }
            return Brushes.White;
        }

        private Brush GetGoalColor()
        {
            if (_model is GoalCell goalCell)
            {
                return goalCell.Color switch
                {
                    PlayerColor.Red => Brushes.IndianRed,
                    PlayerColor.Blue => Brushes.LightBlue,
                    PlayerColor.Green => Brushes.LightGreen,
                    PlayerColor.Yellow => Brushes.LightYellow,
                    _ => Brushes.White
                };
            }
            return Brushes.White;
        }

        private void RecalculatePieceSizes()
        {
            int baseSize = 40;
            int decrement = 10;
            for (int i = 0; i < Pieces.Count; i++)
            {
                Pieces[i].Size = Math.Max(10, baseSize - (i * decrement));
            }
        }

        public void MergePathType(PathType newType)
        {
            if (newType == PathType.Star || newType == PathType.Globe)
            {
                PathType = newType;
                OnPropertyChanged(nameof(PathType));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
