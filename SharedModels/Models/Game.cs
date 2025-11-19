using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.Models
{
    public class Game 
    {

        private string game_name;

        public string Game_Name 
        {
            get { return game_name; }
            set { game_name = value;}
        }


        // Ny tilføjelse: Spiltilstand
        public Board? Board { get; private set; }
        public ObservableCollection<Player>? Players { get; private set; }

        // Behold den gamle constructor (ingen ændring)
        public Game() { }

        // Ny constructor til InGameViewModels
        public Game(Board board, ObservableCollection<Player> players)
        {
            Board = board;
            Players = players;
        }


        // Flyttelogik
        public void MovePiece(Piece piece, int steps)
        {
            if (Board == null || Players == null) return;

            int newIndex = piece.SpaceIndex + steps;
            if (newIndex >= Board.Path.Length)
                newIndex -= Board.Path.Length;

            var occupiedPiece = Players.SelectMany(p => p.PlayerPieces)
                                       .FirstOrDefault(p => p.SpaceIndex == newIndex);

            if (occupiedPiece != null && !Board.Path[newIndex].IsSafeZone())
            {
                occupiedPiece.SpaceIndex = -1; // Send hjem
            }

            piece.SpaceIndex = newIndex;
        }


        public bool IsPositionOccupied(int index)
        {
            if (Players == null) return false;
            return Players.SelectMany(p => p.PlayerPieces).Any(p => p.SpaceIndex == index);
        }



    }
}
