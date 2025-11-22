using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.DTO
{
    public class MovePieceDto
    {
        public int Player_ID { get; set; }       // for DB updates
        public PieceColor Color { get; set; }    // identifies the piece color
        public int SlotIndex { get; set; }       // identifies which piece of that color
        public int SpaceIndex { get; set; }      // the final position after move
    }
}

