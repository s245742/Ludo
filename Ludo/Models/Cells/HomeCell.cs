using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models.Cells
{
    public class HomeCell : Cell
    {
        public PieceColor Color { get; }
        public int SlotIndex { get; }
        public HomeCell(int cellIndex, PieceColor color, int slotIndex)
            : base(cellIndex, CellType.Home)
        {
            Color = color;
            SlotIndex = slotIndex;
        }

        public bool addPiece(Piece piece)
        {
            if(piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if(piece.Color != this.Color)
            {
                throw new InvalidOperationException("Piece color does not match HomeCell color.");
            }
            if(this.Pieces.Count >= 1)
            {
                throw new InvalidOperationException("HomeCell already has a piece.");
            }
            this.Pieces.Add(piece);
            return true;
        }
        public bool removePiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if (!this.Pieces.Contains(piece))
            {
                throw new InvalidOperationException("Piece not found in HomeCell.");
            }
            this.Pieces.Remove(piece);
            return true;
        }
    }
}
