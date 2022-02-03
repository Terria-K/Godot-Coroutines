using System;

namespace Godot.Coroutines
{
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
