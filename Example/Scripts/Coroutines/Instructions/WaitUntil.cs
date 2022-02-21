using System;

namespace Godot.Coroutines
{
    [Obsolete("Use the while keyword and make the condition opposite instead!")]
    public sealed class WaitUntil : YieldInstruction
    {
        private Func<bool> UntilCall { get; }
        public WaitUntil(Func<bool> action)
        {
            this.UntilCall = action;
        }

        public override bool Condition => !UntilCall();
    }

}
