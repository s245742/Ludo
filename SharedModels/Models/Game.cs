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

            // TODO: muligvis ænding til LINQ:
            // Find alle spillere der er samme sted og sæt i en liste--> tæl listen --> hvis flere send sig selv hjem ellers send listen(0) hjem
            // flyt spiller hen på rigtige plads
            if (Board == null || Players == null) return null;
            rules.MovePieceSteps(piece, steps);

            //var a = Board.Path[0].OwnedBy = PieceColor.None;

            int countPlayer = 0;
            // Tæl hvor mange andre spillere der er
            foreach (var player in Players)
            {
                foreach (var otherPiece in player.PlayerPieces)
                {
                    if (piece.isPieceSameIndex(otherPiece) && !piece.isPieceSameColor(otherPiece) && piece != otherPiece && otherPiece.isAtPath())
                    {
                            countPlayer++;                       
                    }
                }
            }
            // hvis der er nogen der står på globus:

            // send sig selv hjem hvis der er flere andre brikker på pladsen eller på globus
            if ((countPlayer > 1 && countPlayer <5))
            {
                // hvis flere brikker er på en hjemmeglobus
                if (Board.Path[piece.GetCommonPathIndex() - 1].OwnedBy == piece.Color)
                {
                    foreach (var player in Players)
                    {
                        foreach (var otherPiece in player.PlayerPieces)
                        {
                            if (piece.isPieceSameIndex(otherPiece) && !piece.isPieceSameColor(otherPiece) && piece != otherPiece)
                            {
                                    rules.MovePieceBackToHome(otherPiece);
                            }
                        }
                    }

                }
                else
                {
                    rules.MovePieceBackToHome(piece);
                }
            }
            if (countPlayer == 1)
            {
                //find other piece med samme index og send hjem
                foreach (var player in Players)
                {
                    foreach (var otherPiece in player.PlayerPieces)
                    {
                        if (piece.isPieceSameIndex(otherPiece) && !piece.isPieceSameColor(otherPiece) && piece != otherPiece)
                        {
                            if (otherPiece.isAtPath() && Board.Path[piece.GetCommonPathIndex() - 1].Type == PathType.Globe)
                            {
                                if (Board.Path[piece.GetCommonPathIndex()-1].OwnedBy == piece.Color)
                                { 
                                    rules.MovePieceBackToHome(otherPiece); 
                                }
                                else
                                {
                                    rules.MovePieceBackToHome(piece);
                                }
                            }
                            else
                            {
                                rules.MovePieceBackToHome(otherPiece);
                            }
                        }
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
