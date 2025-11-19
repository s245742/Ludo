using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.Cells
{
    public class PathCell : Cell
    {
        public int PathIndex { get; }
        public PathType Type { get; set; }
        public PieceColor OwnedBy { get; set; }
        public PathCell(int cellIndex, int pathIndex, PathType type) 
            : base(cellIndex, CellType.Path)
        {
            PathIndex = pathIndex;
            Type = type;
        }

        public bool IsOccupied()
        {
            return this.Pieces.Count > 0;
        }

        public bool IsSafeZone()
        {
            return this.Type == PathType.Star || this.Type == PathType.Globe;
        }

        public PieceColor? GetOccupyingColor()
        {
            if (!this.IsOccupied())
            {
                return null;
            }
            return this.Pieces[0].Color;
        }

        public bool addPiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if (this.IsOccupied() && piece.Color != GetOccupyingColor())
            {
                return false;
            }
            // cant have more than 4 pieces in a path cell
            if (this.Pieces.Count >= 4)
            {
                throw new InvalidOperationException("PathCell already has maximum number of pieces.");
            }
            
            this.Pieces.Add(piece);
            Size++;
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
                return false;
            }
            this.Pieces.Remove(piece);
            Size--;
            return true;
        }
    }
}
