using LudoClient.Models;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using Ludo.Models;
using System.ComponentModel;
using System.Windows.Media;

namespace LudoClient.ViewModels.InGameViewModels
{
    public class PieceViewModel : INotifyPropertyChanged
    {
        public Piece ModelPiece { get; }
        public int SpaceIndex
        {
            get => ModelPiece.SpaceIndex;
            set
            {
                if (ModelPiece.SpaceIndex != value)
                {
                    ModelPiece.SpaceIndex = value;
                    OnPropertyChanged(nameof(SpaceIndex));
                }
            }
        }

        public Brush Fill => ModelPiece.Color switch
        {
            PieceColor.Red => Brushes.Red,
            PieceColor.Blue => Brushes.Blue,
            PieceColor.Green => Brushes.Green,
            PieceColor.Yellow => Brushes.Yellow,
            _ => Brushes.Gray
        };

        private double _visualSize = 40.0;
        public double VisualSize
        {
            get => _visualSize;
            set
            {
                if (_visualSize != value)
                {
                    _visualSize = value;
                    OnPropertyChanged(nameof(VisualSize));
                }
            }
        }


        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { if (_isSelected != value) { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); } }
        }


        public PieceViewModel(Piece piece)
        {
            ModelPiece = piece;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}