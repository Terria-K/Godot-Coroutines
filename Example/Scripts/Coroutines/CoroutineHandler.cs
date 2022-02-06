using System.Collections;
using System.Collections.Generic;

namespace Godot.Coroutines
{
    public class CoroutineHandler : Node
    {
        private static readonly List<IEnumerator> routines = new List<IEnumerator>();
        private static readonly List<IEnumerator> physicsRoutines = new List<IEnumerator>();
        private static readonly List<IEnumerator> lateRoutines = new List<IEnumerator>();

        public override void _Process(float delta)
        {
            if (routines.Count > 0)
            CoroutineAutoload.HandleCoroutines(routines);
            CallDeferred(nameof(LateProcess));
        }

        public override void _PhysicsProcess(float delta)
        {
            if (physicsRoutines.Count > 0)
            {
                CoroutineAutoload.HandleCoroutines(physicsRoutines);
            }
        }

        private void LateProcess()
        {
            if (lateRoutines.Count > 0)
            {
                CoroutineAutoload.HandleCoroutines(lateRoutines);
            }
        }

        public Coroutine StartCoroutine(IEnumerator method, CoroutineType coroutineType = CoroutineType.Process)
        {
            switch (coroutineType)
            {
                case CoroutineType.Process:
                    routines.Add(method);
                    break;
                case CoroutineType.PhysicsProcess:
                    physicsRoutines.Add(method);
                    break;
                case CoroutineType.LateProcess:
                    lateRoutines.Add(method);
                    break;
            }
            return new Coroutine(method);
        }

        public void StopCoroutine(IEnumerator method)
        {
            routines.Remove(method);
        }
        public void StopAllCoroutines()
        {
            routines.Clear();
        }
    }
}
