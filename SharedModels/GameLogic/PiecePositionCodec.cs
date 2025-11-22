using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.GameLogic
{
    public static class PiecePositionCodec
    {
        public const int Home = 0;        // hjemme
        public const int PathStart = 1;
        public const int GoalPathStart = 100;    // 100..104 = goal-path step 0..4
        public const int GoalValue = 200;
        public const int GoalPathEnd = GoalPathStart + 4;
        public const int PathEnd = 51;    // sidste plads på stien (efter denne kommer goal-path)


        public static bool IsHome(int spaceIndex)
        { 
            return spaceIndex == Home;
        } // tolerant for gamle -1-data
        public static bool IsPath(int spaceIndex) => spaceIndex >= 1 && spaceIndex <= PathEnd;
        public static bool IsGoalPath(int spaceIndex) => spaceIndex >= GoalPathStart && spaceIndex < GoalPathEnd; // 100..105
        public static bool IsGoal(int spaceIndex) => spaceIndex == GoalValue;




        public static bool IsEndPath(int spaceIndex)
        {
            return (spaceIndex == PathEnd);
        }

        public static bool IsStartPath(int spaceIndex)
        {
            return spaceIndex == 1;
        }

        public static bool isEndGoalPath(int spaceIndex)
        {
            return (spaceIndex == GoalPathEnd);
        }

        public static bool isStartGoalPath(int spaceIndex)
        {
            return spaceIndex == GoalPathStart;
        }



        public static int EncodeGoalPathStep(int step) => GoalPathStart + step; // step 0..4
        public static int DecodeGoalPathStep(int spaceIndex) => spaceIndex - GoalPathStart;
    }
}
