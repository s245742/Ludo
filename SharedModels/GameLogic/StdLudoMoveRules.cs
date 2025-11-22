using SharedModels.Models;
using SharedModels.Models.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.GameLogic
{
    public class StdLudoMoveRules
    {
        public bool FwdDirection { get; set; } = true;

        public StdLudoMoveRules()
        {
        }
        public bool MovePieceOneStep(Piece piece)
        {
            // hvis piece er på sidste plads i path, så flyt til goal path start
            if (PiecePositionCodec.IsEndPath(piece.SpaceIndex))
            {
                piece.SpaceIndex = PiecePositionCodec.GoalPathStart;
                return true;
            }

            // hvis piece er på sidste plads i pieces goalpath så flyt til goal 
            if (PiecePositionCodec.isEndGoalPath(piece.SpaceIndex))
            {
                piece.SpaceIndex = PiecePositionCodec.GoalValue;
                piece.IsFinished = true;
                return true;
            }

            // hvis piece er i mål, så flyt piece til Goal Path end
            if (PiecePositionCodec.IsGoal(piece.SpaceIndex))
            {
                MovePieceBackToGoalPath(piece);
                FwdDirection = false;
                return true;
            }
            piece.SpaceIndex += 1;
            return true;
        }


        public bool MovePieceSteps(Piece piece, int steps)
        {
            if (PiecePositionCodec.IsHome(piece.SpaceIndex))
            {
                if (steps == 6)
                {
                    MovePieceFromHomeToTrack(piece);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            FwdDirection = true;
            for(int i = 0; i < steps;i++)
            {
                if (FwdDirection == false)
                {
                    MovePieceOneBackStep(piece);
                }
                else
                {
                    MovePieceOneStep(piece);
                }
            }
            return true;
            // Implementation of standard Ludo move rules for multiple steps
        }

        public bool MovePieceBackToHome(Piece piece)
        {
            piece.MoveTo(PiecePositionCodec.Home);
            return true;
            // Implementation of sending a piece back to home
        }

        public bool MovePieceFromHomeToTrack(Piece piece)
        {
            piece.SpaceIndex = PiecePositionCodec.PathStart;
            return true;
            // Implementation of moving a piece from home to the track
        }
        public bool MovePieceToGoalPath(Piece piece)
        {
            piece.SpaceIndex = PiecePositionCodec.GoalPathStart;
            return true;
            // Implementation of moving a piece to the goal path
        }

        public bool MovePieceToGoal(Piece piece)
        {
            piece.SpaceIndex = PiecePositionCodec.GoalValue;
            return true;
            // Implementation of moving a piece to the goal
        }


        public bool IsPieceAtHome(Piece piece)
        {
            return (PiecePositionCodec.IsHome(piece.SpaceIndex));
            // Implementation of logic to check if a piece is at home
        }

        public bool MovePieceBackSteps(Piece piece, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                MovePieceOneBackStep(piece);
            }
            FwdDirection = true;
            return true;
        }

        public bool MovePieceOneBackStep(Piece piece)
        {
            if (PiecePositionCodec.isStartGoalPath(piece.SpaceIndex))
            {
                MovePieceBackToPath(piece);
                return true;
            }
            if (PiecePositionCodec.IsStartPath(piece.SpaceIndex))
            {
                MovePieceBackToHome(piece);
                return true;
            }
            piece.SpaceIndex -= 1;
            return false;
            // Implementation of moving a piece back one step
        }
        public bool MovePieceBackToGoalPath(Piece piece)
        {
            piece.SpaceIndex = PiecePositionCodec.GoalPathEnd;
            return true;
            // Implementation of moving a piece back to the goal path
        }

        public bool MovePieceBackToPath(Piece piece)
        {
            piece.SpaceIndex = PiecePositionCodec.PathEnd;
            return true;
            // Implementation of moving a piece back to the track
        }

        



    }
}
