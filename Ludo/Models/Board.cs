using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoClient.Models
{
    public class Board
    {
        public Space[] Spaces { get; }

        int Dice { get; set; }
        public Board()
        {
            Spaces = new Space[225];



            for (int i = 0; i < 255; i++)
            {
               

            }
     

        }


        public Space GetSpace(int index)
        {
            return Spaces[index];
        }

        public void MovePiece(Piece piece, int newSpaceIndex)
        {
            var currentSpace = GetSpace(piece.SpaceIndex);
            var newSpace = GetSpace(newSpaceIndex);
            // Remove piece from current space
            currentSpace.Pieces.Remove(piece);
            // Update piece's space index
            piece.SpaceIndex = newSpaceIndex;
            // Add piece to new space
            newSpace.Pieces.Add(piece);
        }

        public void MoveSteps(Piece piece, int steps)
        {
            int currentSpaceIndex = piece.SpaceIndex;
        
            
        }


    }
}
