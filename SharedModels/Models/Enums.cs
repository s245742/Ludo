using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public enum PathType
    {
        Normal,
        PathEntry,
        Star,
        Globe,
        GoalEntry
    }
    public enum PieceColor : int { Red = 0, Green = 1, Blue = 2, Yellow = 3 }

    public enum CellType
    {
        Empty,
        Path,
        GoalPath,
        Home,
        Goal
    }

}


