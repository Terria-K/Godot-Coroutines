using System;

namespace Godot.Coroutines
{
    [Obsolete("Use the float instead!")]
    public sealed class WaitForSeconds : YieldInstruction
    {
        private readonly float finishTime;

        public WaitForSeconds(float seconds)
        {
            finishTime = Time.time + seconds;
        }

        public override bool Condition => finishTime > Time.time;
    }
}

