using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class Player
    {
        public PieceColor Color { get; }
        public Piece[] Pieces { get; }
        
        public Player(PieceColor color)
        {
            Color = color;
            Pieces = new Piece[4];
            for (int i = 0; i < 4; i++)
            {
                Pieces[i] = new Piece (color, i, -1)
                { 
                    Color = color,
                    SlotIndex = i, 
                    IsAtStart = true
                };
            }
        }
    }
}
