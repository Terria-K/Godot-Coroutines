using System.Collections;

namespace Godot.Coroutines
{
    public sealed class Coroutine : YieldInstruction
    {
        public readonly IEnumerator routine;
        public Coroutine(IEnumerator routine)
        {
            this.routine = routine;
        }
        public override bool Condition => routine.MoveNext();
    }
}

