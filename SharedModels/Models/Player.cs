using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public class Player
    {
        public ObservableCollection<Piece> PlayerPieces { get; }
        
        public int Player_ID { get; set; }
        public string Game_Name { get; set; }
        public string Player_Name { get; set; }

        public  PieceColor Color { get; set; }
        
        public Player(PieceColor color)
        {
            Color = color;
            PlayerPieces = new ObservableCollection<Piece>();
        }


    }
}
