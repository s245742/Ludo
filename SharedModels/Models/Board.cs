using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedModels.Models.Cells;

namespace SharedModels.Models;

public class Board
{
    public PathCell[] Path { get; }
    public Dictionary<PieceColor, GoalPathCell[]> GoalPaths { get; }
    public Dictionary<PieceColor, GoalCell> GoalCells { get; }
    public Dictionary<PieceColor, HomeCell[]> HomeCells { get; }
    public Player[] Player { get; }

    public int Dice { get; set; }

    public Board(
        PathCell[] path,
        Dictionary<PieceColor, GoalPathCell[]> goalPaths,
        Dictionary<PieceColor, GoalCell> goalCellls,
        Dictionary<PieceColor, HomeCell[]> homeCells,
        Player[] player)
    {
        Path = path;
        GoalPaths = goalPaths;
        GoalCells = goalCellls;
        HomeCells = homeCells;
        Player = player;
    }

    public bool IsPositionOccupied(int index, IEnumerable<Piece> allPieces)
    {
        return allPieces.Any(p => p.SpaceIndex == index);
    }

    public bool isSafeZone(int index)
    {
        return Path[index].IsSafeZone();
    }


    // methods to move pieces around the board






}


