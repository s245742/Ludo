using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.GameLogic
{
    public static class PiecePositionCodec
    {
        public const int Home = 0;          // hjemme
        public const int GoalBase = 100;    // 100..104 = goal-path step 0..4
        public const int GoalValue = 200;   // mål

        public static bool IsHome(int spaceIndex) => spaceIndex <= 0; // tolerant for gamle -1-data
        public static bool IsTrack(int spaceIndex) => spaceIndex >= 1 && spaceIndex <= 52;
        public static bool IsGoalPath(int spaceIndex) => spaceIndex >= GoalBase && spaceIndex < GoalBase + 10; // 100..109
        public static bool IsGoal(int spaceIndex) => spaceIndex == GoalValue;

        public static int EncodeGoalPathStep(int step) => GoalBase + step; // step 0..4
        public static int DecodeGoalPathStep(int spaceIndex) => spaceIndex - GoalBase;
    }
}
