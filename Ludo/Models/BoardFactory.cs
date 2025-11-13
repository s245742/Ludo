using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Models
{
    public static class BoardFactory
    {
        public static Board CreateBoard()
        {
            if (GameViewBoardDefinition.Path.Length == 0)
            {
                throw new InvalidOperationException("GameViewBoardDefinition.Path is empty.");
            }

            // Create Path Cells array
            var pathCells = new PathCell[GameViewBoardDefinition.Path.Length];
            for (int i = 0; i < GameViewBoardDefinition.Path.Length; i++)
            {
                int cellIndex = GameViewBoardDefinition.Path[i];
                PathType type;
                PlayerColor ownedBy = PlayerColor.None;

                if (GameBoardDefinitions.Stars.Contains(i))
                {
                    type = PathType.Star;
                }

                else if (GameBoardDefinitions.Globes.Contains(i))
                {
                    type = PathType.Globe;
                }
                else
                {
                    type = PathType.Normal;
                }

                pathCells[i] = new PathCell(

                    cellIndex: cellIndex,
                    pathIndex: i,
                    type: type
                );
                Debug.WriteLine($"PathIndex: {i}, CellIndex: {cellIndex}, Type: {type}");

                // set home entry cells
                SetHomeEntryCells(i, pathCells[i]);
            }

            // Create each colors goal paths array
            var goalPaths = new Dictionary<PlayerColor, GoalPathCell[]>
            {
                [PlayerColor.Green] = BuildGoalPath(PlayerColor.Green, GameViewBoardDefinition.GreenHome),
                [PlayerColor.Yellow] = BuildGoalPath(PlayerColor.Yellow, GameViewBoardDefinition.YellowHome),
                [PlayerColor.Red] = BuildGoalPath(PlayerColor.Red, GameViewBoardDefinition.RedHome),
                [PlayerColor.Blue] = BuildGoalPath(PlayerColor.Blue, GameViewBoardDefinition.BlueHome),
            };

            // Create each colors goal cells
            var goals = new Dictionary<PlayerColor, GoalCell>
            {
                [PlayerColor.Green] = new GoalCell(GameViewBoardDefinition.GreenGoal, PlayerColor.Green),
                [PlayerColor.Yellow] = new GoalCell(GameViewBoardDefinition.YellowGoal, PlayerColor.Yellow),
                [PlayerColor.Red] = new GoalCell(GameViewBoardDefinition.RedGoal, PlayerColor.Red),
                [PlayerColor.Blue] = new GoalCell(GameViewBoardDefinition.BlueGoal, PlayerColor.Blue),
            };

            // Create each colors home cells array
            var homeCells = new Dictionary<PlayerColor, HomeCell[]>
            {
                [PlayerColor.Green] = BuildHomeCells(PlayerColor.Green, GameViewBoardDefinition.GreenStart),
                [PlayerColor.Yellow] = BuildHomeCells(PlayerColor.Yellow, GameViewBoardDefinition.YellowStart),
                [PlayerColor.Red] = BuildHomeCells(PlayerColor.Red, GameViewBoardDefinition.RedStart),
                [PlayerColor.Blue] = BuildHomeCells(PlayerColor.Blue, GameViewBoardDefinition.BlueStart),
            };

            // Create Players
            var players = new Player[]
            {
                new Player(PlayerColor.Green, GameBoardDefinitions.GreenHomeEntry, GameBoardDefinitions.GreenGoalEntry),
                new Player(PlayerColor.Yellow, GameBoardDefinitions.YellowHomeEntry, GameBoardDefinitions.YellowGoalEntry),
                new Player(PlayerColor.Red, GameBoardDefinitions.RedHomeEntry, GameBoardDefinitions.RedGoalEntry),
                new Player(PlayerColor.Blue, GameBoardDefinitions.BlueHomeEntry, GameBoardDefinitions.BlueGoalEntry),
            };

            

            var board = new Board(pathCells, goalPaths, goals, homeCells, players);

            return board;
        }


        // static helper methods to build goal paths and home cells
        private static GoalPathCell[] BuildGoalPath(PlayerColor color, int[] goalPath)
        {
            var arr = new GoalPathCell[goalPath.Length];
            for (int i = 0; i < goalPath.Length; i++)
            {
                int cellIndex = goalPath[i];
                arr[i] = new GoalPathCell(
                    cellIndex: cellIndex,
                    pathColor: color,
                    stepIndex: i
                );
            }
            return arr;
        }

        private static HomeCell[] BuildHomeCells(PlayerColor color, int[] homePositions)
        {
            var arr = new HomeCell[homePositions.Length];
            for (int i = 0; i < homePositions.Length; i++)
            {
                int cellIndex = homePositions[i];
                arr[i] = new HomeCell(cellIndex,color,i);
            }
            return arr;
        }

        private static void SetHomeEntryCells(int pathIndex, PathCell cell)
        {
            if (pathIndex == GameBoardDefinitions.GreenHomeEntry)
            {
                cell.OwnedBy = PlayerColor.Green;
            }
            else if (pathIndex == GameBoardDefinitions.YellowHomeEntry)
            {
                cell.OwnedBy = PlayerColor.Yellow;
            }
            else if (pathIndex == GameBoardDefinitions.RedHomeEntry)
            {
                cell.OwnedBy = PlayerColor.Red;
            }
            else if (pathIndex == GameBoardDefinitions.BlueHomeEntry)
            {
                cell.OwnedBy = PlayerColor.Blue;
            }
        }

    }
}
