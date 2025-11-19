using Ludo.Models;
using Ludo.Models.Cells;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace Ludo.ViewModels.InGameViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public int CellIndex { get; }
        public CellType Type { get; }
        public PieceColor? Color { get; }
        public ObservableCollection<PieceViewModel> Pieces { get; } = new();

        public Brush CellColor => Type switch
        {
            CellType.Path => Brushes.LightGray,
            CellType.Home => Color switch
            {
                PieceColor.Red => Brushes.LightPink,
                PieceColor.Blue => Brushes.LightBlue,
                PieceColor.Green => Brushes.LightGreen,
                PieceColor.Yellow => Brushes.LightYellow,
                _ => Brushes.Gray
            },
            CellType.Goal => Color switch
            {
                PieceColor.Red => Brushes.Red,
                PieceColor.Blue => Brushes.DodgerBlue,
                PieceColor.Green => Brushes.LimeGreen,
                PieceColor.Yellow => Brushes.Gold,
                _ => Brushes.Gray

            },
            CellType.GoalPath => Color switch
            { 
                PieceColor.Red => Brushes.LightPink,
                PieceColor.Blue => Brushes.LightBlue,
                PieceColor.Green => Brushes.LightGreen,
                PieceColor.Yellow => Brushes.LightYellow,
                _ => Brushes.Gray
            },
            CellType.Empty => Brushes.Transparent,
            _ => Brushes.Transparent
        };

        public CellViewModel(Cell cell)
        {
            CellIndex = cell.CellIndex;
            Type = cell.CellType;
            if (cell is HomeCell home) Color = home.Color;
            else if (cell is GoalCell goal) Color = goal.Color;
            else if (cell is GoalPathCell goalPath) Color = goalPath.PathColor;
        }

       


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}