using Ludo.Models.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class Piece
    {
        public int Player_ID { get; set; } //sent to db

        public int SpaceIndex { get; set; } //sent to db
        public PieceColor Color { get; init; } //can derive from player
        public int SlotIndex { get; init; } //sent to db

        public bool IsAtStart { get; set; } = true; //can derive from boardpos
        public bool IsFinished { get; set; } //can derive from boardpos

        public Piece(PieceColor color, int slotIndex, int spaceIndex)
        {
            Color = color;
            SlotIndex = slotIndex;
            SpaceIndex = spaceIndex;
        }

        // Metode til at opdatere position
        public void MoveTo(int newSpaceIndex)
        {
            SpaceIndex = newSpaceIndex;
        }

        public bool IsAtPosition(int index) => SpaceIndex == index;

    }
}
