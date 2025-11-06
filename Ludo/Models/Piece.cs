using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class Piece
    {

        public PieceColor Color { get; init; }
        public int SlotIndex { get; init; }
        public int SpaceIndex {get; set; }
        public bool IsAtStart { get; set; } = true;
        public bool IsFinished { get; set; }

        public Piece(PieceColor color, int slotIndex, int spaceIndex)
        {
            Color = color;
            SlotIndex = slotIndex;
            SpaceIndex = spaceIndex;
        }



    }
}
