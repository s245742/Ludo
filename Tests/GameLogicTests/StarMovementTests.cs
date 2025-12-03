using SharedModels.GameLogic;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.GameLogicTests
{
    public class StarMovementTests
    {
        [Fact]
        public void MovePiece_LandsOnStar_Red_MovesNormally()
        {
            //arrange
            var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
            var piece = player.PlayerPieces[0];

            int offset = GameBoardDefinitions.PathOffsets[PieceColor.Red]; // 42

            //red first star in common path = 1
            int firstStarCommon = GameBoardDefinitions.Stars[0];

            // Convert common to red SpaceIndex
            int firstRedStarSpaceIndex =
                ((firstStarCommon - offset + 52) % 52) + 1;

            // place piece on red star
            piece.SpaceIndex = firstRedStarSpaceIndex;

            //act 
            game.MovePiece(piece, 1);

            //Assert expected normal movement, no star jump
            int expected = firstRedStarSpaceIndex + 1;
            Assert.Equal(expected, piece.SpaceIndex);
        }



        [Fact]
        public void MovePiece_LandsOnStar_Green_JumpsCorrectly()
        {
            //arramge
            var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Green);
            var piece = player.PlayerPieces[0];

            //offset is 3
            int offset = GameBoardDefinitions.PathOffsets[PieceColor.Green];

            //first Star 
            int firstStarCommon = GameBoardDefinitions.Stars[0];

            //convert to SpaceIndex
            int firstGreenStarSpaceIndex =
                ((firstStarCommon - offset + 52) % 52) + 1;

            piece.SpaceIndex = firstGreenStarSpaceIndex;

            //act 
            game.MovePiece(piece, 1);

            //assert
            Assert.Equal(PiecePositionCodec.GoalPathStart, piece.SpaceIndex); // normally 100
        }

        [Fact]
        public void MovePiece_LandOnNextStar()
        {
            //arrange
            var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
            var piece = player.PlayerPieces[0];

            piece.SpaceIndex = 1; // 104
                                    //act
            game.MovePiece(piece, 5);
            //assert
            Assert.Equal(12, piece.SpaceIndex); // 200

        }

        [Fact]
        public void MovePiece_LandOnLastStarHopToGoal()
        {
            //arrange
            var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
            var piece = player.PlayerPieces[0];

            piece.SpaceIndex = 48; // 104
                                  //act
            game.MovePiece(piece, 3);
            //assert
            Assert.Equal(PiecePositionCodec.GoalValue, piece.SpaceIndex); // 200

        }

    }
}
