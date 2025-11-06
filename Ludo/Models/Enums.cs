using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public enum SpaceType
    {
        Empty, 
        Normal, 
        Star, 
        Globe, 
        Start, 
        Home, 
        Goal
    }
    public enum PieceColor : int { Red = 0, Green = 1, Blue = 2, Yellow = 3 }

    public enum CellType
    {
        Empty,
        Red,
        Green,
        Blue,
        Yellow,
        Fields
    }

}
