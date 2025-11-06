using Ludo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ludo.ViewModels
{
    public class PieceViewModel : INotifyPropertyChanged
    {
        private readonly Piece _piece;
        
        private bool _isSelected;
        private double _size = 40;

        public PieceViewModel(Piece piece, int size)
        {
            _piece = piece;
            _size = size;
        }

        public PieceColor Color => _piece.Color;
        public int SlotIndex => _piece.SlotIndex;
        public int SpaceIndex
        {
            get { return _piece.SpaceIndex; }
            set
            {
                if (_piece.SpaceIndex != value)
                {
                    _piece.SpaceIndex = value;
                    OnPropertyChanged(nameof(SpaceIndex));
                }
            }
        }


        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(StrokeThickness));
                }
            }
        }

        

        public double Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged(nameof(Size));
                    OnPropertyChanged(nameof(PositionX));
                    OnPropertyChanged(nameof(PositionY));
                }
            }
        }
        public double StrokeThickness
        {
            get
            {
                return IsSelected ? 1 : 2;
            }
        }

        public double PositionX => (50 - Size) / 2;
        public double PositionY => (50 - Size) / 2;


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Brush Fill => Color switch
        {
            PieceColor.Red => Brushes.Red,
            PieceColor.Blue => Brushes.Blue,
            PieceColor.Green => Brushes.Green,
            PieceColor.Yellow => Brushes.Yellow,
            _ => Brushes.Gray
        };




    }
}
