using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class GamePiece
    {
       
        public int Player_ID { get; set; }
        public int Board_Pos { get; set; }

        public GamePiece()
        {
            Board_Pos = 0; 

        }


    }
}
