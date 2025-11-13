using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ludo.Tests.Models;
using Ludo.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Ludo.Tests.Models
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void CanCreateBoard()
        {
            // Arrange: Lav dummy data til Board
            var path = new PathCell[1] { new PathCell(0, 0, PathType.Normal) };

            var goalPaths = new Dictionary<PlayerColor, GoalPathCell[]>
            {
                [PlayerColor.Red] = new GoalPathCell[1] { new GoalPathCell(1, PlayerColor.Red, 0) }
            };

            var goalCells = new Dictionary<PlayerColor, GoalCell>
            {
                [PlayerColor.Red] = new GoalCell(99, PlayerColor.Red)
            };

            var homeCells = new Dictionary<PlayerColor, HomeCell[]>
            {
                [PlayerColor.Red] = new HomeCell[1] { new HomeCell(2, PlayerColor.Red, 0) }
            };

            var players = new[] { new Player(PlayerColor.Red, 0, 0) };

            // Act: Opret Board
            var board = new Board(path, goalPaths, goalCells, homeCells, players);

            // Assert: Board er ikke null
            Assert.IsNotNull(board);
            Assert.AreEqual(1, board.Path.Length);
            Assert.AreEqual(PlayerColor.Red, board.Player[0].Color);
        }
    }
}