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
using System.Security.Cryptography.X509Certificates;

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
        public PieceColor PlayerWon { get; set; } = PieceColor.None;

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
        public List<Piece> MovePiece(Piece piece, int steps)
        {
            List<Piece> PiecesToMove = new List<Piece>();
            PiecesToMove.Add(piece);

            if (Board == null || Players == null)
                return PiecesToMove;

            // kan ikke flytte en brik der er i mål
            if (PiecePositionCodec.IsGoal(piece.SpaceIndex))
            {
                return PiecesToMove;
            }

            rules.MovePieceSteps(piece, steps);

            // hvis landing på stjerne
            rules.MovePieceToNextStar(piece);

            
            


            int countPlayer = 0;

            foreach (var player in Players)
            {
                foreach (var otherPiece in player.PlayerPieces)
                {
                    if (piece.isPieceSameIndex(otherPiece) &&
                        !piece.isPieceSameColor(otherPiece) &&
                        piece != otherPiece &&
                        otherPiece.isAtPath())
                    {
                        countPlayer++;
                    }
                }
            }

            // CAPTURE LOGIC
            if (countPlayer > 1)
            {
                if (Board.Path[piece.GetCommonPathIndex() - 1].OwnedBy == piece.Color)
                {
                    foreach (var player in Players)
                    {
                        foreach (var otherPiece in player.PlayerPieces)
                        {
                            if (piece.isPieceSameIndex(otherPiece) &&
                                !piece.isPieceSameColor(otherPiece) &&
                                otherPiece != piece)
                            {
                                rules.MovePieceBackToHome(otherPiece);
                                PiecesToMove.Add(otherPiece);
                            }
                        }
                    }
                }
                else
                {
                    rules.MovePieceBackToHome(piece);
                    PiecesToMove.Add(piece);
                }
            }

            if (countPlayer == 1)
            {
                foreach (var player in Players)
                {
                    foreach (var otherPiece in player.PlayerPieces)
                    {
                        if (piece.isPieceSameIndex(otherPiece) &&
                            !piece.isPieceSameColor(otherPiece) &&
                            piece != otherPiece)
                        {
                            if (otherPiece.isAtPath() &&
                                Board.Path[piece.GetCommonPathIndex() - 1].Type == PathType.Globe)
                            {
                                if (Board.Path[piece.GetCommonPathIndex() - 1].OwnedBy == piece.Color)
                                {
                                    rules.MovePieceBackToHome(otherPiece);
                                    PiecesToMove.Add(otherPiece);
                                }
                                else
                                {
                                    rules.MovePieceBackToHome(piece);
                                    PiecesToMove.Add(piece);
                                }
                            }
                            else
                            {
                                rules.MovePieceBackToHome(otherPiece);
                                PiecesToMove.Add(otherPiece);
                            }
                        }
                    }
                }
            }

            return PiecesToMove;
        }



        public bool IsPositionOccupied(int index)
        {
            if (Players == null) return false;
            return Players.SelectMany(p => p.PlayerPieces).Any(p => p.SpaceIndex == index);
        }



    }
}
