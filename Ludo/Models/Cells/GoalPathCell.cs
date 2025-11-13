using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class GoalPathCell : Cell
    {
        public PlayerColor PathColor { get; }
        public int StepIndex { get; }

        public GoalPathCell(int cellIndex, PlayerColor pathColor, int stepIndex) 
            : base(cellIndex, CellType.GoalPath)
        {
            PathColor = pathColor;
            StepIndex = stepIndex;
        }
    }
}
