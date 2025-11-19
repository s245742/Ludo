using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Cells
{
    public class GoalCell : Cell
    {
        public PieceColor Color { get; }
        public GoalCell(int cellIndex, PieceColor color)
            : base(cellIndex, CellType.Goal)
        {
            Color = color;
        }

        public bool addPiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if (piece.Color != this.Color)
            {
                throw new InvalidOperationException("Piece color does not match GoalCell color.");
            }
            if (this.Size >= 4)
            {
                throw new InvalidOperationException("GoalCell already has maximum number of pieces.");
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
                throw new InvalidOperationException("Piece not found in GoalCell.");
            }
            this.Pieces.Remove(piece);
            return true;
        }
    }
}
