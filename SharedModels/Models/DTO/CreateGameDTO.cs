using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models.DTO
{
    public class CreateGameDTO
    {
        public Game Game { get; set; }
        public List<Player> Players { get; set; } // note not observableColl since this is used in backend

        //these lambdas we use so the frontend (view) can acces the playername
        public string RedPlayer => Players.FirstOrDefault(p => p.Color == PieceColor.Red).Player_Name;
        public string BluePlayer => Players.FirstOrDefault(p => p.Color == PieceColor.Blue)?.Player_Name;
        public string GreenPlayer => Players.FirstOrDefault(p => p.Color == PieceColor.Green)?.Player_Name;
        public string YellowPlayer => Players.FirstOrDefault(p => p.Color == PieceColor.Yellow)?.Player_Name;
    }
}
