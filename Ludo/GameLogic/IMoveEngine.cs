using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.GameLogic
{
    using Ludo.Models;
    using System.Collections.ObjectModel;

    public interface IMoveEngine
    {
        MoveOutcome Move(Piece piece, int steps, Board board, ObservableCollection<Player> players);
    }
}