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
        public ObservableCollection<Piece> PlayerPieces { get; set; }
        
        public int Player_ID { get; set; }
        public string Game_Name { get; set; }
        public string Player_Name { get; set; }

        public  PieceColor Color { get; set; }
        
        public Player(PieceColor color)
        {
            Color = color;
            PlayerPieces = new ObservableCollection<Piece>();
        }

        public bool InitPieces()
        {
            for (int i = 0; i < 4; i++)
            {
                Piece piece = new Piece(this.Color, i, -i-1); // (-1..-4) indicates the piece is in home
                PlayerPieces.Add(piece);
            }
            return true;
        }
        public bool RemovePiece(Piece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }
            if (!this.PlayerPieces.Contains(piece))
            {
                throw new InvalidOperationException("Piece not found in Player's pieces.");
            }
            this.PlayerPieces.Remove(piece);
            return true;
        }


    }
}
