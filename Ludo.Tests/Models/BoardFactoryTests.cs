
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ludo.Models;
using System.Linq;

namespace Ludo.Tests.Models
{
    [TestClass]
    public class BoardFactoryTests
    {
        [TestMethod]
        public void CanCreateBoard_UsingBoardFactory()
        {
            // Act
            var board = BoardFactory.CreateBoard();

            // Assert
            Assert.IsNotNull(board);
            Assert.AreEqual(4, board.Player.Length, "Board should have 4 players.");
            Assert.IsTrue(board.Path.Length > 0, "Path should not be empty.");
        }

        [TestMethod]
        public void CanPlacePieceInHomeCell()
        {
            // Arrange
            var board = BoardFactory.CreateBoard();
            var player = board.Player.First(p => p.Color == PlayerColor.Red);
            var piece = player.Pieces[0];

            // Act
            bool result = board.SetPieceInHomeCell(0, piece);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(board.HomeCells[PlayerColor.Red][0].Pieces.Contains(piece));
        }

        [TestMethod]
        public void CanMovePieceFromHomeToPath()
        {
            // Arrange
            var board = BoardFactory.CreateBoard();
            var player = board.Player.First(p => p.Color == PlayerColor.Red);
            var piece = player.Pieces[0];

            // Først placer i Home
            board.SetPieceInHomeCell(0, piece);

            // Act: Fjern fra Home og flyt til Path
            board.RemovePieceFromHomeCell(0, piece);
            bool moved = board.setPieceInPathCell(player.PathEntryIndex, piece);

            // Assert
            Assert.IsTrue(moved);
            Assert.IsTrue(board.Path[player.PathEntryIndex].Pieces.Contains(piece));
        }
    }
}
