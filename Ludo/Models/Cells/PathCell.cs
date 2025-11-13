using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class PathCell : Cell
    {
        public int PathIndex { get; }
        public PathType Type { get; set; }
        public PlayerColor OwnedBy { get; set; } = PlayerColor.None;
        public PathCell(int cellIndex, int pathIndex, PathType type) 
            : base(cellIndex, CellType.Path)
        {
            PathIndex = pathIndex;
            Type = type;
            OwnedBy = PlayerColor.None;

        }


    }
}
