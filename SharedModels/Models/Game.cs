using SharedModels.GameLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Models.Cells;
using SharedModels.Models;

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
        public StdLudoMoveRules rules = new StdLudoMoveRules();

        // Behold den gamle constructor (ingen ændring)
        public Game() { }

        // Ny constructor til InGameViewModels
        public Game(Board board, ObservableCollection<Player> players)
        {
            Board = board;
            Players = players;
        }


        // Flyttelogik
        public Piece MovePiece(Piece piece, int steps)
        {
            if (Board == null || Players == null) return null;
            rules.MovePieceSteps(piece, steps);

            // Håndter slå ud logik
            foreach (var player in Players)
            {
                foreach (var otherPiece in player.PlayerPieces)
                {
                    if (piece.isPieceSameIndex(otherPiece) && !piece.isPieceSameColor(otherPiece))
                    {
                        rules.MovePieceBackToHome(otherPiece);
                    }

                }
            }
            return piece;
        }


        public bool IsPositionOccupied(int index)
        {
            if (Players == null) return false;
            return Players.SelectMany(p => p.PlayerPieces).Any(p => p.SpaceIndex == index);
        }



    }
}
