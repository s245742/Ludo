using SharedModels.GameLogic;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
   public class GameLogicIntegration
    {
        [Fact]
        public void Integration_CaptureEnemyPiece_Works()
        {
            // Arrange
            var board = BoardFactory.CreateBoard();

            var red = new Player(PieceColor.Red); 
            red.InitPieces();
            var blue = new Player(PieceColor.Blue); 
            blue.InitPieces();

            var players = new ObservableCollection<Player> { red, blue };
            var game = new Game(board, players);

            var redPiece = red.PlayerPieces[0];
            var bluePiece = blue.PlayerPieces[0];

            //common tile for both = 10 
            int commonTile = 10;

            // convert common to each color SpaceIndex
            redPiece.SpaceIndex =
                ((commonTile - GameBoardDefinitions.PathOffsets[PieceColor.Red] + 52) % 52) + 1;

            bluePiece.SpaceIndex =
                ((commonTile - GameBoardDefinitions.PathOffsets[PieceColor.Blue] + 52) % 52) + 1;

            // Act – Red moves 0 (because they are already on the same tile)
            var moved = game.MovePiece(redPiece, 0);

            // Assert: blue must be home
            Assert.Equal(PiecePositionCodec.Home, bluePiece.SpaceIndex);
        }



        [Fact]
        public void Integration_MoveOutOfHome_WithSix_EntersTrack()
        {
            // Arrange
            var board = BoardFactory.CreateBoard();

            var red = new Player(PieceColor.Red);
            red.InitPieces();

            var players = new ObservableCollection<Player> { red };
            var game = new Game(board, players);

            var piece = red.PlayerPieces[0];

            piece.SpaceIndex = PiecePositionCodec.Home;

            // Act
            game.MovePiece(piece, 6);

            // Assert – Should be on start tile
            Assert.Equal(PiecePositionCodec.PathStart, piece.SpaceIndex);
        }



    }
}
