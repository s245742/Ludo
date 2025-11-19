using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.GameLogic
{
    public enum MoveResult
    {
        StayedHome,
        EnteredTrack,
        MovedOnTrack,
        EnteredGoalPath,
        MovedOnGoalPath,
        ReachedGoal
    }

    public enum MoveEventType
    {
        None,
        EnteredTrack,
        EnteredGoalPath,
        ReachedGoal,
        KnockedOpponent,
        LandedOnSafe
    }

    public record MoveEvent(MoveEventType Type, string? Note = null);

    public record MoveOutcome(
        MoveResult Result,
        int OldSpaceIndex,
        int NewSpaceIndex,
        IReadOnlyList<MoveEvent> Events
    );
}
