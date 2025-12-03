using SharedModels.GameLogic;
using SharedModels.Models;
using System.Collections.ObjectModel;
using UnitTests.GameLogicTests;
using Xunit;



public class GameTests
{
    [Fact]
    public void MovePiece_OutOfHome_WithSix_ShouldEnterTrack()
    {
        //arangge
        var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
        var piece = player.PlayerPieces[0];

        piece.SpaceIndex = PiecePositionCodec.Home;
        //act
        game.MovePiece(piece, 6);
        //assert
        Assert.Equal(PiecePositionCodec.PathStart, piece.SpaceIndex); // 1
    }

    [Fact]
    public void MovePiece_NormalMovement_IncrementsPosition()
    {
        //arrange
        var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
        var piece = player.PlayerPieces[0];

        piece.SpaceIndex = 5;
        //act
        game.MovePiece(piece, 3);
        //assert
        Assert.Equal(8, piece.SpaceIndex);
    }




    [Fact]
    public void MovePiece_EnteringGoalPath()
    {
        //arramge
        var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
        var piece = player.PlayerPieces[0];

        piece.SpaceIndex = PiecePositionCodec.PathEnd; // 51
        //act
        game.MovePiece(piece, 1);
        //asssert
        Assert.Equal(PiecePositionCodec.GoalPathStart, piece.SpaceIndex); // 100
    }

    [Fact]
    public void MovePiece_ReachingGoal()
    {
        //arrange
        var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
        var piece = player.PlayerPieces[0];

        piece.SpaceIndex = PiecePositionCodec.GoalPathEnd; // 104
        //act
        game.MovePiece(piece, 1);
        //assert
        Assert.Equal(PiecePositionCodec.GoalValue, piece.SpaceIndex); // 200
        Assert.True(piece.IsFinished);
    }

    [Fact]
    public void MovePiece_ReachingGoalStepBack()
    {
        //arrange
        var (game, player) = TestUtils.CreateSinglePlayerTest(PieceColor.Red);
        var piece = player.PlayerPieces[0];

        piece.SpaceIndex = 102; // 104
        //act
        game.MovePiece(piece, 5);
        //assert
        Assert.Equal(103, piece.SpaceIndex); // 200
        
    }
}


