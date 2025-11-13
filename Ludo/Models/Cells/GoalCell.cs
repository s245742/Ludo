using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class GoalCell : Cell
    {
        public PlayerColor Color { get; }
        public GoalCell(int cellIndex, PlayerColor color)
            : base(cellIndex, CellType.Goal)
        {
            Color = color;
        }
    }
}
