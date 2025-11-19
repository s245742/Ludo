namespace Ludo.GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Ludo.Models;

    public class StandardLudoMoveEngine : IMoveEngine
    {
        public MoveOutcome Move(Piece piece, int steps, Board board, ObservableCollection<Player> players)
        {
            if (board == null || players == null)
                return new MoveOutcome(MoveResult.StayedHome, piece.SpaceIndex, piece.SpaceIndex, Array.Empty<MoveEvent>());
            if (steps <= 0)
                return new MoveOutcome(MoveResult.StayedHome, piece.SpaceIndex, piece.SpaceIndex, Array.Empty<MoveEvent>());

            int oldIndex = piece.SpaceIndex;

            // 1) HOME -> første felt på stien
            if (PiecePositionCodec.IsHome(piece.SpaceIndex))
            {
                // (tilføj evt. 6'er-regel her)
                piece.SpaceIndex = 1;
                steps -= 1;
                if (steps == 0)
                    return new MoveOutcome(MoveResult.EnteredTrack, oldIndex, piece.SpaceIndex,
                        new[] { new MoveEvent(MoveEventType.EnteredTrack) });
                // fortsæt resten af skridtene på stien
            }

            // 2) GOAL-PATH
            if (PiecePositionCodec.IsGoalPath(piece.SpaceIndex))
            {
                int len = board.GoalPaths[piece.Color].Length; // typisk 5
                int step = PiecePositionCodec.DecodeGoalPathStep(piece.SpaceIndex);
                int newStep = step + steps;

                if (newStep >= len)
                {
                    piece.SpaceIndex = PiecePositionCodec.GoalValue; // mål
                    piece.IsFinished = true;
                    return new MoveOutcome(MoveResult.ReachedGoal, oldIndex, piece.SpaceIndex,
                        new[] { new MoveEvent(MoveEventType.ReachedGoal) });
                }
                else
                {
                    piece.SpaceIndex = PiecePositionCodec.EncodeGoalPathStep(newStep);
                    return new MoveOutcome(MoveResult.MovedOnGoalPath, oldIndex, piece.SpaceIndex,
                        new[] { new MoveEvent(MoveEventType.None, $"GoalPath step {newStep}") });
                }
            }

            // 3) TRACK (1..52) – gå skridt for skridt for at fange goal-entry præcist
            int total = GameViewBoardDefinition.Path.Length; // 52
            int offset = GameBoardDefinitions.PathOffsets[piece.Color];
            int localZeroBased = piece.SpaceIndex - 1; // 0..51

            while (steps > 0)
            {
                localZeroBased = (localZeroBased + 1) % total;
                steps--;

                int globalIndex = (localZeroBased + offset) % total;

                // goal-entry for farven, og har vi stadig steps tilbage?
                if (IsThisColorsGoalEntry(piece.Color, globalIndex) && steps > 0)
                {
                    // ind i goal-path: første felt = step 0
                    int len = board.GoalPaths[piece.Color].Length;

                    int targetStep = steps - 1; // allerede indtrådt på step 0
                    if (targetStep >= len)
                    {
                        piece.SpaceIndex = PiecePositionCodec.GoalValue;
                        piece.IsFinished = true;
                        return new MoveOutcome(MoveResult.ReachedGoal, oldIndex, piece.SpaceIndex,
                            new[] {
                                new MoveEvent(MoveEventType.EnteredGoalPath, "Entered at step 0"),
                                new MoveEvent(MoveEventType.ReachedGoal)
                            });
                    }
                    else
                    {
                        piece.SpaceIndex = PiecePositionCodec.EncodeGoalPathStep(Math.Max(0, targetStep));
                        return new MoveOutcome(MoveResult.EnteredGoalPath, oldIndex, piece.SpaceIndex,
                            new[] { new MoveEvent(MoveEventType.EnteredGoalPath, $"Step {Math.Max(0, targetStep)}") });
                    }
                }
            }

            // endt på stien uden entry
            piece.SpaceIndex = localZeroBased + 1;

            // (her kan du senere indsætte “knock-out / safe zone” logik med players.SelectMany(...) osv.)
            return new MoveOutcome(MoveResult.MovedOnTrack, oldIndex, piece.SpaceIndex, Array.Empty<MoveEvent>());
        }

        private static bool IsThisColorsGoalEntry(PieceColor color, int globalPathIndex)
        {
            return color switch
            {
                PieceColor.Green => globalPathIndex == GameBoardDefinitions.GreenGoalEntry,
                PieceColor.Blue => globalPathIndex == GameBoardDefinitions.BlueGoalEntry,
                PieceColor.Yellow => globalPathIndex == GameBoardDefinitions.YellowGoalEntry,
                PieceColor.Red => globalPathIndex == GameBoardDefinitions.RedGoalEntry,
                _ => false
            };
        }
    }
}
