using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.GameLogicTests
{
    public static class TestUtils
    {
        public static (Game game, Player player) CreateSinglePlayerTest(PieceColor color)
        {

            var board = BoardFactory.CreateBoard();

            var players = new ObservableCollection<Player>
    {
        new Player(color)
    };

            players[0].InitPieces();

            return (new Game(board, players), players[0]);
        }

    }
}
