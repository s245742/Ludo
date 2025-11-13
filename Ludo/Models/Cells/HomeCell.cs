using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class HomeCell : Cell
    {
        public PlayerColor Color { get; }
        public int SlotIndex { get; }
        public HomeCell(int cellIndex, PlayerColor color, int slotIndex)
            : base(cellIndex, CellType.Home)
        {
            Color = color;
            SlotIndex = slotIndex;
        }
    }
}
