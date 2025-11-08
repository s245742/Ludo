using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public class Player
    {
        public ObservableCollection<GamePiece> PlayerPieces { get; }

        public int Player_ID { get; set; }
        public string Game_Name { get; set; }
        public string Player_Name { get; set; }

        public  string Color { get; set; }
        
        public Player(String color)
        {
            this.Color = color;
            PlayerPieces = new ObservableCollection<GamePiece>();
        }


    }
}
