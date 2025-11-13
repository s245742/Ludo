
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ludo.Models;
using System;
using System.Collections.Generic;

namespace Ludo.Tests.Models
{
    [TestClass]
    public class BoardTests
    {
        private Board CreateTestBoard()
        {
            var path = new PathCell[10];
            for (int i = 0; i < 10; i++)
                path[i] = new PathCell(i, i, PathType.Normal);

            var goalPaths = new Dictionary<PlayerColor, GoalPathCell[]>
            {
                [PlayerColor.Red] = new GoalPathCell[5]
            };
            for (int i = 0; i < 5; i++)
                goalPaths[PlayerColor.Red][i] = new GoalPathCell(i, PlayerColor.Red, i);

            var goalCells = new Dictionary<PlayerColor, GoalCell>
            {
                [PlayerColor.Red] = new GoalCell(99, PlayerColor.Red)
            };

            var homeCells = new Dictionary<PlayerColor, HomeCell[]>
            {
                [PlayerColor.Red] = new HomeCell[4]
            };
            for (int i = 0; i < 4; i++)
                homeCells[PlayerColor.Red][i] = new HomeCell(i, PlayerColor.Red, i);

            var players = new[] { new Player(PlayerColor.Red, 0, 0) };

            return new Board(path, goalPaths, goalCells, homeCells, players);
        }

        [TestMethod]
        public void SetPieceInHomeCell_ShouldAddPiece()
        {
            var board = CreateTestBoard();
            var piece = new Piece(PlayerColor.Red, 0);

            bool result = board.SetPieceInHomeCell(0, piece);

            Assert.IsTrue(result);
            Assert.IsTrue(board.HomeCells[PlayerColor.Red][0].Pieces.Contains(piece));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetPieceInHomeCell_ShouldThrow_WhenPieceIsNull()
        {
            var board = CreateTestBoard();
            board.SetPieceInHomeCell(0, null);
        }
    }
}

