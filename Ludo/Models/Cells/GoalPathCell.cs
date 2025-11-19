using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models.Cells
{
    public class GoalPathCell : Cell
    {
        public PieceColor PathColor { get; }
        public int StepIndex { get; }

        public GoalPathCell(int cellIndex, PieceColor pathColor, int stepIndex) 
            : base(cellIndex, CellType.GoalPath)
        {
            PathColor = pathColor;
            StepIndex = stepIndex;
        }

        public bool addPiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if (piece.Color != this.PathColor)
            {
                throw new InvalidOperationException("Piece color does not match GoalPathCell color.");
            }
            if (this.Pieces.Count >= 4)
            {
                throw new InvalidOperationException("GoalPathCell already has maximum number of pieces.");
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
                throw new InvalidOperationException("Piece not found in GoalPathCell.");
            }
            this.Pieces.Remove(piece);
            return true;
        }
    }
}
