using System;

namespace Godot.Coroutines
{
    [Obsolete("Use the while keyword instead!")]
    public sealed class WaitWhile : YieldInstruction
    {
        private Func<bool> WhileLoop { get; }
        public WaitWhile(Func<bool> whileLoop)
        {
            WhileLoop = whileLoop;
        }

        public override bool Condition => WhileLoop();
    }

}
