using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public sealed class Space
    {
        public int Index { get; set; }

        public SpaceType Type { get; set; } = SpaceType.Empty;
        public PieceColor? HomeColor { get; set; }

        public ObservableCollection<Piece> Pieces { get; set; } = new ObservableCollection<Piece>();

        public bool IsOccupied => Pieces.Count > 0;

        // tilføj andre relevante properties og metoder efter behov herunder

    }
}
