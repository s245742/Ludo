using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LudoClient.Models;
using SharedModels.Models;
using SharedModels.Models.Cells;

namespace SharedModels.Models
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
                var ownedBy = PieceColor.None;

                if (GameBoardDefinitions.Stars.Contains(i))
                {
                    type = PathType.Star;
                }

                else if (GameBoardDefinitions.Globes.Contains(i))
                {
                    
                    if (i == GameBoardDefinitions.GreenHomeEntry)
                    {
                        ownedBy = PieceColor.Green;
                    }
                    else if (i == GameBoardDefinitions.YellowHomeEntry)
                    {
                        ownedBy = PieceColor.Yellow;
                    }
                    else if (i == GameBoardDefinitions.RedHomeEntry)
                    {
                        ownedBy = PieceColor.Red;
                    }
                    else if (i == GameBoardDefinitions.BlueHomeEntry)
                    {
                        ownedBy = PieceColor.Blue;
                    }
                    type = PathType.Globe;
                }
                else
                {
                    type = PathType.Normal;
                }

                pathCells[i] = new PathCell(
                    cellIndex: cellIndex,
                    pathIndex: i,
                    type: type);
                pathCells[i].OwnedBy = ownedBy;
                Debug.WriteLine($"PathIndex: {i}, CellIndex: {cellIndex}, Type: {type}");

                // set home entry cells
                SetHomeEntryCells(i, pathCells[i]);
            }

            // Create each colors goal paths array
            var goalPaths = new Dictionary<PieceColor, GoalPathCell[]>
            {
                [PieceColor.Green] = BuildGoalPath(PieceColor.Green, GameViewBoardDefinition.GreenHome),
                [PieceColor.Yellow] = BuildGoalPath(PieceColor.Yellow, GameViewBoardDefinition.YellowHome),
                [PieceColor.Red] = BuildGoalPath(PieceColor.Red, GameViewBoardDefinition.RedHome),
                [PieceColor.Blue] = BuildGoalPath(PieceColor.Blue, GameViewBoardDefinition.BlueHome),
            };  

            // Create each colors goal cells
            var goals = new Dictionary<PieceColor, GoalCell>
            {
                [PieceColor.Green] = new GoalCell(GameViewBoardDefinition.GreenGoal, PieceColor.Green),
                [PieceColor.Yellow] = new GoalCell(GameViewBoardDefinition.YellowGoal, PieceColor.Yellow),
                [PieceColor.Red] = new GoalCell(GameViewBoardDefinition.RedGoal, PieceColor.Red),
                [PieceColor.Blue] = new GoalCell(GameViewBoardDefinition.BlueGoal, PieceColor.Blue),
            };

            // Create each colors home cells array
            var homeCells = new Dictionary<PieceColor, HomeCell[]>
            {
                [PieceColor.Green] = BuildHomeCells(PieceColor.Green, GameViewBoardDefinition.GreenStart),
                [PieceColor.Yellow] = BuildHomeCells(PieceColor.Yellow, GameViewBoardDefinition.YellowStart),
                [PieceColor.Red] = BuildHomeCells(PieceColor.Red, GameViewBoardDefinition.RedStart),
                [PieceColor.Blue] = BuildHomeCells(PieceColor.Blue, GameViewBoardDefinition.BlueStart),
            };

            // Create Players
            var players = new Player[]
            {
                new Player(PieceColor.Green),
                new Player(PieceColor.Yellow),
                new Player(PieceColor.Red),
                new Player(PieceColor.Blue),
            };



            var board = new Board(pathCells, goalPaths, goals, homeCells, players);

            return board;
        }


        // static helper methods to build goal paths and home cells
        private static GoalPathCell[] BuildGoalPath(PieceColor color, int[] goalPath)
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

        private static HomeCell[] BuildHomeCells(PieceColor color, int[] homePositions)
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
                cell.OwnedBy = PieceColor.Green;
            }
            else if (pathIndex == GameBoardDefinitions.YellowHomeEntry)
            {
                cell.OwnedBy = PieceColor.Yellow;
            }
            else if (pathIndex == GameBoardDefinitions.RedHomeEntry)
            {
                cell.OwnedBy = PieceColor.Red;
            }
            else if (pathIndex == GameBoardDefinitions.BlueHomeEntry)
            {
                cell.OwnedBy = PieceColor.Blue;
            }
        }

    }
}
